using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PowerLog;

namespace PowerLog.Sinks.Asynchronous
{
    #region AsynchronousSink Class XML
    /// <summary>
    /// Provides an alternate sink stack in which pushed sinks are called asynchronously through Fire-and-Forget, useful in cases where synchronous sink invocation noticably affects logging performance.
    /// </summary>
    #endregion
    public class AsynchronousSink : ISink
    {
        #region Identifier String XML
        /// <summary>
        /// The sink identifier / name.
        /// </summary>
        #endregion
        public string Identifier { get; }

        #region Logger Log XML
        /// <summary>
        /// The sink logger.
        /// </summary>
        #endregion
        public Log Logger { get; }

        #region AllowedSeverities Severity XML
        /// <summary>
        /// The sink's allowed severity levels.
        /// </summary>
        #endregion
        public Severity AllowedSeverities { get; set; }

        #region StrictFiltering Boolean XML
        /// <summary>
        /// Determines whether a log needs to fully or partially match the allowed severities.
        /// </summary>
        #endregion
        public bool StrictFiltering { get; set; }

        #region IsEnabled Boolean XML
        /// <summary>
        /// Determines if the sink is enabled.
        /// </summary>
        #endregion
        public bool IsEnabled { get; set; }


        // Private / Hidden variables..
        #region Sinks ISink List XML
        /// <summary>
        /// The sink stack.
        /// </summary>
        #endregion
        private List<ISink> Sinks { get; init; }

        #region Logs Arguments Queue XML
        /// <summary>
        /// Queue of logs to dispatch to sinks asynchronously.
        /// </summary>
        #endregion
        private Queue<Arguments> Logs { get; init; }

        #region IsAlive Boolean XML
        /// <summary>
        /// Acts as an exit condition for the log check loop.
        /// </summary>
        #endregion
        private bool IsAlive { get; set; }

        #region AllowEnqueue Boolean XML
        /// <summary>
        /// Queue guard for <see cref="Emit(Arguments)"/>.
        /// </summary>
        #endregion
        private bool AllowEnqueue { get; set; }


        // Public / Accessible Events.
        #region OnLog Event XML
        /// <summary>
        /// Invoked in the <see cref="Log.Write(string, Severity, Dictionary{string, Object}, Object)"/> function.
        /// </summary>
        #endregion
        public event Action<Arguments> OnLog;

        #region OnSave Event XML
        /// <summary>
        /// Signals to listening sinks to "save" the log, invoked in the <see cref="Log.Save()"/> function.
        /// </summary>
        #endregion
        public event Action OnSave;

        #region OnClear Event XML
        /// <summary>
        /// Signals to listening sinks to "clear" the log, invoked in the <see cref="Log.Clear()"/> function.
        /// </summary>
        #endregion
        public event Action OnClear;


        #region Emit Function XML
        /// <inheritdoc/>
        #endregion
        public void Emit(Arguments Log)
        {
            if (AllowEnqueue) {
                Logs.Enqueue(Log);
            }
        }


        #region Find<Sink> Function XML
        /// <summary>
        /// Finds and returns matching sinks based on their type and identifier.
        /// </summary>
        /// <param name="Identifier">A name filter for sinks, used if there are multiple sinks of the same type.<br/>If <see langword="null"/> or equal to <see cref="String.Empty"/>, only filters sinks by type.</param>
        /// <typeparam name="Sink">Type filter for sinks, supports any type that implements <see cref="ISink"/> types.</typeparam>
        /// <returns>A list containing the matching sinks, or an empty list if nothing is found.</returns>
        #endregion
        public List<Sink> Find<Sink>(string Identifier = null) where Sink : ISink
        {
            Identifier ??= String.Empty;

            List<Sink> Return = new List<Sink>();
            foreach (ISink ProbedSink in Sinks)
            {
                if (ProbedSink is Sink && ProbedSink.Identifier.Contains(Identifier)) {
                    Return.Add(((Sink)(ProbedSink)));
                }
            }

            return Return;
        }

