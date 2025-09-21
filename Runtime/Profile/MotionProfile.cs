using System;
using System.Collections.Generic;
using UnityEngine;

namespace BP.TextMotionPro
{
    [CreateAssetMenu(fileName = "New TypeUp Profile", menuName = "TypeUp/Profile")]
    public class MotionProfile : ScriptableObject
    {
        [System.Serializable]
        private class TagAliasPair : ISerializationCallbackReceiver
        {
            [SerializeField] private string tag;
            [SerializeField] private string alias;

            public string Tag => tag;
            public string Alias => alias;

            public void OnAfterDeserialize() { }
            public void OnBeforeSerialize()
            {
                tag = tag?.Trim();
            }
        }

#pragma warning disable
        [SerializeField, HideInInspector] private int seletedTabIndex = 0;
        [SerializeField] private List<TagAliasPair> tagAliases = new();
        [SerializeField] private TextComponentCollection<TextComponent> tagEffects = new();

        public event Action OnProfileChanged;

        public TextComponentCollection<TextComponent> TagEffects => tagEffects;
    }
}
