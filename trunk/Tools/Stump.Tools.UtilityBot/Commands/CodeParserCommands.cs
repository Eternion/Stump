// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Squishy.Irc.Commands;
using Stump.BaseCore.Framework.Attributes;
using Stump.Tools.UtilityBot.FileParser;
using Stump.Tools.UtilityBot.FileWriter;

namespace Stump.Tools.UtilityBot.Commands
{
    public class PacketGeneratorCommand : Command
    {
        /// <summary>
        /// Output path
        /// </summary>
        [Variable]
        public static string Output = "./../../../DofusProtocol/Messages/Messages/";

        public PacketGeneratorCommand()
            : base("genpackets")
        {
            Description = "Generate packet's classes";
        }

        public override void Process(CmdTrigger trigger)
        {
            trigger.Reply("Generating packet's classes. Please wait.");

            IEnumerable<string> files =
                Directory.EnumerateFiles(
                    Bot.DofusSourcePath + @"\Scripts\ActionScript 3.0\com\ankamagames\dofus\network\messages\",
                    "*", SearchOption.AllDirectories);

            foreach (string file in files.Where((entry) => char.IsUpper(Path.GetFileName(entry)[0])))
            {
                var parser = new AsParser(file, SharedRules.m_replaceRules, SharedRules.m_beforeParsingRules,
                                          SharedRules.m_afterParsingRules, SharedRules.m_ignoredLines);
                try
                {
                    parser.ParseFile();
                }
                catch
                {
                    continue;
                }

                string[] splitPath = file.Split('\\');
                splitPath = splitPath.SkipWhile(entry => entry != "messages").Skip(1).ToArray();

                string dirPath = Path.GetFullPath(Output) + "/";
                foreach (string dir in splitPath)
                {
                    if (dirPath.Contains(".as"))
                        break;

                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    dirPath += dir + "/";
                }

                string path = (Output + string.Join("/", splitPath));

                parser.ToCSharp(path.Remove(path.Length - 2, 2) + "cs", "Stump.DofusProtocol.Messages",
                                new[]
                                    {
                                        "System",
                                        "System.Collections.Generic",
                                        "Stump.BaseCore.Framework.Utils",
                                        "Stump.BaseCore.Framework.IO",
                                        "Stump.DofusProtocol.Classes"
                                    });
            }

            trigger.Reply("Generated classes.");
        }
    }

    public class ClassesGeneratorCommand : Command
    {
        /// <summary>
        /// Output path
        /// </summary>
        [Variable]
        public static string Output = "./../../../DofusProtocol/Classes/Types/";

        public ClassesGeneratorCommand()
            : base("genclasses")
        {
            Description = "Generate type's classes";
        }

        public override void Process(CmdTrigger trigger)
        {
            trigger.Reply("Generating type's classes. Please wait.");

            IEnumerable<string> files =
                Directory.EnumerateFiles(
                    Bot.DofusSourcePath + @"\Scripts\ActionScript 3.0\com\ankamagames\dofus\network\types\",
                    "*", SearchOption.AllDirectories);

            foreach (string file in files.Where((entry) => char.IsUpper(Path.GetFileName(entry)[0])))
            {
                var parser = new AsParser(file, SharedRules.m_replaceRules, SharedRules.m_beforeParsingRules,
                                          SharedRules.m_afterParsingRules, SharedRules.m_ignoredLines);
                try
                {
                    parser.ParseFile();
                }
                catch
                {
                    continue;
                }

                string[] splitPath = file.Split('\\');
                splitPath = splitPath.SkipWhile(entry => entry != "types").Skip(1).ToArray();

                string dirPath = Path.GetFullPath(Output) + "/";
                foreach (string dir in splitPath)
                {
                    if (dirPath.Contains(".as"))
                        break;

                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    dirPath += dir + "/";
                }

                string path = (Output + string.Join("/", splitPath));

                parser.ToCSharp(path.Remove(path.Length - 2, 2) + "cs", "Stump.DofusProtocol.Classes",
                                new[]
                                    {
                                        "System",
                                        "System.Collections.Generic",
                                        "Stump.BaseCore.Framework.Utils",
                                        "Stump.BaseCore.Framework.IO"
                                    });
            }

            trigger.Reply("Generated classes.");
        }
    }

