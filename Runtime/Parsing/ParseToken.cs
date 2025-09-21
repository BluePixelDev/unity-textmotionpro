using System;
using System.Text.RegularExpressions;

namespace BP.TextMotionPro.Parsing
{
    public class ParseToken : IComparable<ParseToken>
    {
        public bool IsClosing { get; private set; }
        public string Name { get; private set; }
        public string Value { get; private set; }
        public int Index { get; private set; }
        public int Length { get; private set; }

        public ParseToken(Match match)
        {
            IsClosing = match.Groups[1].Value == "/";
            Name = match.Groups[2].Value;
            Value = match.Groups[3].Value;
            Index = match.Index;
            Length = match.Length;
        }

        public int CompareTo(ParseToken other) => Index - other.Index;
        public override string ToString() => $"{Name} {(IsClosing ? "[/]" : "[ ]")} \"{Value}\" (pos: {Index}, len: {Length})";
    }
}
