using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("BP.TextMotionPro.Editor.Tests")]
[assembly: InternalsVisibleTo("BP.TextMotionPro.Editor")]

namespace BP.TextMotionPro
{
    internal class RegisteredTextComponent
    {
        public string Name { get; }
        public string Description { get; }
        public string Version { get; }
        public TextComponentRole Role { get; }
        public TextComponent Instance { get; }

        public RegisteredTextComponent(string name, string description, string version, TextComponentRole role, TextComponent instance)
        {
            Name = name;
            Description = description;
            Version = version;
            Role = role;
            Instance = instance;
        }
    }

    internal static class ComponentRegistry
    {
        private readonly static Dictionary<string, RegisteredTextComponent> effects = new();
        private readonly static Dictionary<string, RegisteredTextComponent> transitions = new();


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        internal static void Initialize()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a =>
            {
                var name = a.GetName().Name;
                return !name.StartsWith("Unity", StringComparison.OrdinalIgnoreCase)
                       && !name.StartsWith("System", StringComparison.OrdinalIgnoreCase)
                       && !name.StartsWith("mscorlib", StringComparison.OrdinalIgnoreCase)
                       && !name.StartsWith("netstandard", StringComparison.OrdinalIgnoreCase)
                       && !name.StartsWith("Mono.", StringComparison.OrdinalIgnoreCase);
            });

            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types.Where(t => t != null).ToArray();
                }

                foreach (var type in types)
                {
                    if (!type.IsSubclassOf(typeof(TextComponent)) || type.IsAbstract)
                        continue;

                    var attr = type.GetCustomAttribute<TextComponentInfoAttribute>();
                    if (attr == null) continue;

                    var instance = ScriptableObject.CreateInstance(type) as TextComponent;
                    var registeredComponent = new RegisteredTextComponent(
                        attr.Name, attr.Description, attr.Version, attr.Role, instance
                    );

                    switch (attr.Role)
                    {
                        case TextComponentRole.Effect:
                            effects[attr.Name] = registeredComponent;
                            break;
                        case TextComponentRole.Transition:
                            transitions[attr.Name] = registeredComponent;
                            break;
                    }
                }
            }
        }

        public static TextComponent GetEffect(string name) =>
        effects.TryGetValue(name, out var effect) ? effect.Instance : null;

        public static TextComponent GetTransition(string name) =>
            transitions.TryGetValue(name, out var transition) ? transition.Instance : null;

        public static IEnumerable<RegisteredTextComponent> GetAllEffects() => effects.Values;
        public static IEnumerable<RegisteredTextComponent> GetAllTransitions() => transitions.Values;
    }
}
