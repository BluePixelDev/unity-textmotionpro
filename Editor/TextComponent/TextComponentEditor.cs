using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BP.TextMotionPro.Editor
{
    [CustomEditor(typeof(TextComponent), true, isFallback = true)]
    public class TextComponentEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset treeAsset;
        public override VisualElement CreateInspectorGUI()
        {
            var rootElement = new VisualElement();
            treeAsset.CloneTree(rootElement);

            var iterator = serializedObject.GetIterator();
            iterator.Next(true);
            while (iterator.NextVisible(false))
            {
                if (iterator.name == "m_Script")
                    continue;

                var propField = new PropertyField();
                propField.BindProperty(iterator);
                rootElement.Add(propField);
            }

            return rootElement;
        }
    }
}
