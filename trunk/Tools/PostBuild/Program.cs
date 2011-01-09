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
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.Xml.Docs;

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

                    foreach (Type type in asm.GetTypes())
                    {
                        bool classWrote = false;

                        IEnumerable<FieldInfo> fields =
                            type.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public).Where(
                                entry => entry.GetCustomAttributes(typeof(Variable), false).Count() > 0);

                        if (fields.Count() == 0)
                            continue;

                        foreach (FieldInfo fieldInfo in fields)
                        {
                            if(!asmWrote)
                            {
                                writer.WriteStartElement(asm.GetName().Name);

                                asmWrote = true;
                            }

                            if (!classWrote)
                            {
                                writer.WriteStartElement(type.Name);
                                classWrote = true;
                            }

                            DocEntry member = null;
                            if (doc != null)
                                member = doc.Members.Where(entry => entry.Name == type.FullName + "." + fieldInfo.Name).FirstOrDefault();

                            if (member != null)
                            {
                                writer.WriteComment(member.Summary);
                            }

                            string name = fieldInfo.Name;
                            object value = fieldInfo.GetValue(null);

                            writer.WriteStartElement(name);
                            
                            writer.WriteString(value is ICollection
                                                          ? (value as ICollection).ToStringCol(",")
                                                          : value.ToString());
                            writer.WriteEndElement();
                        }

                        if (classWrote)
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