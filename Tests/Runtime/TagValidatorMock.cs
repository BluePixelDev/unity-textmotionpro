using BP.TextMotionPro.Parsing;
using System.Collections.Generic;

namespace BP.TextMotionPro.Tests
{
    public class TagValidatorMock : ITokenValidator
    {
        private readonly HashSet<string> validTags = new();

        public TagValidatorMock(params string[] tags)
        {
            foreach (var tag in tags)
            {
                validTags.Add(tag);
            }
        }

        public bool Validate(ParseToken token)
        {
            return validTags.Contains(token.Name);
        }
    }
}
