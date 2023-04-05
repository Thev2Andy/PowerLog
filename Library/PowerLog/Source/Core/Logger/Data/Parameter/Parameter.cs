using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region Parameter Struct XML
    /// <summary>
    /// A custom log parameter, embedded in the raw log.
    /// </summary>
    #endregion
    [Serializable] public struct Parameter
    {
        #region Identifier String XML
        /// <summary>
        /// The identifier of the log parameter.
        /// </summary>
        #endregion
        public string Identifier { get; private set; }

        #region Value Object XML
        /// <summary>
        /// The value of the log parameter.
        /// </summary>
        #endregion
        public Object Value { get; private set; }


        #region Get Method XML
        /// <summary>
        /// Gets the value of this log parameter.
        /// </summary>
        /// <returns>The log parameter value.</returns>
        #endregion
        public Object Get() {
            return Value;
        }

        #region Set Method XML
        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name="Value">The value to be assigned to this log parameter.</param>
        #endregion
        public void Set(Object Value) {
            this.Value = Value;
        }


        #region Parameter Constructor XML
        /// <summary>
        /// The default <see cref="Parameter"/> constructor.
        /// </summary>
        /// <param name="Identifier">The identifier / name of this parameter.</param>
        /// <param name="Value">The value of this parameter.</param>
        #endregion
        public Parameter(string Identifier, Object Value) {
            this.Identifier = Identifier;
            this.Value = Value;
        }
    }
}
