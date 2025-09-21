using UnityEngine;

namespace BP.TextMotionPro
{
    public abstract class TextComponent : ScriptableObject
    {
        public virtual bool IsActive() => true;
        public abstract void Apply(in CharMod charMod, in CharState charState);
    }
}
