using System.Configuration;

namespace OntimerConverter
{
    public class ScriptReader
    {
        private DirectoryInfo SourceDirectory { get; }

        public ScriptReader()
        {
            SourceDirectory = new DirectoryInfo(ConfigurationManager.AppSettings["SourceFolder"] ?? "");

            if (!SourceDirectory.Exists)
            {
                throw new DirectoryNotFoundException($"Source folder was not found at ${SourceDirectory.FullName}");
            }
        }

        public IEnumerable<FileInfo> GetScriptsInfo()
        {
            DirectoryInfo sourceFolderInfo = new DirectoryInfo(SourceDirectory.FullName);

            return sourceFolderInfo.EnumerateFiles("*.cfg", SearchOption.AllDirectories);
        }

        public string GetFileContents(FileInfo fileInfo)
        {
            using (StreamReader streamReader = fileInfo.OpenText())
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}