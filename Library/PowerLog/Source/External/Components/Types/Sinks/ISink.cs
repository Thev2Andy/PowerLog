using System;

namespace PowerLog
{
    #region ISink Interface XML
    /// <summary>
    /// An interface with the required functions and variables to create a destination for all log events.
    /// </summary>
    #endregion
    public interface ISink : IComponent
    {
        // Public / Accessible variables..
        #region Identifier String XML
        /// <summary>
        /// The sink identifier / name.
        /// </summary>
        #endregion
        public abstract new string Identifier { get; }

        #region Logger Log XML
        /// <summary>
        /// The sink logger.
        /// </summary>
        #endregion
        public abstract new Log Logger { get; }

        #region AllowedSeverities Severity XML
        /// <summary>
        /// The sink's allowed severity levels.
        /// </summary>
        #endregion
        public abstract Severity AllowedSeverities { get; set; }

        #region StrictFiltering Boolean XML
        /// <summary>
        /// Determines whether a log needs to fully or partially match filtering tests.
        /// </summary>
        #endregion
        public abstract bool StrictFiltering { get; set; }

        #region IsEnabled Boolean XML
        /// <summary>
        /// Determines if the sink is enabled.
        /// </summary>
        #endregion
        public abstract new bool IsEnabled { get; set; }



        // Abstract methods..
        #region Emit Function XML
        /// <summary>
        /// The main function of the sink, called on every log.
        /// </summary>
        /// <param name="Log">The emitted log.</param>
        #endregion
        public void Emit(Arguments Log);


        #region Save Function XML
        /// <summary>
        /// Called when the logger requests a save.
        /// </summary>
        #endregion
        public void Save();

        #region Clear Function XML
        /// <summary>
        /// Called when the logger requests a clear.
        /// </summary>
        #endregion
        public void Clear();


        #region Initialize Function XML
        /// <summary>
        /// Initializes the sink.
        /// </summary>
        #endregion
        public void Initialize();

        #region Shutdown Function XML
        /// <summary>
        /// Requests a sink shutdown.
        /// </summary>
        #endregion
        public void Shutdown();
    }
}