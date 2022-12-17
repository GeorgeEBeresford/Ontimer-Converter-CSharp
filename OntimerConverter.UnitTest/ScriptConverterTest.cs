namespace OntimerConverter.UnitTest
{
    [TestClass]
    public class ScriptConverterTest
    {
        private ScriptConverter ScriptConverter { get; }

        public ScriptConverterTest()
        {
            ScriptConverter = new ScriptConverter();
        }

        [TestMethod]
        public void AddOntimerToCommands()
        {
            // Semi colon delimiters
            string nonOntimerScript = @"echo cool;echo cool2;echo cool3;echo cool4";
            string result = ScriptConverter.ConvertScript(nonOntimerScript);

            Assert.AreEqual("ontimer 0 \"echo cool\"\nontimer 10 \"echo cool2\"\nontimer 20 \"echo cool3\"\nontimer 30 \"echo cool4\"", result);

            // Newline delimiters
            nonOntimerScript = "echo cool\necho cool2\necho cool3\necho cool4";
            result = ScriptConverter.ConvertScript(nonOntimerScript);

            Assert.AreEqual("ontimer 0 \"echo cool\"\nontimer 10 \"echo cool2\"\nontimer 20 \"echo cool3\"\nontimer 30 \"echo cool4\"", result);

            // Semicolon and newline delimiters
            nonOntimerScript = "echo cool;\necho cool2;\necho cool3;\necho cool4";
            result = ScriptConverter.ConvertScript(nonOntimerScript);

            Assert.AreEqual("ontimer 0 \"echo cool\"\nontimer 10 \"echo cool2\"\nontimer 20 \"echo cool3\"\nontimer 30 \"echo cool4\"", result);
        }

        [TestMethod]
        public void ChangeOntimerDelay()
        {
            // Semi colon delimiters
            string fullyOntimerScript = @"ontimer 0 echo cool;ontimer 30 echo cool2; ontimer 60 echo cool3; ontimer 90 echo cool4";
            string result = ScriptConverter.ConvertScript(fullyOntimerScript);

            Assert.AreEqual("ontimer 0 \"echo cool\"\nontimer 10 \"echo cool2\"\nontimer 20 \"echo cool3\"\nontimer 30 \"echo cool4\"", result);

            // Newline delimiters
            fullyOntimerScript = "ontimer 0 echo cool\nontimer 30 echo cool2\n ontimer 60 echo cool3\nontimer 90 echo cool4";
            result = ScriptConverter.ConvertScript(fullyOntimerScript);

            Assert.AreEqual("ontimer 0 \"echo cool\"\nontimer 10 \"echo cool2\"\nontimer 20 \"echo cool3\"\nontimer 30 \"echo cool4\"", result);

            // Semicolon and newline delimiters
            fullyOntimerScript = "ontimer 0 echo cool;\nontimer 30 echo cool2;\n ontimer 60 echo cool3;\nontimer 90 echo cool4;";
            result = ScriptConverter.ConvertScript(fullyOntimerScript);
            Assert.AreEqual("ontimer 0 \"echo cool\"\nontimer 10 \"echo cool2\"\nontimer 20 \"echo cool3\"\nontimer 30 \"echo cool4\"", result);

            // Command is inside quotation marks
            fullyOntimerScript = "ontimer 0 \"echo cool\"\nontimer 30 \"echo cool2\"\n ontimer 60 \"echo cool3\"\nontimer 90 \"echo cool4\"";
            result = ScriptConverter.ConvertScript(fullyOntimerScript);
            Assert.AreEqual("ontimer 0 \"echo cool\"\nontimer 10 \"echo cool2\"\nontimer 20 \"echo cool3\"\nontimer 30 \"echo cool4\"", result);
        }
    }
}