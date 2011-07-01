using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Stump.Core.Xml.Docs
{
	[XmlRoot("doc")]
	public class DotNetDocumentation : XmlFile<DotNetDocumentation>
	{
		private static readonly Dictionary<char, MemberType> TypeMap = new Dictionary<char, MemberType>();

		static DotNetDocumentation()
		{
			TypeMap.Add('T', MemberType.Type);
			TypeMap.Add('F', MemberType.Field);
			TypeMap.Add('P', MemberType.Property);
			TypeMap.Add('M', MemberType.Method);
			TypeMap.Add('E', MemberType.Event);
		}

		public static MemberType GetMemberType(char shortcut)
		{
			MemberType type;
			if (!TypeMap.TryGetValue(shortcut, out type))
			{
				throw new Exception("Undefined Type-shortcut: " + shortcut);
			}

			return type;
		}

		protected override void OnLoad()
		{

		}

		[XmlElement("assembly")]
        public AssemblyInfo Assembly
		{
			get;
			set;
		}

		[XmlArray("members")]
		[XmlArrayItem("member")]
		public DocEntry[] Members
		{
			get;
			set;
		}

        public class AssemblyInfo
        {
            [XmlElement("name")]
            public string Name
            {
                get;
                set;
            }
        }
	}
}