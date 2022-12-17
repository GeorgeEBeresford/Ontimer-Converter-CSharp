using System.Configuration;
using System.Text.RegularExpressions;

namespace OntimerConverter
{
    public class ScriptConverter
    {
        public int Delay { get; }

        public ScriptConverter()
        {
            Delay = Convert.ToInt32(ConfigurationManager.AppSettings["Delay"] ?? "0");

            if (Delay == 0)
            {
                Console.WriteLine("Delay must be provided with a value that's greater than 0. Defaulting to 1000 (1 second)");
                Delay = 1000;
            }
        }

        public string ConvertScript(string scriptContents)
        {
            IEnumerable<string> scriptLines = scriptContents.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            scriptLines = scriptLines.SelectMany(scriptLine => scriptLine.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            IEnumerable<string> convertedScriptLines = ConvertScriptLines(scriptLines);
            string convertedScriptContents = string.Join('\n', convertedScriptLines);

            return convertedScriptContents;
        }

        private IEnumerable<string> ConvertScriptLines(IEnumerable<string> scriptLines)
        {
            return scriptLines.Select(ConvertScriptLine);
        }

        private string ConvertScriptLine(string scriptLine, int lineIndex)
        {
            int currentDelay = lineIndex * Delay;
            Match existingRegexMatch = Regex.Match(scriptLine, "ontimer [0-9]+ \"(.+)\"");

            if (existingRegexMatch.Success && existingRegexMatch.Groups.Count > 1)
            {
                Group capturedValue = existingRegexMatch.Groups.Cast<Group>().Last();

                return $"ontimer {currentDelay} \"{capturedValue.Value}\"";
            }

            return $"ontimer {currentDelay} \"{scriptLine}\"";
        }
    }
}
