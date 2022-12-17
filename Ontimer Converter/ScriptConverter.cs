using System.Configuration;
using System.Text.RegularExpressions;

namespace OntimerConverter
{
    public class ScriptConverter
    {
        /// <summary>
        /// The amount of milliseconds automatically added between each command
        /// </summary>
        public int DelayPerCommand { get; }

        /// <summary>
        /// The amount of milliseconds added to the next ontimer command
        /// </summary>
        private int CurrentDelay { get; set; }

        /// <summary>
        /// Creates a new <see cref="ScriptConverter"/>
        /// </summary>
        public ScriptConverter()
        {
            DelayPerCommand = Convert.ToInt32(ConfigurationManager.AppSettings["Delay"] ?? "0");

            if (DelayPerCommand <= 0)
            {
                Console.WriteLine("Delay must be provided with a value that's greater than 0. Defaulting to 1000 (1 second)");
                DelayPerCommand = 1000;
            }
        }

        public string ConvertScript(string scriptContents)
        {
            CurrentDelay = 0;

            IEnumerable<string> scriptLines = scriptContents
                .Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(scriptLine => scriptLine.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

            IEnumerable<string> convertedScriptLines = scriptLines
                .Select(ConvertScriptLine)
                .Where(scriptLine => scriptLine != "");

            string convertedScriptContents = string.Join('\n', convertedScriptLines);
            return convertedScriptContents;
        }

        private string ConvertScriptLine(string scriptLine)
        {
            string extractedCommand = ExtractCommandFromScriptLine(scriptLine);
            int manualWait = ExtractManualDelayFromScriptLine(scriptLine);

            if (manualWait != 0) {

                // The wait command uses centiseconds and the ontimer command uses milliseconds
                CurrentDelay += (manualWait * 10);
                return "";
            }
            else
            {
                string ontimerCommand = $"ontimer {CurrentDelay} \"{extractedCommand}\"";
                CurrentDelay += DelayPerCommand;
                return ontimerCommand;
            }
        }

        private int ExtractManualDelayFromScriptLine(string scriptLine)
        {
            Match delayRegexMatch = Regex.Match(scriptLine, "wait ([0-9]+);?");

            // Check if the script line is a manual wait command
            if (delayRegexMatch.Success && delayRegexMatch.Groups.Count > 1)
            {
                Group capturedValue = delayRegexMatch.Groups.Cast<Group>().Last();
                string manualDelay = capturedValue.Value;

                bool isValid = int.TryParse(manualDelay, out int parsedDelay);
                return isValid ? parsedDelay : 0;
            }

            return 0;
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
