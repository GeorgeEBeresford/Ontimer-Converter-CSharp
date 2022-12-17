using System.Configuration;
using System.Text.RegularExpressions;
using System.Linq;

namespace OntimerConverter
{
    public class Program
    {
        private ScriptReader ScriptReader { get; }
        private ScriptWriter ScriptWriter { get; }
        private ScriptConverter ScriptConverter { get; }

        public Program()
        {
            ScriptReader = new ScriptReader();
            ScriptWriter = new ScriptWriter();
            ScriptConverter = new ScriptConverter();
        }

        public static void Main(string[] args)
        {
            Program program = new Program();
            program.CompileScripts();
        }

        public void CompileScripts()
        {
            ScriptWriter.ClearBinFolder();

            IEnumerable<FileInfo> scripts = ScriptReader.GetScriptsInfo();
            foreach(FileInfo script in scripts)
            {
                string scriptContents = ScriptReader.GetFileContents(script);
                string convertedScriptContents = ScriptConverter.ConvertScript(scriptContents);
                ScriptWriter.WriteScript(script, convertedScriptContents);
            }
        }
    }
}