using System.Configuration;
using System.Text.RegularExpressions;

namespace OntimerConverter
{
    public class ScriptConverter
    {
        public int Delay { get; }
        private int ManualWait { get; set; }

        public ScriptConverter()
        {
            Delay = Convert.ToInt32(ConfigurationManager.AppSettings["Delay"] ?? "0");

            if (Delay <= 0)
            {
                Console.WriteLine("Delay must be provided with a value that's greater than 0. Defaulting to 1000 (1 second)");
                Delay = 1000;
            }
        }

        public string ConvertScript(string scriptContents)
        {
            IEnumerable<string> scriptLines = scriptContents.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            scriptLines = scriptLines.SelectMany(scriptLine => scriptLine.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            IEnumerable<string> convertedScriptLines = scriptLines.Select(ConvertScriptLine);
            string convertedScriptContents = string.Join('\n', convertedScriptLines);

            return convertedScriptContents;
        }

        private string ConvertScriptLine(string scriptLine, int lineIndex)
        {
            int currentDelay = lineIndex * Delay;
            string extractedCommand = ExtractCommandFromScriptLine(scriptLine);

            return $"ontimer {currentDelay} \"{extractedCommand}\"";
        }

        /// <summary>
        /// Checks whether the command is already using ontimer. If it is then return the command 
        /// </summary>
        /// <param name="scriptLine"></param>
        /// <returns></returns>
        private string ExtractCommandFromScriptLine(string scriptLine)
        {
            Match existingRegexMatch = Regex.Match(scriptLine, "ontimer [0-9]+ \"?([^\"]+)\"?;?");

            // Check if the script line is already an ontimer command
            if (existingRegexMatch.Success && existingRegexMatch.Groups.Count > 1)
            {
                Group capturedValue = existingRegexMatch.Groups.Cast<Group>().Last();
                return capturedValue.Value;
            }

            return scriptLine;
        }
    }
}
