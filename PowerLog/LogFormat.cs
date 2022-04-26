using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLog
{
    // Missing XML
    public class LogFormat
    {
        // Missing XML.
        public static string Format(LogArgs Log, string LogTemplate, string DateTemplate)
        {
            if (!String.IsNullOrEmpty(LogTemplate) && !String.IsNullOrEmpty(DateTemplate))
            {
                Regex TimeRegex = new Regex(@"\|([^|]*)T([^|]*)\|", RegexOptions.Multiline);
                string Result = TimeRegex.Replace(LogTemplate, ("${1}" + $"{Log.LogTime.ToString(DateTemplate)}" + "${2}"));
                

                Regex LogRegex = new Regex(@"\|([^|]*)N([^|]*)\|", RegexOptions.Multiline);
                Result = LogRegex.Replace(Result, ((Log.Logger != null && !String.IsNullOrEmpty(Log.Logger.Identifier)) ? ("${1}" + $"{Log.Logger.Identifier}" + "${2}") : String.Empty));

                Regex LevelRegex = new Regex(@"\|([^|]*)L([^|]*)\|", RegexOptions.Multiline);
                Result = LevelRegex.Replace(Result, ((Log.LogLevel != LogType.NA) ? ("${1}" + $"{Log.LogLevel.ToString()}" + "${2}") : String.Empty));

                Regex ContentRegex = new Regex(@"\|([^|]*)C([^|]*)\|", RegexOptions.Multiline);
                Result = ContentRegex.Replace(Result, ("${1}" + $"{Log.LogContent}" + "${2}"));
                
                Regex SenderRegex = new Regex(@"\|([^|]*)S([^|]*)\|", RegexOptions.Multiline);
                Result = SenderRegex.Replace(Result, ((Log.LogSender != null) ? ("${1}" + $"{Log.LogSender}" + "${2}") : String.Empty));

                return Result;
            }
            
            else {
                throw new ArgumentException("One / Both of the log formats are empty.");
            }
        }
    }
}
