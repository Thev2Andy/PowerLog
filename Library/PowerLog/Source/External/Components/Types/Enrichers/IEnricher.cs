using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region IEnricher Interface XML
    /// <summary>
    /// An interface with the required functions and variables to create a log enricher.
    /// </summary>
    #endregion
    public interface IEnricher : IComponent
    {
        // Public / Accessible variables..
        #region Identifier String XML
        /// <summary>
        /// The enricher identifier / name.
        /// </summary>
        #endregion
        public abstract new string Identifier { get; }

        #region Logger Log XML
        /// <summary>
        /// The sink logger.
        /// </summary>
        #endregion
        public abstract new Log Logger { get; }

        #region IsEnabled Boolean XML
        /// <summary>
        /// Determines if the enricher is enabled.
        /// </summary>
        #endregion
        public abstract new bool IsEnabled { get; set; }



        // Abstract methods..
        #region Enrich Function XML
        /// <summary>
        /// The main function of the enricher, called on every log.
        /// </summary>
        /// <param name="Enrichments">The enrichments dictionary in which to add / remove / modify properties.</param>
        #endregion
        public void Enrich(in Dictionary<string, Object> Enrichments);
    }
}
