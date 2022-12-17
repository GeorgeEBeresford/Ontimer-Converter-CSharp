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
        public void ConvertScripts()
        {
            // Already ontimered script with semi colon delimiters
            string fullyOntimerScript = @"ontimer 0 echo cool;ontimer 30 echo cool2; ontimer 60 echo cool3; ontimer 90 echo cool4";
            string result = ScriptConverter.ConvertScript(fullyOntimerScript);

            Assert.AreEqual("ontimer 0 \"echo cool\"\nontimer 10 \"echo cool2\"\nontimer 20 \"echo cool3\"\nontimer 30 \"echo cool4\"", result);

            // Already ontimered script with newline delimiters
            fullyOntimerScript = "ontimer 0 echo cool\nontimer 30 echo cool2\n ontimer 60 echo cool3\nontimer 90 echo cool4";
            result = ScriptConverter.ConvertScript(fullyOntimerScript);

            Assert.AreEqual("ontimer 0 \"echo cool\"\nontimer 10 \"echo cool2\"\nontimer 20 \"echo cool3\"\nontimer 30 \"echo cool4\"", result);

            // Already ontimered script with semicolon and newline delimiters
            fullyOntimerScript = "ontimer 0 echo cool;\nontimer 30 echo cool2;\n ontimer 60 echo cool3;\nontimer 90 echo cool4;";
            result = ScriptConverter.ConvertScript(fullyOntimerScript);

            Assert.AreEqual("ontimer 0 \"echo cool\"\nontimer 10 \"echo cool2\"\nontimer 20 \"echo cool3\"\nontimer 30 \"echo cool4\"", result);
        }
    }
}