    public class D2OClassesGeneratorCommand : Command
    {
        /// <summary>
        /// Output path
        /// </summary>
        [Variable]
        public static string Output = "./../../../DofusProtocol/D2oClasses/Classes/";


        public D2OClassesGeneratorCommand()
            : base("gend2oclasses")
        {
            Description = "Generate d2o's classes";
        }

        public override void Process(CmdTrigger trigger)
        {
            trigger.Reply("Generating d2o's classes. Please wait.");

            IEnumerable<string> files =
                Directory.EnumerateFiles(
                    Bot.DofusSourcePath + @"\Scripts\ActionScript 3.0\com\ankamagames\dofus\datacenter\",
                    "*", SearchOption.AllDirectories);

            foreach (string file in files.Where((entry) => char.IsUpper(Path.GetFileName(entry)[0])))
            {
                var parser = new AsParser(file, true, SharedRules.m_replaceRules, SharedRules.m_beforeParsingRules,
                                          SharedRules.m_afterParsingRules, SharedRules.m_ignoredLines);
                try
                {
                    parser.ParseFile();
                }
                catch
                {
                    continue;
                }

                FieldInfo modulefield = parser.Fields.Where(entry => entry.Name == "MODULE").FirstOrDefault();

                if (modulefield != null)
                    parser.Class.CustomAttribute = "[AttributeAssociatedFile(" + modulefield.Value + ")]";

                // remove logger field
                parser.Fields.RemoveAll(entry => entry.Name == "_log");
                // remove internal fields that aren't arrays
                parser.Fields.RemoveAll(
                    entry =>
                    entry.Modifiers == AccessModifiers.INTERNAL && entry.Stereotype != "const" && entry.Type != "Array");

                foreach (
                    FieldInfo field in
                        parser.Fields.Where(entry => entry.Stereotype == "const" && entry.Type == "Array"))
                {
                    field.Stereotype = "static";
                }

                foreach (
                    FieldInfo field in
                        parser.Fields.Where(entry => entry.Name.StartsWith("_") && entry.Modifiers == AccessModifiers.PROTECTED))
                {
                    field.Name = field.Name.Remove(0, 1);
                    field.Modifiers = AccessModifiers.PUBLIC;
                }

                string[] splitPath = file.Split('\\');
                splitPath = splitPath.SkipWhile(entry => entry != "datacenter").Skip(1).ToArray();

                string dirPath = Path.GetFullPath(Output) + "/";
                foreach (string dir in splitPath)
                {
                    if (dirPath.Contains(".as"))
                        break;

                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    dirPath += dir + "/";
                }

                string path = (Output + string.Join("/", splitPath));

                parser.ToCSharp(path.Remove(path.Length - 2, 2) + "cs", "Stump.DofusProtocol.D2oClasses",
                                new[]
                                    {
                                        "System",
                                        "System.Collections.Generic"
                                    });
            }

            trigger.Reply("Generated classes.");
        }
    }

    public class GenEnumsCommand : Command
    {
        /// <summary>
        /// Output path
        /// </summary>
        [Variable]
        public static string Output = "./../../../DofusProtocol/Enums/Export/";

        public GenEnumsCommand()
            : base("genenums")
        {
            Description = "Generates enum's files";
        }

        public override void Process(CmdTrigger trigger)
        {
            IEnumerable<string> files =
                Directory.EnumerateFiles(
                    Bot.DofusSourcePath + @"\Scripts\ActionScript 3.0\com\ankamagames\dofus\network\enums\",
                    "*", SearchOption.AllDirectories);

            var enumsDict = new Dictionary<string, string[]>();

            foreach (string file in files)
            {
                string[] lines = File.ReadAllLines(file);

                string classname = lines.Where(entry => entry.Contains("class")).First().Trim().Split(' ')[2];

                IEnumerable<string> enums = from entry in lines
                                            where entry.Contains("public static const")
                                            select
                                                entry.Trim().Replace(";", "").Replace("public static const ", "").
                                                Replace(":int", "").Replace(":uint", "");

                enumsDict.Add(classname, enums.ToArray());

                using (var writer = new CsFileWriter(Output + classname + ".cs", new List<string>()))
                {
                    writer.StartNamespace("Stump.DofusProtocol.Enums");
                    writer.StartEnum(AccessModifiers.PUBLIC, classname);

                    foreach (string value in enums)
                        writer.WriteEnumElement(value);

                    writer.EndEnum();
                    writer.EndNamespace();
                }
            }

            trigger.Reply("Enums were generated sucessfully !");
        }
    }

