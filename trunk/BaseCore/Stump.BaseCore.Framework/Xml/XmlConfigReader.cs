﻿// /*************************************************************************
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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using NLog;
using Stump.BaseCore.Framework.Attributes;

namespace Stump.BaseCore.Framework.Xml
{
    public class XmlConfigReader
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly XmlDocument m_document;
        private readonly XmlTextReader m_reader;
        private readonly XmlSchemaSet m_schema = new XmlSchemaSet();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "XmlConfigReader" /> class.
        /// </summary>
        /// <param name = "uriConfig">The URI config.</param>
        public XmlConfigReader(string uriConfig)
        {
            uriConfig = Path.GetFullPath(uriConfig);

            m_reader = new XmlTextReader(new MemoryStream(File.ReadAllBytes(uriConfig)));

            if (!File.Exists(uriConfig))
                throw new FileNotFoundException("Config file is not found");

            (m_document = new XmlDocument()).Load(m_reader);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "XmlConfigReader" /> class.
        /// </summary>
        /// <param name = "uriConfig">The URI config.</param>
        /// <param name = "uriSchema">The URI schema.</param>
        public XmlConfigReader(string uriConfig, string uriSchema)
        {
            uriConfig = Path.GetFullPath(uriConfig);
            uriSchema = Path.GetFullPath(uriSchema);

            m_reader = new XmlTextReader(new MemoryStream(File.ReadAllBytes(uriConfig)));

            if (!File.Exists(uriConfig))
                throw new FileNotFoundException("Config file is not found");
            if (!File.Exists(uriSchema))
                throw new FileNotFoundException("Schema file is not found");

            (m_document = new XmlDocument()).Load(m_reader);

            using (var reader = new StreamReader(uriSchema))
            {
                m_schema.Add(XmlSchema.Read(reader, ValidationEventHandler));
            }

            m_document.Schemas = m_schema;
            m_document.Validate(ValidationEventHandler);
        }

        /// <summary>
        ///   Validation event handler.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Xml.Schema.ValidationEventArgs" /> instance containing the event data.</param>
        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            var elem = sender as XmlElement;

            if (e.Severity == XmlSeverityType.Error)
            {
                if (elem != null)
                {
                    logger.Warn("Schema error : " + e.Message);
                    Console.WriteLine("Enter a value for {0} :", elem.Name);
                    elem.Value = Console.ReadLine();

                    m_document.Validate(ValidationEventHandler);
                }
                else
                {
                    throw new Exception("Schema error : " + e.Message);
                }
            }
        }

        public void DefinesVariables(Assembly currentAssembly)
        {
            if (m_document.DocumentElement.Name != "Configuration")
                throw new Exception("The element Configuration is not found");

            XmlNode asmNode = m_document.DocumentElement.FirstChild;
            while (asmNode != null)
            {
                if (currentAssembly.GetName().Name != asmNode.Name)
                {
                    logger.Error("[Config] Assembly " + asmNode.Name + " isn't found");
                    continue;
                }

                foreach (XPathNavigator navigator in asmNode.CreateNavigator().SelectDescendants("Variable", "", false))
                {
                    if (!navigator.IsNode)
                        continue;

                    XmlNode variable = (navigator as IHasXmlNode).GetNode();

                    string name = variable.Attributes["name"] != null ? variable.Attributes["name"].Value : "";
                    string type = variable.Attributes["type"] != null ? variable.Attributes["type"].Value : "";
                    string @namespace = GetNamespaceOfNode(variable);

                    if (name == string.Empty)
                        throw new Exception(string.Format("[Config] Variable in {0} has not attribute 'name'",
                                                          @namespace));

                    if (type != string.Empty)
                    {
                        Type valueType = Type.GetType(type);

                        if (valueType == null)
                        {
                            valueType = currentAssembly.GetType(type);
                        }

                        DefineVariable(name, @namespace, variable, valueType, currentAssembly);

                    }
                    else
                        DefineVariable(name, @namespace, variable.InnerText, null, currentAssembly);
                }

                asmNode = asmNode.NextSibling;
            }
        }

