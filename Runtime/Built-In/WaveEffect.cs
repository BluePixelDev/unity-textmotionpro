using UnityEngine;

namespace BP.TextMotionPro
{
    [TextComponentInfo(TextComponentRole.Effect, name: "wave")]
    public class WaveEffect : TextComponent
    {
        [SerializeField] private float frequency = 6f;
        [SerializeField] private float amplitude = 3f;
        [SerializeField] private float offset = 20f;

        public void OnValidate()
        {
            frequency = Mathf.Clamp(frequency, 0, Mathf.Infinity);
            amplitude = Mathf.Clamp(amplitude, 0, Mathf.Infinity);
            offset = Mathf.Clamp(offset, 0, Mathf.Infinity);
        }

        public override bool IsActive() => offset != 0.0f;

        public override void Apply(in CharMod charMod, in CharState charState)
        {
            float funcOffset = charState.animationTime * frequency + charState.characterIndex * Mathf.Deg2Rad * offset;
            float waveOffset = Mathf.Sin(funcOffset) * amplitude;
            charMod.MoveY(waveOffset);
        }
    }
}
