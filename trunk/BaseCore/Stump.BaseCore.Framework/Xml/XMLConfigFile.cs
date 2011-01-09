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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using NLog;
using Stump.BaseCore.Framework.Attributes;

namespace Stump.BaseCore.Framework.Xml
{
    public class XmlConfigFile
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly XmlDocument m_document;
        private readonly XmlSchemaSet m_schema = new XmlSchemaSet();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "XmlConfigFile" /> class.
        /// </summary>
        /// <param name = "uriConfig">The URI config.</param>
        public XmlConfigFile(string uriConfig)
        {
            uriConfig = Path.GetFullPath(uriConfig);

            if (!File.Exists(uriConfig))
                throw new FileNotFoundException("Config file is not found");

            (m_document = new XmlDocument()).Load(uriConfig);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "XmlConfigFile" /> class.
        /// </summary>
        /// <param name = "uriConfig">The URI config.</param>
        /// <param name = "uriSchema">The URI schema.</param>
        public XmlConfigFile(string uriConfig, string uriSchema)
        {
            uriConfig = Path.GetFullPath(uriConfig);
            uriSchema = Path.GetFullPath(uriSchema);

            if (!File.Exists(uriConfig))
                throw new FileNotFoundException("Config file is not found");
            if (!File.Exists(uriSchema))
                throw new FileNotFoundException("Schema file is not found");

            (m_document = new XmlDocument()).Load(uriConfig);

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
            var stackPath = new Stack<string>();

            XmlNode firstNode = m_document.GetElementsByTagName("Configuration").Item(0);

            if (firstNode == null)
                throw new Exception("The element Configuration is not found");

            ExploreNode(firstNode, ref stackPath, currentAssembly);
        }

        /// <summary>
        ///   Defines the variables.
        /// </summary>
        public void DefinesVariables(ref Dictionary<string, Assembly> loadedAssemblies)
        {
            var stackPath = new Stack<string>();

            XmlNode firstNode = m_document.GetElementsByTagName("Configuration").Item(0);

            if (firstNode == null)
                throw new Exception("The element Configuration is not found");

            foreach (XmlNode assemblyNode in firstNode.ChildNodes)
            {
                if (!loadedAssemblies.ContainsKey(assemblyNode.Name))
                    throw new Exception("Assembly " + assemblyNode.Name + " isn't found");

                ExploreNode(assemblyNode, ref stackPath, loadedAssemblies[assemblyNode.Name]);
            }
        }

        private void ExploreNode(XmlNode node, ref Stack<string> stackPath, Assembly asm)
        {
            if (node.NodeType == XmlNodeType.Element && node.HasChildNodes)
            {
                stackPath.Push(node.Name);

                foreach (XmlNode child in node.ChildNodes)
                {
                    ExploreNode(child, ref stackPath, asm);
                }

                stackPath.Pop();
            }
            else if (node.NodeType == XmlNodeType.Text)
            {
                string name = stackPath.Peek();
                string path = string.Join(".", stackPath.Skip(1).Reverse());

                DefineVariable(name, path, node.Value, asm);
            }
        }

        private void DefineVariable(string variableName, string className, object value, Assembly asm)
        {
            Type type = asm.GetType(className);

            if (type == null)
            {
                throw new Exception("[Config] Type " + className + " doesn't exist");
            }

            FieldInfo field = type.GetField(variableName, BindingFlags.Static | BindingFlags.Public);
            PropertyInfo property = type.GetProperty(variableName, BindingFlags.Static | BindingFlags.Public);

            try
            {
                if (field != null)
                {
                    IEnumerable<Variable> attributes =
                        field.GetCustomAttributes(typeof (Variable), false).OfType<Variable>();

                    if (attributes.Count() > 0)
                    {
                        Variable attribute = attributes.First();

                        if (attribute.DefinableByConfig)
                        {
                            field.SetValue(null, ReadElement(value, field.FieldType));
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
                        property.GetCustomAttributes(typeof (Variable), false).OfType<Variable>();

                    if (attributes.Count() > 0)
                    {
                        Variable attribute = attributes.First();

                        if (attribute.DefinableByConfig)
                        {
                            property.SetValue(null, ReadElement(value, property.PropertyType), null);
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
                    logger.Warn("[Config] " + className + "." + variableName + " doesn't exist");
                }
            }
            catch (InvalidCastException)
            {
                logger.Warn(string.Format("[Config] Type of {0}.{1} isn't correct. Expected Type : {2}", className,
                                          variableName,
                                          (field != null ? field.FieldType : property.PropertyType)));
            }
            catch
            {
                logger.Warn(string.Format("[Config] Cannot define the variable {0}.{1} with value {2}", className,
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
                return (T) Convert.ChangeType(root.Value, typeof (T), CultureInfo.InvariantCulture);
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
            return XmlVariableConverters.FoundConverter(value.ToString(), type);
        }

        /// <summary>
        ///   Reads the element.
        /// </summary>
        /// <param name = "element">The element.</param>
        /// <param name = "type">The type.</param>
        /// <returns></returns>
        internal object ReadElement(XmlElement element, Type type)
        {
            return ReadElement(element.Value, type);
        }
    }
}