        /// <summary>
        ///   Defines the variables.
        /// </summary>
        public void DefinesVariables(ref Dictionary<string, Assembly> loadedAssemblies)
        {
            if (m_document.DocumentElement.Name != "Configuration")
                throw new Exception("The element Configuration is not found");

            XmlNode asmNode = m_document.DocumentElement.FirstChild;
            while (asmNode != null)
            {
                if (!loadedAssemblies.ContainsKey(asmNode.Name))
                    throw new Exception("Assembly " + asmNode.Name + " isn't found");

                foreach (XPathNavigator navigator in asmNode.CreateNavigator().SelectDescendants("Variable", "", false))
                {
                    if (!navigator.IsNode)
                        continue;

                    XmlNode variable = (navigator as IHasXmlNode).GetNode();

                    string name = variable.Attributes["name"] != null ? variable.Attributes["name"].Value : "";
                    string type = variable.Attributes["type"] != null ? variable.Attributes["type"].Value : "";
                    string @namespace = GetNamespaceOfNode(variable);

                    if (name == string.Empty)
                        throw new Exception(string.Format("[Config] Variable in {0} has not attribute 'name'",
                                                          @namespace));

                    if (type != string.Empty)
                    {
                        Type valueType = Type.GetType(type);
                        int i = 0;
                        while (valueType == null && i < loadedAssemblies.Count)
                        {
                            valueType = loadedAssemblies.Values.ElementAt(i).GetType(type);

                            i++;
                        }

                        DefineVariable(name, @namespace, variable, valueType, loadedAssemblies[asmNode.Name]);

                    }
                    else
                        DefineVariable(name, @namespace, variable.InnerText, null, loadedAssemblies[asmNode.Name]);
                }

                asmNode = asmNode.NextSibling;
            }
        }

        private string GetNamespaceOfNode(XmlNode node)
        {
            var stringBuilder = new StringBuilder();

            var currentNode = node;
            while (currentNode.ParentNode != null && currentNode.ParentNode != m_document.DocumentElement)
            {
                stringBuilder.Insert(0, currentNode.ParentNode.Name + ".");

                currentNode = currentNode.ParentNode;
            }

            return stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString();
        }

        private void DefineVariable(string variableName, string @namespace, object value, Type valueType, Assembly asm)
        {
            Type type = asm.GetType(@namespace);

            if (type == null)
            {
                throw new Exception("[Config] Type " + @namespace + " doesn't exist");
            }

            FieldInfo field = type.GetField(variableName, BindingFlags.Static | BindingFlags.Public);
            PropertyInfo property = type.GetProperty(variableName,
                                                     BindingFlags.Static | BindingFlags.Public);

            try
            {
                if (field != null)
                {
                    IEnumerable<Variable> attributes =
                        field.GetCustomAttributes(typeof(Variable), false).OfType<Variable>();

                    if (attributes.Count() > 0)
                    {
                        Variable attribute = attributes.First();

                        if (attribute.DefinableByConfig)
                        {
                            field.SetValue(null, ReadElement(value, valueType ?? field.FieldType));
                        }
                    }
                    else
                    {
                        logger.Warn("[Config] Field " + field.Name + " must have Variable attribute to be modifiable");
                    }
                }
                else if (property != null)
                {
                    IEnumerable<Variable> attributes =
                        property.GetCustomAttributes(typeof(Variable), false).OfType<Variable>();

                    if (attributes.Count() > 0)
                    {
                        Variable attribute = attributes.First();

                        if (attribute.DefinableByConfig)
                        {
                            property.SetValue(null, ReadElement(value, valueType ?? property.PropertyType), null);
                        }
                    }
                    else
                    {
                        logger.Warn("[Config] Property " + property.Name +
                                    " must have Variable attribute to be modifiable");
                    }
                }
                else
                {
                    logger.Warn("[Config] " + @namespace + "." + variableName + " doesn't exist");
                }
            }
            catch (InvalidCastException)
            {
                logger.Warn(string.Format("[Config] Type of {0}.{1} isn't correct. Expected Type : {2}", @namespace,
                                          variableName,
                                          (field != null ? field.FieldType : property.PropertyType)));
            }
            catch
            {
                logger.Warn(string.Format("[Config] Cannot define the variable {0}.{1} with value {2}", @namespace,
                                          variableName,
                                          value.ToString()));
            }
        }


        /// <summary>
        ///   Reads the specified nodes.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "nodes">The nodes.</param>
        /// <returns></returns>
        public T Read<T>(params string[] nodes)
        {
            XmlElement root = nodes.Aggregate(m_document.DocumentElement,
                                              (current, node) =>
                                              current.GetElementsByTagName(node).Item(0) as XmlElement);

            try
            {
                return (T)Convert.ChangeType(root.Value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }

        /// <summary>
        ///   Reads the element.
        /// </summary>
        /// <returns></returns>
        internal object ReadElement(object value, Type type)
        {
            if (value is XmlNode)
            {
                return new XmlSerializer(type).Deserialize(new StringReader((value as XmlNode).InnerXml));
            }
            if (type.IsEnum)
            {
                return new XmlSerializer(type).Deserialize(new StringReader(value.ToString()));
            }

            if (value.ToString() == string.Empty)
                if (value is string)
                    return string.Empty;
                else
                    return null;

            return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }
    }
}