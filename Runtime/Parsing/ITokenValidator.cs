using BP.TextMotionPro.Parsing;

namespace BP.TextMotionPro
{
    public interface ITokenValidator
    {
        public bool Validate(ParseToken token);
    }
}
