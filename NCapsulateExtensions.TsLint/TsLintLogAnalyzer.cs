using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace NCapsulateExtensions.TsLint
{
    public static class TsLintLogAnalyzer
    {
        private const string tsLintPattern =
            @"^\s*" +
            @"\((?<Start>[^\)]+)\)\s+" +
            @"(?<FileName>[\w\\\.]+\.ts)\[(?<LineNumber>\d+),\s+(?<ColumnNumber>\d+)\]:\s+" +
            @"(?<End>.+)$";

        private static readonly Regex tsLintRegex = new Regex(tsLintPattern, RegexOptions.Compiled);

        public static bool LogMessage(IBuildEngine buildEngine, BuildMessageEventArgs args)
        {
            bool success = true;
            if (args.Importance == MessageImportance.High)
            {
                int index = 0;
                using (TextReader reader = new StringReader(args.Message))
                {
                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                    {
                        Match match = tsLintRegex.Match(line);
                        if (match.Success)
                        {
                            int lineNumber = int.Parse(match.Groups["LineNumber"].Value);
                            int columnNumber = int.Parse(match.Groups["ColumnNumber"].Value);
                            BuildErrorEventArgs errorArgs = new BuildErrorEventArgs(
                                string.Empty,
                                string.Empty,
                                match.Groups["FileName"].Value,
                                lineNumber,
                                columnNumber,
                                0,
                                0,
                                match.Groups["End"].Value,
                                string.Empty,
                                string.Empty);
                            buildEngine.LogErrorEvent(errorArgs);
                            success = false;
                            index++;
                            if (index == 100)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            buildEngine.LogMessageEvent(args);
            return success;
        }
    }
}
