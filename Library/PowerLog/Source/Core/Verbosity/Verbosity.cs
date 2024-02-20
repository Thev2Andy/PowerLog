using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    public static class Verbosity
    {
        public const Severity All = ((Severity)(~0));
        public const Severity None = 0;


        public static Severity Except(this Severity Operand, Severity Exclusions) {
            return Operand & ~Exclusions;
        }

        public static Severity Plus(this Severity Operand, Severity Inclusions) {
            return Operand | Inclusions;
        }

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

        public static Severity Maximum(Severity Maximum) {
            return (~Verbosity.Minimum(Maximum) | Maximum);
        }

        
        public static bool Passes(this Severity Severity, Severity Verbosity) {
            return ((Verbosity & Severity) != 0);
        }
    }
}
