using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    #region Verbosity Class XML
    /// <summary>
    /// The class used for generating and manipulating verbosity masks.
    /// </summary>
    #endregion
    public static class Verbosity
    {
        #region All Constant Severity XML
        /// <summary>
        /// Convenience constant for enabling every logging level.
        /// </summary>
        #endregion
        public const Severity All = ((Severity)(~(-1 << 14)));

        #region None Constant Severity XML
        /// <summary>
        /// Convenience constant for disabling every logging level.
        /// </summary>
        #endregion
        public const Severity None = 0;


        #region Except Function XML
        /// <summary>
        /// Excludes certain flags from a <see cref="Severity"/> value.
        /// </summary>
        /// <param name="Operand">The value to exclude from.</param>
        /// <param name="Exclusions">The values to exclude.</param>
        /// <returns>The new value without the excluded values.</returns>
        #endregion
        public static Severity Except(this Severity Operand, Severity Exclusions) {
            return Operand & ~Exclusions;
        }

        #region Plus Function XML
        /// <summary>
        /// Includes certain flags from a <see cref="Severity"/> value.
        /// </summary>
        /// <param name="Operand">The value to include to.</param>
        /// <param name="Inclusions">The values to include.</param>
        /// <returns>The new value with the included values.</returns>
        #endregion
        public static Severity Plus(this Severity Operand, Severity Inclusions) {
            return Operand | Inclusions;
        }

        #region Minimum Function XML
        /// <summary>
        /// Generated a verbosity mask with every level above or equal to the minimum.
        /// </summary>
        /// <param name="Minimum">The reference minimum value.</param>
        /// <returns>The generated mask.</returns>
        #endregion
        public static Severity Minimum(Severity Minimum)
        {
            Severity Result = None;

            foreach (Severity Severity in Enum.GetValues<Severity>())
            {
                if (Severity >= Minimum && ((int)(Severity)) >= ((int)(Minimum))) {
                    Result |= Severity;
                }
            }

            return Result;
        }

        #region Maximum Function XML
        /// <summary>
        /// Generated a verbosity mask with every level less or equal to the maximum.
        /// </summary>
        /// <param name="Maximum">The reference maximum value.</param>
        /// <returns>The generated mask.</returns>
        #endregion
        public static Severity Maximum(Severity Maximum) {
            return (~Verbosity.Minimum(Maximum) | Maximum);
        }


        #region Passes Function XML
        /// <summary>
        /// Tests a severity value against a verbosity mask and returns the result.
        /// </summary>
        /// <param name="Severity">The value to test.</param>
        /// <param name="Verbosity">The mask to test against.</param>
        /// <param name="StrictFiltering">Verbosity test behaviour, determines if the severity needs to fully or partially match the verbosity mask.</param>
        /// <returns>A boolean indicating <see langword="true"/> if the severity value passed the test.</returns>
        #endregion
        public static bool Passes(this Severity Severity, Severity Verbosity, bool StrictFiltering = true) {
            return ((StrictFiltering) ? ((Verbosity & Severity) == Severity) : ((Verbosity & Severity) != 0));
        }



        #region Verbosity Static Constructor XML
        /// <summary>
        /// Ensures that <see cref="All"/> is valid and includes all of the severities, when adding / removing a severity. <br/>
        /// In the case that <see cref="All"/> is invalid, update the bitshift amount with the new amount of severities.
        /// </summary>
        /// <exception cref="ApplicationException"><see cref="All"/> is invalid.</exception>
        #endregion
        static Verbosity()
        {
            Severity ValidationAll = (Severity)(~(-1 << Enum.GetValues<Severity>().Length));
            if (All != ValidationAll) {
                throw new ApplicationException($"The verbosity preset `{nameof(Verbosity)}.{nameof(All)}` is invalid!");
                // please don't ever fire thank you <3
            }
        }
    }
}
