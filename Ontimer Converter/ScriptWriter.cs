using System.Configuration;

namespace OntimerConverter
{
    public class ScriptWriter
    {
        private DirectoryInfo BinDirectory { get; }

        public ScriptWriter()
        {
            BinDirectory = new DirectoryInfo(ConfigurationManager.AppSettings["BinFolder"] ?? "");
        }

        public void ClearBinFolder()
        {
            ClearFolder(BinDirectory);
        }

        private void ClearFolder(DirectoryInfo directoryInfo)
        {
            if (!BinDirectory.Exists)
            {
                return;
            }

            // Delete all of the files
            foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles())
            {
                fileInfo.Delete();
            }

            // Delete all of the directories
            foreach (DirectoryInfo subDirectory in directoryInfo.EnumerateDirectories())
            {
                ClearFolder(subDirectory);
            }

            directoryInfo.Delete();
        }

        public void WriteScript(FileInfo inputFile, string script)
        {
            string inputSubDirectory = inputFile.Directory.FullName.Substring(ConfigurationManager.AppSettings["SourceFolder"].Length);
            string outputFileName = inputFile.Name.Substring(0, inputFile.Name.Length - inputFile.Extension.Length);
            FileInfo outputFile = new FileInfo($@"{BinDirectory.FullName}\{inputSubDirectory}\{outputFileName}.cfg");

            DirectoryInfo outputDirectory = outputFile.Directory;
            if (!outputDirectory.Exists)
            {
                outputDirectory.Create();
            }

            using (StreamWriter streamWriter = new StreamWriter(outputFile.OpenWrite()))
            {
                streamWriter.Write(script);
            }
        }
    }
}
