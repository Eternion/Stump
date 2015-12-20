using System;
using System.Text.RegularExpressions;

namespace DofusProtocolBuilder.Parsing
{
    public class TypeReference
    {
        public const string RegExpType = @"(?<type>[\w\d_\.]+)(?:\<(?<generic>[\w\d\._<>]+)\>)?";

        public TypeReference(string fullType)
        {
            var regex = new Regex(RegExpType, RegexOptions.Compiled);
            var match = regex.Match(fullType);
            var type = match.Groups["type"].Value;

            var dotLastIndex = type.LastIndexOf(".", StringComparison.Ordinal);
            Name = dotLastIndex != -1 ? type.Substring(dotLastIndex + 1) : type;
            Namespace = dotLastIndex != -1 ? type.Substring(0, dotLastIndex + 1) : "";

            if (match.Groups["generic"].Success)
                GenericType = new TypeReference(match.Groups["generic"].Value);
        }

        public string Name
        {
            get;
            set;
        } 

        public string Namespace
        {
            get;
            set;
        }

        public bool HasNamespace => !string.IsNullOrEmpty(Namespace);

        public TypeReference GenericType
        {
            get;
            set;
        }

        public bool HasGenericType => !string.IsNullOrEmpty(GenericType?.Name);

        public override string ToString()
        {
            return Name + (HasGenericType ? $"<{GenericType}>" : "");
        }


        public string ToString(bool hideNamespace)
        {
            if (hideNamespace)
                return ToString();

            return HasNamespace ? $"{Namespace}.{Name}" : Name + (HasGenericType ? $"<{GenericType}>" : "");
        }
    }
}