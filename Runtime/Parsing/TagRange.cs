using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BP.TextMotionPro.Parsing
{
    internal readonly struct TagRange : IEquatable<TagRange>
    {
        public readonly int startIndex;
        public readonly int endIndex;
        public IReadOnlyList<ParseToken> Tags { get; }

        public TagRange(int startIndex, int endIndex, IReadOnlyList<ParseToken> tags)
        {
            this.startIndex = startIndex;
            this.endIndex = endIndex;
            Tags = tags;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(int index)
        {
            if (Tags == null || index < 0)
                return false;

            return startIndex <= index && endIndex >= index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"Range({startIndex}-{endIndex}) with {(Tags != null ? Tags.Count : 0)} tag(s)";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is TagRange range &&
                   startIndex == range.startIndex &&
                   endIndex == range.endIndex;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(startIndex, endIndex);
        }

        public bool Equals(TagRange other) =>
            startIndex == other.startIndex && endIndex == other.endIndex;
    }
}
