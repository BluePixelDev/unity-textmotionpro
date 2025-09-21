using UnityEngine;

namespace BP.TextMotionPro
{
    [CreateAssetMenu(menuName = "Scriptable/Rainbow")]
    [TextComponentInfo(TextComponentRole.Effect, name: "rainbow")]
    public class RainbowEffect : TextComponent
    {
        [SerializeField] private float charOffset = 0.1f;
        [SerializeField] private float offsetScale = 0.1f;

        public override void Apply(in CharMod charMod, in CharState charState)
        {
            float t = Mathf.Repeat(charState.animationTime * 0.5f + charState.characterIndex * charOffset, 1f);
            CharMod.FastHSVToRGB(t, out byte r, out byte g, out byte b);
            charMod.SetColor(r, g, b);
        }
    }
}
