using BP.TextMotionPro.Parsing;
using NUnit.Framework;

namespace BP.TextMotionPro.Tests
{
    public class ParserTests
    {
        [Test]
        public void Parse_MultipleTags_CorrectParseResult()
        {
            string input = "<wave>Hello</wave>";
            var result = Parser.Parse(input);
            AssertParserResult(result, 1);

            var tags = result.Ranges[0].Tags;
            foreach (var tag in tags)
            {
                Assert.AreEqual(tag.Name, "wave");
            }
        }
        [Test]
        public void Parse_MultipleNonNestedTags_CorrectParseResult()
        {
            string input = "<wave>Hello</wave> <wave>World</wave>";
            var result = Parser.Parse(input);
            AssertParserResult(result, 2);
        }
        [Test]
        public void Parse_NestedTags_CorrectParseResult()
        {
            string input = "<bold><wave>Hello</wave></bold>";
            var result = Parser.Parse(input);
            AssertParserResult(result, 1);
        }
        [Test]
        public void Parse_OverlappingTags_CorrectParseResult()
        {
            string input = "<bold>Hello <wave>there</bold> friend</wave>";
            var result = Parser.Parse(input);
            AssertParserResult(result, 3);
        }
        [Test]
        public void Parse_MultipleOpeningTags_CorrectParseResult()
        {
            string input = "<bold><wave>Hello there friend";
            var result = Parser.Parse(input);
            AssertParserResult(result, 1);
        }

        private void AssertParserResult(ParseResult result, int expectedRangesCount)
        {
            Assert.NotNull(result);
            Assert.NotNull(result.Ranges);
            Assert.AreEqual(expectedRangesCount, result.Ranges.Length);
        }
    }
}
