using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region Template Struct XML
    /// <summary>
    /// Holds data related to the formatting of the logs.
    /// </summary>
    #endregion
    [Serializable] public struct Template
    {
        #region DateFormat String XML
        /// <summary>
        /// The date formatting template.
        /// </summary>
        #endregion
        public string DateFormat { get; private set; }

        #region LogFormat String XML
        /// <summary>
        /// The log formatting template.
        /// Examples:<br/> <br/>
        /// Use `<c>|T|</c>` for the timestamp. (Refers to 'Time / Timestamp'.)<br/>
        /// Use `<c>|I|</c>` for the logger identifier. (Refers to 'Identifier'.)<br/>
        /// Use `<c>|S|</c>` for the severity. (Refers to 'Severity'.)<br/>
        /// Use `<c>|C|</c>` for the content. (Refers to 'Content'.)<br/>
        /// Use `<c>|O|</c>` for the sender.  (Refers to 'Object'.)<br/>
        /// </summary>
        #endregion
        public string LogFormat { get; private set; }


        // Predefined profiles..
        #region Default Template XML
        /// <summary>
        /// The default <see cref="Template"/> profile.
        /// </summary>
        #endregion
        public static Template Default {
            get {
                return new Template("|[T]| ||I |S: ||C|| (O)|", "HH:mm:ss");
            }
        }

        #region Analysis Template XML
        /// <summary>
        /// A <see cref="Template"/> profile, specifically made for analysis purposes.
        /// </summary>
        #endregion
        public static Template Analysis {
            get {
                return new Template("Received log from logger `|I|`| of severity `S`| at |T|, sent by `|O|`: `|C|`", "HH-mm-ss tt, dd MMMM yyyy");
            }
        }

        #region Empty Template XML
        /// <summary>
        /// Empty <see cref="Template"/> profile.
        /// </summary>
        #endregion
        public static Template Empty {
            get {
                return new Template(String.Empty);
            }
        }



        #region Template Constructor XML
        /// <summary>
        /// The default <see cref="Template"/> constructor.
        /// </summary>
        /// <param name="LogFormat">The log formatting template.</param>
        /// <param name="DateFormat">The date formatting template.</param>
        #endregion
        public Template(string LogFormat, string DateFormat = "HH:mm:ss") {
            this.DateFormat = DateFormat;
            this.LogFormat = LogFormat;
        }
    }
}
