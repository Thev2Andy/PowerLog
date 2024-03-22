using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PowerLog
{
    #region Log Class XML
    /// <summary>
    /// The class used for writing logs.
    /// </summary>
    #endregion
    public sealed class Log : IDisposable
    {
        // Public / Accessible variables.
        #region Identifier String XML
        /// <summary>
        /// Contains the identifier / name of the current logger object.
        /// </summary>
        #endregion
        public string Identifier { get; init; }

        #region AllowedSeverities Severity XML
        /// <summary>
        /// The logger's allowed severity levels.
        /// </summary>
        #endregion
        public Severity AllowedSeverities { get; set; }

        #region StrictFiltering Boolean XML
        /// <summary>
        /// Determines whether a log needs to fully or partially match filtering tests and the allowed severities.
        /// </summary>
        #endregion
        public bool StrictFiltering { get; set; }

        #region FormattingTemplate Template XML
        /// <summary>
        /// Formatting template used by most sinks to compose the log.<br/>The <see cref="Template"/> structure provides some presets to use, such as <see cref="Template.Minimal"/>, <see cref="Template.Modern"/> or <see cref="Template.Detailed"/>.<br/>(Warning: Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.ComposedLog"/> property and / or the formatting template.)
        /// </summary>
        #endregion
        public Template FormattingTemplate { get; set; }

        #region IsDisposed Boolean XML
        /// <summary>
        /// Determines if the current logger has been disposed.
        /// </summary>
        #endregion
        public bool IsDisposed { get; private set; }

        #region Context Dictionary XML
        /// <summary>
        /// Contextual properties attached to the logger and automatically forwarded to each emitted log.
        /// </summary>
        #endregion
        public Dictionary<string, Object> Context { get; init; }


        // Private / Hidden variables.
        #region Sinks ISink List XML
        /// <summary>
        /// The sink stack.
        /// </summary>
        #endregion
        private List<ISink> Sinks { get; init; }

        #region Enrichers IEnricher List XML
        /// <summary>
        /// The enricher stack.
        /// </summary>
        #endregion
        private List<IEnricher> Enrichers { get; init; }

        #region Filters IFilter List XML
        /// <summary>
        /// The filter stack.
        /// </summary>
        #endregion
        private List<IFilter> Filters { get; init; }


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

        #region OnDispose Event XML
        /// <summary>
        /// Invoked when this logger is disposed, invoked in the <see cref="Log.Dispose()"/> function.
        /// </summary>
        #endregion
        public event Action OnDispose;



        #region Write Function XML
        /// <summary>
        /// Calls a dynamic <see cref="Severity"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Severity">The type / level / severity of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Write(string Content, Severity Severity, Dictionary<string, Object> Parameters = null, Object Sender = null)
        {
            if (!IsDisposed)
            {
                if (Severity.Passes(AllowedSeverities, StrictFiltering))
                {
                    Dictionary<string, Object> Enrichments = null;
                    if (Enrichers.Count > 0)
                    {
                        Enrichments = new Dictionary<string, Object>();

                        foreach (IEnricher Enricher in Enrichers)
                        {
                            if (Enricher.IsEnabled) {
                                Enricher.Enrich(in Enrichments);
                            }
                        }
                    }

                    Dictionary<string, Object> Context = null;
                    if (this.Context.Count > 0)
                    {
                        Context = new Dictionary<string, Object>();

                        foreach (KeyValuePair<string, Object> ContextualProperty in this.Context) {
                            Context.Add(ContextualProperty.Key, ContextualProperty.Value);
                        }
                    }


                    Arguments ProducedLog = new Arguments()
                    {
                        Content = Content,
                        Severity = Severity,
                        Time = DateTime.Now,
                        Template = FormattingTemplate,
                        Sender = Sender,
                        Parameters = Parameters,
                        Enrichments = Enrichments,
                        Context = Context,

                        Logger = this,
                    };

                    this.Raw(ProducedLog);
                }
            }

            else {
                throw new ObjectDisposedException(Identifier);
            }
        }

        #region Raw Function XML
        /// <summary>
        /// Calls a pre-constructed log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Log">The pre-constructed log to send.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Raw(Arguments Log)
        {
            if (!IsDisposed)
            {
                if (Log.Severity.Passes(AllowedSeverities, StrictFiltering))
                {
                    bool PassesFilters = true;
                    for (int I = 0; I < Filters.Count; I++)
                    {
                        if (Filters[I].IsEnabled)
                        {
                            bool FilterResult = Filters[I].Filter(Log);
                            PassesFilters = ((StrictFiltering || I == 0) ? (PassesFilters && FilterResult) : (PassesFilters || FilterResult));
                        }
                    }

                    if (PassesFilters)
                    {
                        foreach (ISink Sink in Sinks)
                        {
                            if (Sink.IsEnabled && Log.Severity.Passes(Sink.AllowedSeverities, Sink.StrictFiltering)) {
                                Sink.Emit(Log);
                            }
                        }

                        OnLog?.Invoke(Log);
                    }
                }
            }

            else {
                throw new ObjectDisposedException(Identifier);
            }
        }

        #region Log Overloads
        #region Verbose Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Verbose"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Verbose(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Verbose, Parameters, Sender);
        }

        #region Trace Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Trace"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Trace(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Trace, Parameters, Sender);
        }

        #region Debug Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Debug"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Debug(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Debug, Parameters, Sender);
        }

        #region Network Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Network"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Network(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Network, Parameters, Sender);
        }

        #region Information Function XML
        /// <summary>
        /// Calls an <see cref="Severity.Information"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Information(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Information, Parameters, Sender);
        }

        #region Notice Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Notice"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Notice(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Notice, Parameters, Sender);
        }

        #region Caution Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Caution"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Caution(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Caution, Parameters, Sender);
        }

        #region Warning Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Warning"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Warning(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Warning, Parameters, Sender);
        }

        #region Alert Function XML
        /// <summary>
        /// Calls an <see cref="Severity.Alert"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Alert(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Alert, Parameters, Sender);
        }

        #region Error Function XML
        /// <summary>
        /// Calls an <see cref="Severity.Error"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Error(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Error, Parameters, Sender);
        }

        #region Critical Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Critical"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Critical(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Critical, Parameters, Sender);
        }

        #region Emergency Function XML
        /// <summary>
        /// Calls an <see cref="Severity.Emergency"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Emergency(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Emergency, Parameters, Sender);
        }

        #region Fatal Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Fatal"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Fatal(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Fatal, Parameters, Sender);
        }

        #region Generic Function XML
        /// <summary>
        /// Calls a <see cref="Severity.Generic"/> log, sent over to the currently active sinks and the <see cref="Log.OnLog"/> event.
        /// </summary>
        /// <param name="Content">The actual content of the log.</param>
        /// <param name="Parameters">Additional logging data.</param>
        /// <param name="Sender">The log sender.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to write logs with a disposed logger.</exception>
        #endregion
        public void Generic(string Content, Dictionary<string, Object> Parameters = null, Object Sender = null) {
            this.Write(Content, Severity.Generic, Parameters, Sender);
        }
        #endregion


        #region Save Function XML
        /// <summary>
        /// Sends a signal to listening sinks to "save" the log.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to request a save in a disposed logger.</exception>
        #endregion
        public void Save()
        {
            if (!IsDisposed)
            {
                foreach (ISink Sink in Sinks)
                {
                    if (Sink.IsEnabled) {
                        Sink.Save();
                    }
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
        /// <exception cref="ObjectDisposedException">Thrown when attempting to request a clear in a disposed logger.</exception>
        #endregion
        public void Clear()
        {
            if (!IsDisposed)
            {
                foreach (ISink Sink in Sinks)
                {
                    if (Sink.IsEnabled) {
                        Sink.Clear();
                    }
                }

                OnClear?.Invoke();
            }
            
            else {
                throw new ObjectDisposedException(Identifier);
            }
        }


        #region Find<Component> Function XML
        /// <summary>
        /// Finds and returns matching components based on their type and identifier.
        /// </summary>
        /// <param name="Identifier">A name filter for components, used if there are multiple components of the same type.<br/>If <see langword="null"/> or equal to <see cref="String.Empty"/>, only filters components by type.</param>
        /// <typeparam name="Component">Type filter for components, supports any type that implements <see cref="IComponent"/> based types.</typeparam>
        /// <returns>A list containing the matching components, or an empty list if nothing is found.</returns>
        /// <exception cref="ArgumentException">Thrown if the <typeparamref name="Component"/> doesn't exclusively match a single component type.</exception>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to find components pushed to a disposed logger.</exception>
        #endregion
        public List<Component> Find<Component>(string Identifier = null) where Component : IComponent
        {
            if (!IsDisposed)
            {
                Identifier ??= String.Empty;

                bool IsSink = typeof(Component).IsAssignableTo(typeof(ISink));
                bool IsEnricher = typeof(Component).IsAssignableTo(typeof(IEnricher));
                bool IsFilter = typeof(Component).IsAssignableTo(typeof(IFilter));

                if (((IsSink ^ IsEnricher ^ IsFilter) && !(IsSink && IsEnricher && IsFilter)))
                {
                    List<Component> Return = new List<Component>();
                    dynamic SearchCollection = null;

                    if (IsSink) {
                        SearchCollection = Sinks;
                    }

                    if (IsEnricher) {
                        SearchCollection = Enrichers;
                    }

                    if (IsFilter) {
                        SearchCollection = Filters;
                    }


                    foreach (IComponent ProbedComponent in SearchCollection)
                    {
                        if (ProbedComponent is Component && ProbedComponent.Identifier.Contains(Identifier)) {
                            Return.Add(((Component)(ProbedComponent)));
                        }
                    }

                    return Return;
                }

                else {
                    throw new ArgumentException($"Invalid type parameter `{typeof(Component)}`! Type parameter `{typeof(Component)}` must exclusively be a single component type and not a raw `{nameof(IComponent)}` type.");
                }
            }

            else {
                throw new ObjectDisposedException(this.Identifier);
            }
        }

        #region Push Function XML
        /// <summary>
        /// Pushes a component to its appropriate stack.
        /// </summary>
        /// <param name="Component">The component instance to push.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to push a component on a disposed logger.</exception>
        /// <exception cref="ArgumentException">Thrown if the logger reference in the component is not the current logger, or if the component implements multiple <see cref="IComponent"/> types.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Component"/> is <see langword="null"/>.</exception>
        #endregion
        public void Push(IComponent Component)
        {
            if (!IsDisposed)
            {
                if (Component != null)
                {
                    bool IsSink = Component is ISink;
                    bool IsEnricher = Component is IEnricher;
                    bool IsFilter = Component is IFilter;

                    if (((IsSink ^ IsEnricher ^ IsFilter) && !(IsSink && IsEnricher && IsFilter)))
                    {
                        if (Component.Logger == this)
                        {
                            if (IsSink)
                            {
                                ISink Sink = ((ISink)(Component));

                                Sinks.Add(Sink);
                                Sink.Initialize();
                            }

                            if (IsEnricher)
                            {
                                IEnricher Enricher = ((IEnricher)(Component));
                                Enrichers.Add(Enricher);
                            }

                            if (IsFilter)
                            {
                                IFilter Filter = ((IFilter)(Component));
                                Filters.Add(Filter);
                            }
                        }

                        else {
                            throw new ArgumentException($"Invalid logger instance `{Component.Logger.Identifier}` in component `{Component.Identifier}`, should be `{this.Identifier}`.");
                        }
                    }

                    else {
                        throw new ArgumentException($"Invalid component type `{Component.GetType()}`! Component type `{Component.GetType()}` must exclusively be a single component type and not a raw `{nameof(IComponent)}` type.");
                    }
                }

                else {
                    throw new ArgumentNullException(nameof(Component));
                }
            }

            else {
                throw new ObjectDisposedException(Identifier);
            }
        }

        #region Pop Function XML
        /// <summary>
        /// Pops a component off its stack.
        /// </summary>
        /// <param name="Component">The component instance to remove.</param>
        /// <exception cref="ObjectDisposedException">Thrown when attempting to remove a component from a disposed logger.</exception>
        /// <exception cref="ArgumentException">Thrown if the logger reference in the component is not the current logger, or if the component implements multiple <see cref="IComponent"/> types.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="Component"/> is <see langword="null"/>.</exception>
        #endregion
        public void Pop(IComponent Component)
        {
            if (!IsDisposed)
            {
                if (Component != null)
                {
                    bool IsSink = Component is ISink;
                    bool IsEnricher = Component is IEnricher;
                    bool IsFilter = Component is IFilter;

                    if (((IsSink ^ IsEnricher ^ IsFilter) && !(IsSink && IsEnricher && IsFilter)))
                    {
                        if (Component.Logger == this)
                        {
                            if (IsSink)
                            {
                                ISink Sink = ((ISink)(Component));

                                Sinks.Remove(Sink);
                                Sink.Shutdown();
                            }

                            if (IsEnricher)
                            {
                                IEnricher Enricher = ((IEnricher)(Component));
                                Enrichers.Remove(Enricher);
                            }

                            if (IsFilter)
                            {
                                IFilter Enricher = ((IFilter)(Component));
                                Filters.Remove(Enricher);
                            }
                        }
                    }

                    else {
                        throw new ArgumentException($"Invalid component type `{Component.GetType()}`! Component type `{Component.GetType()}` must exclusively be a single component type and not a raw `{nameof(IComponent)}` type.");
                    }
                }

                else {
                    throw new ArgumentNullException(nameof(Component));
                }
            }

            else {
                throw new ObjectDisposedException(Identifier);
            }
        }



        #region Dispose Function XML
        /// <summary>
        /// Marks the logger as disposed and removes all of the components attached to the different stacks.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown on double-disposal of the current logger.</exception>
        #endregion
        public void Dispose()
        {
            if (!IsDisposed)
            {
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
        /// <param name="FormattingTemplate">Formatting template used by most sinks to compose the log.<br/>The <see cref="Template"/> structure provides some presets to use, such as <see cref="Template.Minimal"/>, <see cref="Template.Modern"/> or <see cref="Template.Detailed"/>.<br/>(Warning: Some sinks may use a custom formatting solution and ignore the <see cref="Arguments.ComposedLog"/> property.)</param>
        /// <param name="AllowedSeverities">The logger's allowed severity levels.</param>
        #endregion
        public Log(string Identifier, Severity AllowedSeverities = Verbosity.All)
        {
            this.Identifier = Identifier;
            this.FormattingTemplate = Template.Modern;
            this.AllowedSeverities = AllowedSeverities;

            this.Context = new Dictionary<string, Object>();
            this.Sinks = new List<ISink>();
            this.Enrichers = new List<IEnricher>();
            this.Filters = new List<IFilter>();

            this.StrictFiltering = true;
        }
    }
}
