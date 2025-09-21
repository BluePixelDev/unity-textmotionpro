using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BP.TextMotionPro.Editor
{
    [CustomEditor(typeof(MotionProfile))]
    public class MotionProfileEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset treeAsset;
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            treeAsset.CloneTree(root);

            var currentTab = serializedObject.FindProperty("seletedTabIndex");
            var tabView = root.Q<TabView>();

            tabView.selectedTabIndex = currentTab.intValue;
            tabView.activeTabChanged += (s, e) =>
            {
                currentTab.intValue = tabView.selectedTabIndex;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            };

            var listAliases = root.Q<ListView>("list-aliases");

            return root;
        }
    }
}
