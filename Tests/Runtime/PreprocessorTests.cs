using NUnit.Framework;

namespace BP.TextMotionPro.Tests
{
    public class PreprocessorTests
    {
        private PreProcessor processor;

        [SetUp]
        public void Init()
        {
            processor = new PreProcessor();
        }

        [Test]
        public void PreprocessText_SimpleText_ReturnCleanString()
        {
            string input = "<wave><b>Hello There</wave> John";
            string expecting = "Hello There John";

            string output = processor.PreprocessText(input);
            Assert.AreEqual(expecting, output);
        }
    }
}
