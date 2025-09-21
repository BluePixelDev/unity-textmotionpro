using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("BP.TypeUp")]
namespace BP.TextMotionPro
{
    [System.Serializable]
    public class TextComponentCollection<T> where T : TextComponent
    {
        [System.Serializable]
        private class ComponentEntry : ISerializationCallbackReceiver
        {
            [SerializeField, HideInInspector] private bool isFolded;
            [SerializeField, HideInInspector] private bool isActive;

            [SerializeField] private string key;
            [SerializeReference] private T component;

            public string Key => key;
            public T Component => component;

            public ComponentEntry(string key, T componenet)
            {
                this.key = key;
                component = componenet;
            }

            public void OnAfterDeserialize()
            {
                key = NormalizeKey(key);
            }
            public void OnBeforeSerialize()
            {
                key = NormalizeKey(key);
            }
        }

        [SerializeField] private List<ComponentEntry> list = new();
        private readonly Dictionary<int, ComponentEntry> componentCache = new();

        public bool TryGetComponent(string key, out T component)
        {
            if (TryFindComponent(key, out var wrapper))
            {
                component = wrapper.Component;
                return true;
            }

            component = default;
            return false;
        }

        public T GetComponent(string key) => TryFindComponent(key, out var wrapper) ? wrapper.Component : default;

        private bool TryFindComponent(string key, out ComponentEntry wrapper)
        {
            var targetHash = StringHash.FNV1aHash(key);
            if (componentCache.TryGetValue(targetHash, out wrapper))
                return true;

            foreach (var item in list)
            {
                if (item.Key == key)
                {
                    componentCache[targetHash] = item;
                    wrapper = item;
                    return true;
                }
            }

            componentCache.Remove(targetHash);
            wrapper = null;
            return false;
        }

        public void Clear()
        {
            list.Clear();
            componentCache.Clear();
        }

        public void Add(string key, T component)
        {
            list.Add(new ComponentEntry(NormalizeKey(key), component));
        }

        public void Remove(string key)
        {
            var targetHash = StringHash.FNV1aHash(key);
            if (componentCache.TryGetValue(targetHash, out var cached) && cached.Key == key)
                componentCache.Remove(targetHash);

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Key == key)
                {
                    list.RemoveAt(i);
                    break;
                }
            }
        }

        public string[] GetAllKeys() => list.Select(x => x.Key).ToArray();

        private static string NormalizeKey(string key) =>
            string.IsNullOrWhiteSpace(key) ? string.Empty : key.ToLowerInvariant().Replace(" ", "");
    }
}
