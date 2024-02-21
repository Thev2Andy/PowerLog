﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PowerLog
{
    #region Log Class XML
    /// <summary>
    /// The class used for writing logs.
    /// </summary>
    #endregion
    public class Log
    {
        // Public / Accessible variables.
        #region Identifier String XML
        /// <summary>
        /// Contains the identifier / name of the current logger object.
        /// </summary>
        #endregion
        public string Identifier { get; private set; }

        #region AllowedSeverities Severity XML
        /// <summary>
        /// The logger's allowed severity levels.
        /// </summary>
        #endregion
        public Severity AllowedSeverities { get; set; }

        #region StrictFiltering Boolean XML
        /// <summary>
        /// Verbosity test behaviour, determines if a given log needs to fully or partially match the allowed severities.
        /// </summary>
        #endregion
        public bool StrictFiltering { get; set; }

        #region IsDisposed Boolean XML
        /// <summary>
        /// Determines if the current logger has been disposed.
        /// </summary>
        #endregion
        public bool IsDisposed { get; private set; }


        // Private / Hidden variables.
        #region Sinks ISink List XML
        /// <summary>
        /// The sink stack.
        /// </summary>
        #endregion
        private List<ISink> Sinks { get; set; }


        // Public / Accessible Events.
        #region OnLog Event XML
        /// <summary>
        /// Invoked in the <see cref="Log.Write(string, Severity, Template?, Dictionary{string, Object}, Object)"/> function.
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

        #region OnDispose Event XML
        /// <summary>
        /// Invoked when this logger is disposed, invoked in the <see cref="Log.Dispose()"/> function.
        /// </summary>
        #endregion
        public event Action OnDispose;



        #region Write Function XML
        /// <summary>
        /// Calls a dynamic <see cref="Severity"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Severity">The type / level / severity of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Write(string Content, Severity Severity, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null)
        {
            if (!IsDisposed)
            {
                if (Severity.Passes(AllowedSeverities, StrictFiltering))
                {
                    Arguments ProducedLog = new Arguments();
                    ProducedLog.Content = Content;
                    ProducedLog.Severity = Severity;
                    ProducedLog.Time = DateTime.Now;
                    ProducedLog.Template = ((Template)((Template == null) ? ((!String.IsNullOrEmpty(Content)) ? PowerLog.Template.Default : PowerLog.Template.Empty) : Template));
                    ProducedLog.Sender = Sender;
                    ProducedLog.Stacktrace = new StackTrace();
                    ProducedLog.Parameters = ((Parameters != null) ? Parameters : new Dictionary<string, Object>());

                    ProducedLog.Logger = this;


                    foreach (ISink Sink in Sinks) {
                        Sink.Emit(ProducedLog);
                    }

                    OnLog?.Invoke(ProducedLog);
                }
            }

            else {
                throw new ObjectDisposedException(Identifier);
            }
        }

        #region Log Overloads
        #region Verbose Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Verbose"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Verbose(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Verbose, Template, Parameters, Sender);
        }

        #region Trace Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Trace"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Trace(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Trace, Template, Parameters, Sender);
        }

        #region Debug Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Debug"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Debug(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Debug, Template, Parameters, Sender);
        }

        #region Network Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Network"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Network(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Network, Template, Parameters, Sender);
        }

        #region Information Function XML
        /// <summary>
        /// Calls an <see cref="Severity.Information"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Information(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Information, Template, Parameters, Sender);
        }

        #region Notice Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Notice"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Notice(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Notice, Template, Parameters, Sender);
        }

        #region Caution Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Caution"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Caution(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Caution, Template, Parameters, Sender);
        }

        #region Warning Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Warning"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Warning(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Warning, Template, Parameters, Sender);
        }

        #region Alert Function XML
        /// <summary>
        /// Calls an <see cref="Severity.Alert"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Alert(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Alert, Template, Parameters, Sender);
        }

        #region Error Function XML
        /// <summary>
        /// Calls an <see cref="Severity.Error"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Error(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Error, Template, Parameters, Sender);
        }

        #region Critical Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Critical"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Critical(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Critical, Template, Parameters, Sender);
        }

        #region Emergency Function XML
        /// <summary>
        /// Calls an <see cref="Severity.Emergency"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Emergency(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Emergency, Template, Parameters, Sender);
        }

        #region Fatal Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Fatal"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Fatal(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Fatal, Template, Parameters, Sender);
        }

        #region Generic Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Generic"/> log, sent over to the current active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Template">Format template used by most sinks to compose the log. (Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.FormattedLog"/> property.)</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        #endregion
        public void Generic(string Content, Template? Template = null, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Generic, Template, Parameters, Sender);
        }
        #endregion


        #region Save Function XML
        /// <summary>
        /// Sends a signal to listening sinks to "save" the log.
        /// </summary>
        #endregion
        public void Save()
        {
            if (!IsDisposed)
            {
                foreach (ISink Sink in Sinks) {
                    Sink.Save();
                }

                OnSave?.Invoke();
            }

            else {
                throw new ObjectDisposedException(Identifier);
            }
        }

        #region Clear Function XML
        /// <summary>
        /// Sends a signal to listening sinks to "clear" the log.
        /// </summary>
        #endregion
        public void Clear()
        {
            if (!IsDisposed)
            {
                foreach (ISink Sink in Sinks) {
                    Sink.Clear();
                }

                OnClear?.Invoke();
            }
            
            else {
                throw new ObjectDisposedException(Identifier);
            }
        }


        #region Find Function XML
        /// <summary>
        /// Finds and returns a sink by its identifier.
        /// </summary>
        /// <param name="Identifier">The sink identifier</param>
        /// <returns>The found sink, or nothing if nothing is found.</returns>
        #endregion
        public ISink Find(string Identifier)
        {
            foreach (ISink ProbedSink in Sinks)
            {
                if (ProbedSink.Identifier == Identifier) {
                    return ProbedSink;
                }
            }

            return null;
        }

        #region Push Function XML
        /// <summary>
        /// Adds a sink to the sink stack.
        /// </summary>
        /// <param name="Sink">The sink instance to add.</param>
        #endregion
        public void Push(ISink Sink)
        {
            if (Sink.Logger == this)
            {
                Sinks.Add(Sink);
                Sink.Initialize();
            }

            else {
                throw new ArgumentException($"Invalid logger instance `{Sink.Logger.Identifier}` in sink `{Sink.Identifier}`, should be `{this.Identifier}`.");
            }
        }

        #region Pop Function XML
        /// <summary>
        /// Removes a sink from the sink stack.
        /// </summary>
        /// <param name="Sink">The sink to remove.</param>
        #endregion
        public void Pop(ISink Sink) {
            Sink.Shutdown();
            Sinks.Remove(Sink);
        }



        #region Dispose Function XML
        /// <summary>
        /// Marks the logger as disposed and pops off all of the sinks off the sink stack.
        /// </summary>
        #endregion
        public void Dispose()
        {
            if (!IsDisposed) {
                OnDispose?.Invoke();
                this.Save();

                for (int I = 0; I < Sinks.Count; I++) {
                    this.Pop(Sinks[I]);
                }

                IsDisposed = true;
            }

            else {
                throw new ObjectDisposedException(Identifier);
            }

        }

        #region Log Constructor XML
        /// <summary>
        /// The default <see cref="Log"/> constructor.
        /// </summary>
        /// <param name="Identifier">The identifier / name of this logger.</param>
        /// <param name="AllowedSeverities">The logger's allowed severity levels.</param>
        #endregion
        public Log(string Identifier, Severity AllowedSeverities = Verbosity.All)
        {
            this.Identifier = Identifier;
            this.Sinks = new List<ISink>();
            this.AllowedSeverities = AllowedSeverities;
            this.StrictFiltering = true;
        }
    }
}
