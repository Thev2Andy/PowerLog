using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region IComponent Interface XML
    /// <summary>
    /// A shared interface with common members between different component types.
    /// </summary>
    /// <remarks>
    /// Do <b>NOT</b> implement <see cref="IComponent"/> or any directly descending types in a single component. (Component types are mutually exclusive.)<br/>
    /// This class is internally used to represent any component in unified component management methods.
    /// </remarks>
    #endregion
    public interface IComponent
    {
        // Public / Accessible variables..
        #region Identifier String XML
        /// <summary>
        /// The component identifier / name.
        /// </summary>
        #endregion
        public abstract string Identifier { get; }

        #region Logger Log XML
        /// <summary>
        /// The component logger.
        /// </summary>
        #endregion
        public abstract Log Logger { get; }

        #region IsEnabled Boolean XML
        /// <summary>
        /// Determines if the component is enabled.
        /// </summary>
        #endregion
        public abstract bool IsEnabled { get; set; }
    }
}
