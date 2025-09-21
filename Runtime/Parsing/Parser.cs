using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("BP.TextMotionPro.Tests")]
namespace BP.TextMotionPro.Parsing
{
    internal sealed class ParseResult
    {
        public string CleanText { get; private set; }
        public readonly TagRange[] rangeArray;

        public TagRange[] Ranges => rangeArray;

        public ParseResult(ICollection<TagRange> ranges, string cleanText)
        {
            rangeArray = ranges.ToArray();
            CleanText = cleanText;
        }
    }

    internal static class Parser
    {
        private static readonly Regex TagPattern = new(@"<(\/?)((?:[-\w]+))(?:[= :]([^><]*))?>", RegexOptions.Compiled);
        public static ParseResult Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Input cannot be null or empty.", nameof(text));

            var tagTokens = new SortedSet<ParseToken>();
            foreach (Match match in TagPattern.Matches(text))
            {
                tagTokens.Add(new ParseToken(match));
            }

            var tagRanges = new List<TagRange>();
            var openTags = new List<ParseToken>();
            var cleanText = new StringBuilder();

            int lastIndex = 0;
            int lastIndexClean = 0;
            int cleanOffset = 0;

            foreach (var token in tagTokens)
            {
                if (ReservedTags.IsReserved(token.Name))
                    continue;

                if (token.Index > lastIndexClean)
                {
                    string betweenText = text[lastIndex..token.Index];
                    cleanText.Append(betweenText);
                    int segmentLength = betweenText.Length;

                    if (segmentLength > 0 && openTags.Count > 0)
                    {
                        tagRanges.Add(new TagRange(
                            cleanOffset,
                            cleanOffset + segmentLength,
                            new List<ParseToken>(openTags)
                        ));
                    }

                    cleanOffset += segmentLength;
                }

                if (token.IsClosing)
                {
                    var index = openTags.FindLastIndex(t => t.Name == token.Name);
                    if (index != -1)
                    {
                        openTags.RemoveAt(index);
                    }
                }
                else
                {
                    openTags.Add(token);
                }

                lastIndex = token.Index + token.Length;
                lastIndexClean = lastIndex - cleanOffset;
            }

            if (lastIndex < text.Length)
            {
                string remaining = text[lastIndex..];
                cleanText.Append(remaining);
                int segmentLength = remaining.Length;

                if (segmentLength > 0 && openTags.Any())
                {
                    tagRanges.Add(new TagRange(
                        cleanOffset,
                        cleanOffset + segmentLength,
                        new List<ParseToken>(openTags)
                    ));
                }

                cleanOffset += segmentLength;
            }

            return new ParseResult(tagRanges, cleanText.ToString());
        }
    }
}
