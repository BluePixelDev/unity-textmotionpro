using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BP.TextMotionPro.Editor
{
    public class RegistryDebugEditor : EditorWindow
    {
        private Vector2 scrollPos;
        private bool showEffects = true;
        private bool showTransitions = true;

        [MenuItem("Window/TextMotionPro/Registry Debug")]
        public static void Open()
        {
            var window = GetWindow<RegistryDebugEditor>();
            window.titleContent = new GUIContent("Component Registry Debug");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("TextComponent Registry", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("This window shows all registered Effects and Transitions, including metadata.", MessageType.Info);
            EditorGUILayout.Space();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            // Effects Section
            showEffects = EditorGUILayout.Foldout(showEffects, $"Effects ({ComponentRegistry.GetAllEffects().Count()})", true);
            if (showEffects)
            {
                DrawComponentList(ComponentRegistry.GetAllEffects());
            }

            EditorGUILayout.Space(10);

            // Transitions Section
            showTransitions = EditorGUILayout.Foldout(showTransitions, $"Transitions ({ComponentRegistry.GetAllTransitions().Count()})", true);
            if (showTransitions)
            {
                DrawComponentList(ComponentRegistry.GetAllTransitions());
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawComponentList(IEnumerable<RegisteredTextComponent> components)
        {
            foreach (var component in components)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.LabelField("Name", component.Name, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Description", string.IsNullOrEmpty(component.Description) ? "<none>" : component.Description);
                EditorGUILayout.LabelField("Version", component.Version);
                EditorGUILayout.LabelField("Role", component.Role.ToString());

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }
        }
    }
}
