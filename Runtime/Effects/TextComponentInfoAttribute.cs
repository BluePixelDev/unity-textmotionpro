using System;

namespace BP.TextMotionPro
{
    public enum TextComponentRole
    {
        Effect,
        Transition
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TextComponentInfoAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public TextComponentRole Role { get; set; }

        public TextComponentInfoAttribute(TextComponentRole role, string name, string description = "", string version = "1.0")
        {
            Name = name;
            Description = description;
            Version = version;
            Role = role;
        }
    }
}
