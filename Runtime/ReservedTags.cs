using System.Collections.Generic;

namespace BP.TextMotionPro
{
    public static class ReservedTags
    {
        private static readonly HashSet<string> reservedDict = new()
        {
            "i", "b", "u", "s", "sup", "sub", "mark", "color", "alpha", "size", "font",
            "material", "link", "sprite", "space", "indent", "line-height", "rotate",
            "lowercase", "uppercase", "smallcaps"
        };

        public static bool IsReserved(string tag) => reservedDict.Contains(tag.ToLower());
    }
}
