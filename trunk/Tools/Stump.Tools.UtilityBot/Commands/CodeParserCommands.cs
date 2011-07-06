
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Squishy.Irc.Commands;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
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
                    Bot.DofusSourcePath + @"\Scripts\ActionScript 3.0\package com\package ankamagames\package dofus\package network\package messages\",
                    "*", SearchOption.AllDirectories);

            string @namespace = trigger.Args.NextWord();

            foreach (string file in files.Where((entry) => char.IsUpper(Path.GetFileName(entry).Replace("class ", "")[0])))
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
                splitPath = splitPath.SkipWhile(entry => entry != "package messages").Skip(1).Select(entry => entry.Replace("package ", "").Replace("class ", "")).ToArray();

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

                parser.ToCSharp(path.Remove(path.Length - 2, 2) + "cs", @namespace,
                                new[]
                                    {
                                        "System",
                                        "System.Collections.Generic",
                                        "BigEndianStream",
                                        "DofusProtocol.Messages",
                                        "DofusProtocol.Classes.Types"
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
                    Bot.DofusSourcePath + @"\Scripts\ActionScript 3.0\package com\package ankamagames\package dofus\package network\package types\",
                    "*", SearchOption.AllDirectories);

            foreach (string file in files.Where((entry) => char.IsUpper(Path.GetFileName(entry).Replace("class ", "")[0])))
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
                splitPath = splitPath.SkipWhile(entry => entry != "package types").Skip(1).Select(entry => entry.Replace("package ", "").Replace("class ", "")).ToArray();

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

                parser.ToCSharp(path.Remove(path.Length - 2, 2) + "cs", trigger.Args.NextWord(),
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
                        parser.Fields.Where(
                            entry => entry.Name.StartsWith("_") && entry.Modifiers == AccessModifiers.PROTECTED))
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

    public class GenRecordsCommand : Command
    {
        /// <summary>
        /// Output path
        /// </summary>
        [Variable]
        public static string Output = "./records/";

        private static string[] GenerationClassPattern = new[]
                                                             {
                                                                 "using System;",
                                                                 "using Castle.ActiveRecord;",
                                                                 "using Stump.Database.Types;",
                                                                 "using Stump.DofusProtocol.D2oClasses.Tool;",
                                                                 "",
                                                                 "namespace ##RECORD_NAMESPACE##",
                                                                 "{",
                                                                 "    [Serializable]",
                                                                 "    [ActiveRecord(\"##TABLE_NAME##\")]",
                                                                 "    [AttributeAssociatedFile(##RECORD_FILE##)]",
                                                                 "    [D2OClass(\"##RECORD_CLASS##\", \"##CLASS_PACKAGE##\")]"
                                                                 ,
                                                                 "    public sealed class ##RECORD_NAME## : DataBaseRecord<##RECORD_NAME##>"
                                                                 ,
                                                                 "    {",
                                                                 "##FIELDS##",
                                                                 "    }",
                                                                 "}",
                                                             };

        private static readonly string[] GenerationFieldPattern = new[]
                                                                      {
                                                                          "",
                                                                          "       [D2OField(\"##D2O_FIELD_NAME##\")]",
                                                                          "       ##RECORD_ATTRIBUTE##",
                                                                          "       public ##FIELD_TYPE## ##FIELD_NAME##",
                                                                          "       {",
                                                                          "           get;",
                                                                          "           set;",
                                                                          "       }"
                                                                      };

        public GenRecordsCommand()
            : base("genrecords")
        {
            Description = "Generates records's files";
        }

        public override void Process(CmdTrigger trigger)
        {
            trigger.Reply("Generating records's classes. Please wait.");

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
                    parser.ParseFile(true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    continue;
                }

                string[] splitPath = file.Split('\\');
                splitPath = splitPath.SkipWhile(entry => entry != "datacenter").Skip(1).Select(entry => entry.FirstLetterUpper()).ToArray();

                string dirPath = Path.GetFullPath(Output) + "/";
                foreach (string dir in splitPath)
                {
                    if (dirPath.Contains(".as"))
                        break;

                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    dirPath += dir + "/";
                }

                string path = ( Output + string.Join("/", splitPath) );

                File.WriteAllText(path.Remove(path.Length - 3, 3) + "Record" + ".cs", GenerateRecord(parser));
            }

            trigger.Reply("Generated classes.");
        }

        private string GenerateRecord(AsParser parser)
        {
            var fields = new StringBuilder();

            bool primaryKeyDefined = false;
            bool hasNativeId = parser.Fields.Count(entry => entry.Name.ToLower() == "id") > 0;

            foreach (FieldInfo field in parser.Fields)
            {
                if (field.Modifiers != AccessModifiers.PUBLIC)
                    continue;
                
                bool isPrimaryKey = !primaryKeyDefined &&
                                    (hasNativeId ? field.Name.ToLower() == "id" : field.Name.ToLower().Contains("id"));
                if (isPrimaryKey)
                    primaryKeyDefined = true;

                string propertyName = isPrimaryKey ? "Id" : field.Name.Replace("_", "").FirstLetterUpper();
                string propertyParam = string.Empty;

                if (field.Type.Contains("Rectangle") || field.Type.Contains("List") || field.Type.Contains("Point"))
                    propertyParam = ", ColumnType=\"Serializable\"";

                var fieldsParams = new Dictionary<string, string>
                                       {
                                           {"##D2O_FIELD_NAME##", field.Name},
                                           {
                                               "##RECORD_ATTRIBUTE##",
                                               isPrimaryKey
                                                   ? "[PrimaryKey(PrimaryKeyType.Assigned, \"Id\")]"
                                                   : "[Property(\"" + propertyName  + "\"" + propertyParam + ")]"
                                               },
                                           {"##FIELD_TYPE##", field.Type},
                                           {"##FIELD_NAME##", propertyName}
                                       };

                fields.AppendLine(GenerateByPattern(GenerationFieldPattern, fieldsParams));
            }

            string @namespace = "Stump.Database.Data." +
                                string.Join(".",
                                            parser.Class.Namespace.Split('.').Skip(4).Select(
                                                entry => entry.FirstLetterUpper()));
            FieldInfo modulefield = parser.Fields.Where(entry => entry.Name == "MODULE").FirstOrDefault();
            string className = (parser.Class.Name.EndsWith("s")
                                    ? parser.Class.Name.Remove(parser.Class.Name.Length - 1, 1)
                                    : parser.Class.Name) + "Record";

            var classParams = new Dictionary<string, string>
                                  {
                                      {"##RECORD_NAMESPACE##", @namespace},
                                      {"##TABLE_NAME##", GenerateTableName(parser.Class.Name)},
                                      {"##RECORD_FILE##", modulefield != null ? modulefield.Value : string.Empty},
                                      {"##RECORD_CLASS##", parser.Class.Name},
                                      {"##CLASS_PACKAGE##", parser.Class.Namespace},
                                      {"##RECORD_NAME##", className},
                                      {"##FIELDS##", fields.ToString()}
                                  };

            return GenerateByPattern(GenerationClassPattern, classParams);
        }

        private static string GenerateTableName(string className)
        {
            List<char> chars = className.ToList();

            for (int i = 1; i < chars.Count; i++)
            {
                if (char.IsUpper(chars[i]))
                {
                    chars.Insert(i, '_');
                    i++;
                }
            }

            return new string(chars.ToArray()).ToLower();
        }

        private static string GenerateByPattern(string[] pattern, IEnumerable<KeyValuePair<string, string>> args)
        {
            string result = string.Join("\r\n", pattern);

            foreach (var arg in args)
            {
                result = result.Replace(arg.Key, arg.Value);
            }

            return result;
        }
    }

    internal static class SharedRules
    {
        internal static Dictionary<string, string> m_replaceRules =
            new Dictionary<string, string>
                {
                    {@"\bNetworkMessage", @"Message"},
                    {@"\bIDataOutput", @"BigEndianWriter"},
                    {@"\bIDataInput", @"BigEndianReader"},
                    {@"\bread(?!y)", @"Read"},
                    {@"\bwrite", @"Write"},
                    {@"(Write|Read)Unsigned([^B])", @"$1U$2"},
                };

        internal static Dictionary<string, string> m_beforeParsingRules =
            new Dictionary<string, string>
                {
                    {@"this\.serialize\(loc1\);", @"this.serialize(arg1);"},
                    {@"writePacket\(arg1, this\.getMessageId\(\), loc1\);", @"writePacket(arg1, this.getMessageId());"},
                    {@"int\(([\w_\d]+)\)", @"int.Parse($1)"},
                    {@"flash\.geom\.", ""},
                    {@"Vector.([\w_\d]+) = new ([\w_\d]+)();", "$1 = new List<$2>();"},
                };


        internal static Dictionary<string, string> m_afterParsingRules =
            new Dictionary<string, string>
                {
                    {@"WriteByte\(", @"WriteByte((byte)"},
                    {@"WriteShort\(", @"WriteShort((short)"},
                    {@"WriteUnsignedShort\(", @"WriteUnsignedShort((ushort)"},
                    {@"WriteInt\(", @"WriteInt((int)"},
                    {@"WriteUInt\(", @"WriteUInt((uint)"},
                    {@"WriteFloat\(", @"WriteFloat((uint)"},
                    {@"WriteUTF\(", @"WriteUTF((string)"},
                    {@"(?<!(?:class\s|public\s))\bVersion\b", "Classes.Types.Version"},
                    {
                        @"(?<!(?:class\s\s))\b(?<!Classes\.Types\.)Version\b([^;\n\r]+);",
                        @"Classes.Types.Version$1;"
                        },
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
                    {@"int\.MIN_VALUE", @"int.MinValue"},
                    {
                        @"		internal const DataStoreType DST = new DataStoreType\(MODULE, true, LOCATION_LOCAL, BIND_COMPUTER\);"
                        ,
                        ""
                        },
                };

        internal static List<string> m_ignoredLines =
            new List<string>
                {
                    "var loc1:*=new flash.utils.ByteArray();",
                    "loc1 = new flash.utils.ByteArray();",
                    "var loc1:*;",
                    "super();",
                    "Vector.servers = new int();",
                    "return;",
                    "new DataStoreType(MODULE, true, LOCATION_LOCAL, BIND_COMPUTER);"
                };
    }
}