        #region Push Function XML
        /// <summary>
        /// Pushes a sink to the sink stack.
        /// </summary>
        /// <param name="Sink">The sink to push.</param>
        /// <exception cref="ArgumentException">Thrown if the logger reference in the sink is not the current logger.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Sink"/> is <see langword="null"/>.</exception>
        #endregion
        public void Push(ISink Sink)
        {
            if (Sink != null)
            {
                if (Sink.Logger == this.Logger)
                {
                    Sinks.Add(Sink);
                    Sink.Initialize();
                }

                else {
                    throw new ArgumentException($"Invalid logger instance `{Sink.Logger.Identifier}` in sink `{Sink.Identifier}`, should be `{Logger.Identifier}`.");
                }
            }

            else {
                throw new ArgumentNullException(nameof(Sink));
            }
        }

        #region Pop Function XML
        /// <summary>
        /// Pops a sink off the sink stack.
        /// </summary>
        /// <param name="Sink">The sink to pop.</param>
        /// <exception cref="ArgumentException">Thrown if the logger reference in the sink is not the current logger.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Sink"/> is <see langword="null"/>.</exception>
        #endregion
        public void Pop(ISink Sink)
        {
            if (Sink != null)
            {
                Sinks.Remove(Sink);
                Sink.Shutdown();
            }

            else {
                throw new ArgumentNullException(nameof(Sink));
            }
        }


        #region Initialize Function XML
        /// <inheritdoc/>
        #endregion
        public void Initialize()
        {
            Task.Run(this.BeginLogQueue);
        }

        #region Shutdown Function XML
        /// <inheritdoc/>
        #endregion
        public void Shutdown()
        {
            this.IsAlive = false;

            for (int I = 0; I < Sinks.Count; I++) {
                this.Pop(Sinks[I]);
            }
        }

        #region Save Function XML
        /// <inheritdoc/>
        #endregion
        public void Save()
        {
            Task.Run(this.SaveAsync);
        }

        #region Clear Function XML
        /// <inheritdoc/>
        #endregion
        public void Clear()
        {
            Task.Run(this.ClearAsync);
        }


        private async Task BeginLogQueue()
        {
            this.IsAlive = true;
            while (IsAlive)
            {
                if (Logs.Count > 0)
                {
                    await this.EmitAsync(Logs.Dequeue());
                }
            }
        }


        // TODO: wait method..

        private async Task EmitAsync(Arguments Log)
        {
            await Task.Yield();

            foreach (ISink Sink in Sinks)
            {
                if (Sink.IsEnabled && Log.Severity.Passes(Sink.AllowedSeverities, Sink.StrictFiltering))
                {
                    Sink.Emit(Log);
                }
            }

            OnLog?.Invoke(Log);
        }

        private async Task SaveAsync()
        {
            await Task.Yield();

            for (int I = 0; I < Sinks.Count; I++) {
                Sinks[I].Save();
            }
        }

        private async Task ClearAsync()
        {
            await Task.Yield();

            for (int I = 0; I < Sinks.Count; I++)
            {
                Sinks[I].Clear();
            }
        }



        #region AsynchronousSink Constructor XML
        /// <summary>
        /// The default <see cref="AsynchronousSink"/> constructor.
        /// </summary>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="AllowedSeverities">The sink's allowed severity levels.</param>
        #endregion
        public AsynchronousSink(string Identifier, Log Logger, Severity AllowedSeverities = Verbosity.All)
        {
            this.Identifier = Identifier;
            this.Logger = Logger;
            this.AllowedSeverities = AllowedSeverities;

            this.Sinks = new List<ISink>();
            this.Logs = new Queue<Arguments>();
            this.StrictFiltering = true;
            this.IsEnabled = true;
            this.AllowEnqueue = true;
        }
    }



    #region AsynchronousSinkUtilities Class XML
    /// <summary>
    /// Contains extension methods.
    /// </summary>
    #endregion
    public static class AsynchronousSinkUtilities
    {
        #region PushAsynchronous Function XML
        /// <summary>
        /// Pushes a new asynchronous sink on the logger sink stack.
        /// </summary>
        /// <param name="Logger">The logger to push the sink to.</param>
        /// <param name="Identifier">The sink identifier.</param>
        /// <param name="AllowedSeverities">The sink's allowed severity levels.</param>
        /// <returns>The current logger, to allow for method chaining.</returns>
        #endregion
        public static Log PushAsynchronous(this Log Logger, string Identifier, Severity AllowedSeverities = Verbosity.All)
        {
            AsynchronousSink Sink = new AsynchronousSink(Identifier, Logger, AllowedSeverities);
            Logger.Push(Sink);

            return Logger;
        }
    }
}