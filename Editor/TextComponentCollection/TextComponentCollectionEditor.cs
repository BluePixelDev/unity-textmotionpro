using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BP.TextMotionPro.Editor
{
    [CustomPropertyDrawer(typeof(TextComponentCollection<>))]
    public class TextComponentCollectionEditor : PropertyDrawer
    {
        [SerializeField] private VisualTreeAsset asset;
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var rootElement = new VisualElement();
            asset.CloneTree(rootElement);

            var listProperty = property.FindPropertyRelative("list");
            var listElement = rootElement.Q<ListView>();
            listElement.bindItem = (elm, i) =>
            {
                var arrayProp = listProperty.GetArrayElementAtIndex(i);

                var keyElement = elm.Q<TextField>("text-key");
                keyElement.BindProperty(arrayProp.FindPropertyRelative("key"));

                var imguiContainer = elm.Q<IMGUIContainer>("editor-imgui");
                var uiToolkitContainer = elm.Q("editor-toolkit");

                var componentProperty = arrayProp.FindPropertyRelative("component");
                var targetObject = componentProperty.objectReferenceValue;

                if (!targetObject)
                    return;

                var componentEditor = UnityEditor.Editor.CreateEditor(targetObject);

                var toolkitInspector = componentEditor.CreateInspectorGUI();
                if (toolkitInspector != null)
                {
                    uiToolkitContainer.Add(toolkitInspector);
                }
                else
                {
                    imguiContainer.onGUIHandler = componentEditor.OnInspectorGUI;
                }
            };

            return rootElement;
        }
    }
}
