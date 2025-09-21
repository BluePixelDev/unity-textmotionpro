using UnityEditor;

namespace BP.TextMotionPro.Editor
{
    [InitializeOnLoad]
    internal static class ComponentRegistryEditorBootstrap
    {
        static ComponentRegistryEditorBootstrap
        ()
        {
            ComponentRegistry.Initialize();
        }
    }
}
