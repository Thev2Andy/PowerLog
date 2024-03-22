using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region IFilter Interface XML
    /// <summary>
    /// An interface with the required functions and variables to create a log filter.
    /// </summary>
    #endregion
    public interface IFilter : IComponent
    {
        // Public / Accessible variables..
        #region Identifier String XML
        /// <summary>
        /// The filter identifier / name.
        /// </summary>
        #endregion
        public abstract new string Identifier { get; }

        #region Logger Log XML
        /// <summary>
        /// The filter logger.
        /// </summary>
        #endregion
        public abstract new Log Logger { get; }

        #region IsEnabled Boolean XML
        /// <summary>
        /// Determines if the filter is enabled.
        /// </summary>
        #endregion
        public abstract new bool IsEnabled { get; set; }


        // Abstract methods..
        #region Filter Function XML
        /// <summary>
        /// The main function of the filter, called on every log.
        /// </summary>
        /// <param name="Log">The log to filter.</param>
        /// <returns>
        /// A boolean representing <see langword="true"/> when allowing the log to pass through.
        /// </returns>
        #endregion
        public bool Filter(Arguments Log);
    }
}