    internal static class SharedRules
    {
        internal static Dictionary<string, string> m_replaceRules = new Dictionary<string, string>
            {
                {@"\bNetworkMessage", @"Message"},
                {@"IDataOutput", @"BigEndianWriter"},
                {@"IDataInput", @"BigEndianReader"},
                {@"\bread(?!y)", @"Read"},
                {@"\bwrite", @"Write"},
                {@"(Write|Read)Unsigned([^B])", @"$1U$2"},
            };

        internal static Dictionary<string, string> m_beforeParsingRules = new Dictionary<string, string>
            {
                {@"this\.serialize\(loc1\);", @"this.serialize(arg1);"},
                {@"writePacket\(arg1, this\.getMessageId\(\), loc1\);", @"writePacket(arg1, this.getMessageId());"},
                {@"int\(([\w_\d]+)\)", @"int.Parse($1)"}
            };


        internal static Dictionary<string, string> m_afterParsingRules = new Dictionary<string, string>
            {
                {@"WriteByte\(", @"WriteByte((byte)"},
                {@"WriteShort\(", @"WriteShort((short)"},
                {@"WriteUnsignedShort\(", @"WriteUnsignedShort((ushort)"},
                {@"WriteInt\(", @"WriteInt((int)"},
                {@"WriteUInt\(", @"WriteUInt((uint)"},
                {@"WriteFloat\(", @"WriteFloat((uint)"},
                {@"WriteUTF\(", @"WriteUTF((string)"},
                {@"(?<!(?:class\s|public\s))\bVersion\b", "Stump.DofusProtocol.Classes.Version"},
                {@"(?<!(?:class\s\s))\b(?<!Stump\.DofusProtocol\.Classes\.)Version\b([^;\n\r]+);", @"Stump.DofusProtocol.Classes.Version$1;"},
                {@"(\w+) = new ([\w_]+)\(\)\)\.deserialize\(", "(($1 = new $2()) as $2).deserialize("},
                {@"= (\w+)\.ReadUShort\(\);", @"= (ushort)$1.ReadUShort();"},
                {@"ReadUnsignedByte", @"ReadByte"},
                {@"getFlag\(", @"BooleanByteWrapper.GetFlag("},
                {@"setFlag\(([\w\d_]+), ([\w\d_]+), ([^\)]+)\)", @"$1 = BooleanByteWrapper.SetFlag($1, $2, $3)"},
                {@"public uint getTypeId \(\)", @"public virtual uint getTypeId ()"},
                {@"public void reset \(\)", @"public virtual void reset ()"},
                {
                    @"([\w_\.]+) = (?:\((?:\w+)\))?getInstance\((\w+), (\w+)\)\)\.",
                    @"(( $1 = ProtocolTypeManager.GetInstance<$2>((uint)$3)) as $2)."
                    },
                {
                    @"([\w_\.]+) = (?:\((?:\w+)\))?getInstance\((\w+), (\w+)\)",
                    @"$1 = ProtocolTypeManager.GetInstance<$2>((uint)$3)"
                    },
                {@"int base", @"int @base"},
                {@"this\.base", @"this.@base"},
                {@"this\.object", @"this.@object"},
                {@"this.breed (<|>) (Feca|Zobal)", @"this.breed $1 (int)Stump.DofusProtocol.Enums.BreedEnum.$2"},
                {@"new List<(\w+)>\((\d+)\)", "new List<$1>(new $1[$2])"},
                {@"public (\w+) operator", "public $1 @operator"},
                {@"Array ([\w\d_]+) = \[((?:(?:.+)(?:, )?)+)\];", "Array $1 = new [] { $2 };"},
                {@"flash.geom.", ""},
                {@"int\.MIN_VALUE", @"int.MinValue"},
                {
                    @"		internal const DataStoreType DST = new DataStoreType\(MODULE, true, LOCATION_LOCAL, BIND_COMPUTER\);"
                    , ""
                    },
            };

        internal static List<string> m_ignoredLines = new List<string>
            {
                "var loc1:*=new flash.utils.ByteArray();",
                "super();",
                "return;",
                "new DataStoreType(MODULE, true, LOCATION_LOCAL, BIND_COMPUTER);"
            };
    }
}