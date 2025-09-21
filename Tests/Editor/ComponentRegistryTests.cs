using NUnit.Framework;

namespace BP.TextMotionPro.Editor.Tests
{
    [TextComponentInfo(TextComponentRole.Effect, "mockup", "mockup text effect", "1.0")]
    public class MockupEffectComponent : TextComponent
    {
        public override void Apply(in CharMod charMod, in CharState charState)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ComponentRegistryTests
    {
        [SetUp]
        public void SetUp()
        {
            ComponentRegistry.Initialize();
        }

        public void Registry_ContainsMockupComponent()
        {
            var effect = ComponentRegistry.GetEffect("mockup");
            Assert.IsNotNull(effect);
        }
    }
}
