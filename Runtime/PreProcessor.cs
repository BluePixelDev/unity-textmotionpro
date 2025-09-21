using BP.TextMotionPro.Parsing;
using TMPro;

namespace BP.TextMotionPro
{
    internal class PreProcessor : ITextPreprocessor
    {
        private string cachedInput = null;
        private string cachedResult;
        private ParseResult parseResult;

        public ParseResult ParseResult => parseResult;

        public string PreprocessText(string text)
        {
            if (text == cachedInput && parseResult != null) return cachedResult;

            ClearCache();
            cachedInput = text;
            parseResult = Parser.Parse(text);
            cachedResult = parseResult.CleanText;

            return cachedResult;
        }

        public void ClearCache()
        {
            cachedInput = null;
            cachedResult = null;
        }

        public bool TryGetRangeAt(int index, out TagRange result)
        {
            if (parseResult == null)
            {
                result = default;
                return false;
            }

            var ranges = parseResult.Ranges;

            if (index < 0 || ranges.Length == 0)
            {
                result = default;
                return false;
            }

            int left = 0;
            int right = ranges.Length - 1;
            while (left <= right)
            {
                int mid = (left + right) / 2;
                ref readonly var lookupRange = ref ranges[mid];

                if (lookupRange.Contains(index))
                {
                    result = lookupRange;
                    return true;
                }
                else if (index < lookupRange.startIndex)
                    right = mid - 1;
                else
                    left = mid + 1;
            }

            result = default;
            return false;
        }
    }
}
