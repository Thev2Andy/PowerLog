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
    [Serializable] public record struct Template
    {
        #region Format String XML
        /// <summary>
        /// Provides the log formatting template for the composer.
        /// <br/><br/>
        /// <code>
        /// [ Not Conditional ]  |T| -> Timestamp.  (Refers to 'Time / Timestamp'.)<br/>
        /// [   Conditional   ]  |I| -> Logger.     (Refers to 'Identifier'.)<br/>
        /// [   Conditional   ]  |S| -> Severity.   (Refers to 'Severity'.)<br/>
        /// [ Not Conditional ]  |C| -> Content.    (Refers to 'Content'.)<br/>
        /// [   Conditional   ]  |O| -> Sender.     (Refers to 'Object'.)<br/>
        /// </code>
        /// </summary>
        #endregion
        public string Format { get; init; }

        #region Date String XML
        /// <summary>
        /// Provides the date format for the timestamp.
        /// </summary>
        #endregion
        public string Date { get; init; }

        #region Flags Template.Options XML
        /// <summary>
        /// Provides various options for the formatter.
        /// </summary>
        #endregion
        public Options Flags { get; init; }


        // Predefined profiles..
        #region Detailed Template XML
        /// <summary>
        /// A detailed <see cref="Template"/> profile.
        /// </summary>
        #endregion
        public static Template Detailed {
            get {
                return new Template("|[T] |||I |S: ||C|| (O)|", "HH:mm:ss", Options.Detailed);
            }
        }

        #region Modern Template XML
        /// <summary>
        /// A modern <see cref="Template"/> profile.
        /// </summary>
        /// <remarks>If you're using a single <see cref="Log"/> object, you can probably use the <see cref="Minimal"/> template.</remarks>
        #endregion
        public static Template Modern {
            get {
                return new Template("|T ||[|I |S] ||C|| (O)|", "HH:mm:ss", Options.Compact);
            }
        }

        #region Minimal Template XML
        /// <summary>
        /// A minimal <see cref="Template"/> profile.
        /// </summary>
        /// <remarks>If you're using multiple <see cref="Log"/> objects, you should probably use the <see cref="Modern"/> template.</remarks>
        #endregion
        public static Template Minimal {
            get {
                return new Template("|T ||[S] ||C|| (O)|", "HH:mm:ss", Options.Compact);
            }
        }

        #region Analysis Template XML
        /// <summary>
        /// A <see cref="Template"/> profile specifically made for analysis purposes, such as logging a log.
        /// </summary>
        #endregion
        public static Template Analysis {
            get {
                return new Template("Received log from logger `|I|`| of severity `S`| at |T||, sent by `O`|: `|C|`", "HH:mm:ss, dd MMMM yyyy", Options.Compact);
            }
        }



        #region Template Constructor XML
        /// <summary>
        /// The default <see cref="Template"/> constructor.
        /// </summary>
        /// <param name="Format">Provides the log formatting template for the composer.</param>
        /// <param name="Date">Provides the date format for the timestamp.</param>
        /// <param name="Flags">Provides various options for the formatter.</param>
        #endregion
        public Template(string Format, string Date = "HH:mm:ss", Template.Options Flags = Options.Detailed)
        {
            this.Format = Format;
            this.Date = Date;
            this.Flags = Flags;
        }



        #region Options Enum XML
        /// <summary>
        /// Formatting options enumeration.
        /// </summary>
        #endregion
        [Flags] public enum Options
        {
            #region ConditionalObject Enum Entry XML
            /// <summary>
            /// Discards a <see langword="null"/> sender object's wildcard.
            /// </summary>
            #endregion
            ConditionalObject = 1,

            #region ConditionalSeverity Enum Entry XML
            /// <summary>
            /// Discards the severity wildcard if the log severity is / contains <see cref="Severity.Generic"/>.
            /// </summary>
            #endregion
            ConditionalSeverity = 2,

            #region ConditionalLogger Enum Entry XML
            /// <summary>
            /// Discards a <see langword="null"/> logger's wildcard.
            /// </summary>
            #endregion
            ConditionalLogger = 4,

            #region Parse Enum Entry XML
            /// <summary>
            /// Enables parameter parsing.
            /// </summary>
            #endregion
            Parse = 8,

            #region Compact Enum Entry XML
            /// <summary>
            /// A preset for compact logging templates.
            /// </summary>
            #endregion
            Compact = (ConditionalObject | ConditionalSeverity | ConditionalLogger | Parse),

            #region Detailed Enum Entry XML
            /// <summary>
            /// A preset for detailed logging templates.
            /// </summary>
            #endregion
            Detailed = (ConditionalSeverity | ConditionalLogger | Parse),

            #region Analysis Enum Entry XML
            /// <summary>
            /// A preset for disabling discarding, for semi-raw composed logging data.
            /// </summary>
            #endregion
            Analysis = Parse,

            #region Raw Enum Entry XML
            /// <summary>
            /// A preset for disabling discarding and parsing, for raw composed logging data.
            /// </summary>
            #endregion
            Raw = 0
        }
    }
}
