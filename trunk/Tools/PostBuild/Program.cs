
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Stump.Core.Attributes;
using Stump.Core.Xml.Docs;

namespace PostBuild
{
    internal class Program
    {
        /// <summary>
        ///   Stuff to be executed after Stump has been built
        /// </summary>
        /// <param name = "args"></param>
        private static void Main(string[] args)
        {
            // TODO : Docs commands & msdn'like
            // TODO : generate config file

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Console.WriteLine("Exception raised while generating config : " + e.ExceptionObject);
            };

            if (args[0] == "-config")
            {
                string path = args[1].Replace("\"", "");

                string output = Path.Combine(path, "default_config.xml");
                var writer = new XmlTextWriter(output, Encoding.UTF8)
                             {Formatting = Formatting.Indented, IndentChar = '\t', Indentation = 1};

                writer.WriteStartElement("Configuration");

                foreach (string file in Directory.EnumerateFiles(path, "Stump*.dll"))
                {
                    bool asmWrote = false;
                    Assembly asm;
                    try
                    {
                        if (Path.GetFileNameWithoutExtension(file) != "Stump.BaseCore.Framework")
                            asm = Assembly.LoadFrom(file);
                        else
                            continue;
                    }
                    catch
                    {
                        continue;
                    }

                    DotNetDocumentation doc = null;
                    string docPath = Path.Combine(path, Path.GetFileNameWithoutExtension(file) + ".xml");

                    if (File.Exists(docPath))
                        doc = DotNetDocumentation.Load(docPath);

                    var lastNamespace = new List<string>();
                    foreach (var group in asm.GetTypes().GroupBy(entry => entry.Namespace))
                    {
                        foreach (var type in group)
                        {
                            bool classWrote = false;

                            var memberInfos =
                                type.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public).Where(
                                    entry => entry.GetCustomAttributes(typeof (Variable), false).Count() > 0).Concat(
                                        (IEnumerable<MemberInfo>)
                                        type.GetProperties(BindingFlags.Static | BindingFlags.GetProperty |
                                                           BindingFlags.Public).Where(
                                                               entry =>
                                                               entry.GetCustomAttributes(typeof (Variable), false).Count
                                                                   () > 0));

                            if (memberInfos.Count() == 0)
                                continue;

                            foreach (MemberInfo memberInfo in memberInfos)
                            {
                                if (!asmWrote)
                                {
                                    writer.WriteStartElement(asm.GetName().Name);

                                    asmWrote = true;
                                }

                                List<string> currentNamespace =
                                    type.Namespace.Replace(asm.GetName().Name, "").Split(new[] {'.'},
                                                                                         StringSplitOptions.
                                                                                             RemoveEmptyEntries).ToList();

                                int count = Math.Max(lastNamespace.Count, currentNamespace.Count);
                                for (int i = 0; i < count; i++)
                                {
                                    if (currentNamespace.Count <= i)
                                    {
                                        int nsToClose = lastNamespace.Count - i;
                                        for (int j = 0; j < nsToClose; j++)
                                        {
                                            writer.WriteEndElement();

                                            lastNamespace.RemoveAt(lastNamespace.Count - 1);
                                        }
                                    }
                                    else if (lastNamespace.Count <= i)
                                    {
                                        for (int j = i; j < currentNamespace.Count; j++)
                                        {
                                            writer.WriteStartElement(currentNamespace[i]);

                                            lastNamespace.Add(currentNamespace[i]);
                                        }
                                    }
                                    else if (lastNamespace[i] != currentNamespace[i])
                                    {
                                        int nsToClose = count - i;
                                        for (int j = 0; j < nsToClose; j++)
                                        {
                                            writer.WriteEndElement();

                                            lastNamespace.RemoveAt(lastNamespace.Count - 1);
                                        }

                                        for (int j = i; j < currentNamespace.Count; j++)
                                        {
                                            writer.WriteStartElement(currentNamespace[i]);

                                            lastNamespace.Add(currentNamespace[i]);
                                        }
                                    }
                                }


                                if (!classWrote)
                                {
                                    writer.WriteStartElement(type.Name);
                                    classWrote = true;
                                }


                                DocEntry member = null;
                                if (doc != null)
                                    member =
                                        doc.Members.Where(entry => entry.Name == type.FullName + "." + memberInfo.Name).
                                            FirstOrDefault();

                                if (member != null)
                                {
                                    writer.WriteComment(member.Summary);
                                }

                                string name = memberInfo.Name;
                                object value = memberInfo is FieldInfo
                                                   ? (memberInfo as FieldInfo).GetValue(null)
                                                   : (memberInfo as PropertyInfo).GetValue(null, null);
                                var memberType = memberInfo is FieldInfo
                                                     ? (memberInfo as FieldInfo).FieldType
                                                     : (memberInfo as PropertyInfo).PropertyType;

                                writer.WriteStartElement("Variable");
                                writer.WriteAttributeString("name", name);

                                if (memberType.GetInterfaces().Contains(typeof(IConvertible)) && !memberType.IsEnum)
                                {
                                    if (value != null)
                                        writer.WriteString(value.ToString());
                                }
                                else
                                {
                                    writer.WriteAttributeString("type", memberType.Name);

                                    var stringWriter = new StringWriter();
                                    var xmlWriter = new XmlTextWriter(stringWriter)
                                                        {
                                                            Formatting = Formatting.Indented,
                                                            IndentChar = '\t',
                                                            Indentation = 1
                                                        };

                                    new XmlSerializer(memberType).Serialize(xmlWriter, value);

                                    var xmlReader =
                                        new XmlTextReader(new StringReader(stringWriter.GetStringBuilder().ToString()));
                                    XPathNavigator navigator = new XPathDocument(xmlReader).CreateNavigator();

                                    writer.WriteNode(navigator, false);
                                }

                                writer.WriteEndElement();
                            }

                            if (classWrote)
                                writer.WriteEndElement();
                        }
                    }

                    foreach (string ns in lastNamespace)
                    {
                        writer.WriteEndElement();
                    }

                    if (asmWrote)
                        writer.WriteEndElement();

                }

                writer.WriteEndElement();
                writer.Close();

                Console.WriteLine("Default config file generetad : {0}", output);
            }
        }
    }
}