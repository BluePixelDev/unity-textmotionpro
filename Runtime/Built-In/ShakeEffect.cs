using UnityEngine;

namespace BP.TextMotionPro
{
    [CreateAssetMenu(menuName = "Scriptable/Shake")]
    [TextComponentInfo(TextComponentRole.Effect, name: "shake")]
    public class ShakeEffect : TextComponent
    {
        [SerializeField] private float shakeAmount = 2f;
        [SerializeField] private float randomShakeInterval = 0.1f;

        public override bool IsActive() => shakeAmount > 0;

        public override void Apply(in CharMod charMod, in CharState charState)
        {
            if (charState.animationTime % randomShakeInterval > randomShakeInterval * 0.5f) return;
            float shakeX = Random.Range(-shakeAmount, shakeAmount);
            float shakeY = Random.Range(-shakeAmount, shakeAmount);
            charMod.Move(shakeX, shakeY);
        }
    }
}
