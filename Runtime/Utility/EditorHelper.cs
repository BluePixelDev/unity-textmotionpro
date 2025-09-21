using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BP.TextMotionPro
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    internal static class EditorHelper
    {
        public static Action EditorUpdate;

        static EditorHelper()
        {
            Cleanup();
#if UNITY_EDITOR
            EditorApplication.update += EditorUpdateHandler;
#endif
        }

        private static void Cleanup()
        {
            EditorUpdate = null;
#if UNITY_EDITOR
            EditorApplication.update -= EditorUpdateHandler;
#endif
        }

        private static void EditorUpdateHandler() => EditorUpdate?.Invoke();

        public static void QueueEditorUpdate()
        {
#if UNITY_EDITOR
            EditorApplication.QueuePlayerLoopUpdate();
#endif
        }

        public static bool IsSelected(GameObject gameObject)
        {
#if UNITY_EDITOR
            return Selection.Contains(gameObject);
#else
            return true;
#endif
        }

        public static T CreateAndSaveComponent<T>(MotionProfile profile) where T : TextComponent => (T)CreateAndSaveComponent(typeof(T), profile);
        public static TextComponent CreateAndSaveComponent(Type type, MotionProfile profile)
        {
            if (!type.IsSubclassOf(typeof(TextComponent)))
                throw new InvalidOperationException($"Invalid component type: {type}.");

            var instance = ScriptableObject.CreateInstance(type);
            instance.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            instance.name = type.Name;

#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(profile);
            AssetDatabase.AddObjectToAsset(instance, path);
            AssetDatabase.SaveAssets();
#endif
            return instance as TextComponent;
        }
    }
}
