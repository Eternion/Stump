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
using Stump.Core.Reflection;

namespace Stump.Core.Xml.Config
{
    public class XmlConfig
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, Assembly> m_assemblies = new Dictionary<string, Assembly>();
        private readonly string m_configPath;

        private readonly XmlDocument m_document;
        private readonly Dictionary<string, XmlConfigNode> m_nodes = new Dictionary<string, XmlConfigNode>();
        private readonly XmlTextReader m_reader;
        private readonly XmlSchemaSet m_schema = new XmlSchemaSet();

        private string m_schemaPath;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "XmlConfig" /> class.
        /// </summary>
        /// <param name = "uriConfig">The URI config.</param>
        public XmlConfig(string uriConfig)
        {
            uriConfig = Path.GetFullPath(uriConfig);

            if (!File.Exists(uriConfig))
                throw new FileNotFoundException("Config file is not found");

            m_reader = new XmlTextReader(new MemoryStream(File.ReadAllBytes(uriConfig)));

            (m_document = new XmlDocument()).Load(m_reader);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "XmlConfig" /> class.
        /// </summary>
        /// <param name = "uriConfig">The URI config.</param>
        /// <param name = "uriSchema">The URI schema.</param>
        public XmlConfig(string uriConfig, string uriSchema)
        {
            m_configPath = uriConfig = Path.GetFullPath(uriConfig);
            m_schemaPath = uriSchema = Path.GetFullPath(uriSchema);

            if (!File.Exists(uriConfig))
                throw new FileNotFoundException("Config file is not found");
            if (!File.Exists(uriSchema))
                throw new FileNotFoundException("Schema file is not found");

            m_reader = new XmlTextReader(new MemoryStream(File.ReadAllBytes(uriConfig)));

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
        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            var elem = sender as XmlElement;

            if (e.Severity == XmlSeverityType.Error)
            {
                throw new Exception("Schema error : " + e.Message);
            }
        }

        public bool Loaded
        {
            get;
            private set;
        }

        public void Load()
        {
            if (Loaded)
                throw new Exception("Cannot call Load() twice");

            if (m_assemblies.Count <= 0)
                throw new Exception("No assemblies defined");

            LoadNodes();
            AssignValuesFromNodes(false);

            Loaded = true;
        }

        /// <summary>
        /// Reloading only change variables that have DefineableRunning to true
        /// The others values are stored to prevent any change in the config file when saving
        /// </summary>
        public void Reload()
        {
            if (!Loaded)
                throw new Exception("Call Load() before reloading");

            LoadNodes();
            AssignValuesFromNodes(true);
        }

        public void Save()
        {
            if (!Loaded)
                throw new Exception("Call Load() before saving");

            File.Copy(m_configPath, m_configPath + ".bak");

            BuildConfig();
        }

        /// <summary>
        /// Add an assembly where the XmlConfig will search variables to define
        /// </summary>
        /// <param name="assembly"></param>
        public void AddAssembly(Assembly assembly)
        {
            AddAssemblies(assembly);
        }

        /// <summary>
        /// Add assemblies where the XmlConfig will search variables to define
        /// </summary>
        public void AddAssemblies(params Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                m_assemblies.Add(assembly.GetName().Name, assembly);
            }
        }

        public void RemoveAssembly(Assembly assembly)
        {
            m_assemblies.Remove(assembly.GetName().Name);
        }

        private void LoadNodes()
        {
            m_nodes.Clear();

            foreach (XPathNavigator navigator in m_document.CreateNavigator().Select("//Variable[@name]"))
            {
                if (!navigator.IsNode)
                    continue;

                var variableNode = new XmlConfigNode(((IHasXmlNode) navigator).GetNode());

                if (string.IsNullOrEmpty(variableNode.Name))
                {
                    logger.Error(string.Format("[Config] Variable in {0} has not attribute 'name'", variableNode.Path));
                    continue;
                }

                m_nodes.Add(variableNode.Path, variableNode);
            }
        }

        private void AssignValuesFromNodes(bool reload)
        {
            // avoid multiple useless reallocations
            var assemblies = m_assemblies.Values.ToArray();
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (XmlConfigNode xmlConfigNode in m_nodes.Values)
            {
                try
                {
                    Type classType = SearchType(xmlConfigNode.Namespace + "." + xmlConfigNode.ClassName, assemblies);

                    if (classType == null)
                    {
                        logger.Error(string.Format("[Config] Cannot found the class '{0}', is the assembly loaded ?", xmlConfigNode.Namespace + "." + xmlConfigNode.ClassName));
                        continue;
                    }

                    Type elementType;
                    FieldInfo field = classType.GetField(xmlConfigNode.Name);
                    if (field != null)
                    {
                        xmlConfigNode.BindToField(field);
                        elementType = field.FieldType;
                    }
                    else
                    {
                        PropertyInfo property = classType.GetProperty(xmlConfigNode.Name);
                        if (property != null)
                        {
                            xmlConfigNode.BindToProperty(property);
                            elementType = property.PropertyType;
                        }
                        else
                        {
                            logger.Error(string.Format("[Config] Field or property '{0}' doesn't exist", xmlConfigNode.Path));
                            continue;
                        }
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(xmlConfigNode.Type))
                        {
                            Type variableType = SearchType(xmlConfigNode.Type, loadedAssemblies);

                            if (variableType == null)
                            {
                                logger.Error(string.Format("[Config] Cannot found the type '{0}' linked to the variable '{1}'", xmlConfigNode.Type, xmlConfigNode.Path));
                                continue;
                            }

                            object value = ReadElement(xmlConfigNode.Node, variableType);

                            xmlConfigNode.SetValue(value, reload);
                        }
                        else
                        {
                            object value = ReadElement(xmlConfigNode.Node.InnerXml, elementType);

                            xmlConfigNode.SetValue(value, reload);
                        }
                    }
                    catch (InvalidCastException)
                    {
                        logger.Warn(string.Format("[Config] Cannot cast {0} to the correct type : {1}", xmlConfigNode.Path, elementType));
                    }
                }

                catch (Exception e)
                {
                    logger.Warn(string.Format("[Config] Cannot define the variable {0} : {1}", xmlConfigNode.Path, e));
                }
            }
        }

        /// <summary>
        /// Search a type by his name in the given assemblies
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static Type SearchType(string typeName, Assembly[] assemblies)
        {
            Type valueType = null;
            int i = 0;
            while (valueType == null && i < assemblies.Length)
            {
                valueType = assemblies[i].GetType(typeName);

                i++;
            }

            return valueType;
        }

        private void BuildConfig()
        {
            var writer = new XmlTextWriter(m_configPath, Encoding.UTF8)
                             {Formatting = Formatting.Indented, IndentChar = '\t', Indentation = 1};

            writer.WriteStartElement("Configuration");

            var groupsByNamespace = from entry in m_nodes
                                    group entry by entry.Value.Namespace
                                    into grp
                                    orderby grp.Key
                                    select grp;

            var lastNamespace = new List<string>();

            foreach (var namespaceNodeGroup in groupsByNamespace)
            {
                var currentNamespace = namespaceNodeGroup.Key.Split('.');

                // close/open namespaces
                int count = Math.Max(lastNamespace.Count, currentNamespace.Length);
                for (int i = 0; i < count; i++)
                {
                    if (currentNamespace.Length <= i)
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
                        for (int j = i; j < currentNamespace.Length; j++)
                        {
                            writer.WriteStartElement(currentNamespace[j]);

                            lastNamespace.Add(currentNamespace[j]);
                        }
                    }
                    else if (lastNamespace[i] != currentNamespace[i])
                    {
                        int nsToClose = lastNamespace.Count - i;
                        for (int j = 0; j < nsToClose; j++)
                        {
                            writer.WriteEndElement();

                            lastNamespace.RemoveAt(lastNamespace.Count - 1);
                        }

                        for (int j = i; j < currentNamespace.Length; j++)
                        {
                            writer.WriteStartElement(currentNamespace[j]);

                            lastNamespace.Add(currentNamespace[j]);
                        }

                        break;
                    }
                }

                var groupsByClass = from entry in namespaceNodeGroup
                                    group entry by entry.Value.ClassName
                                    into grp
                                    orderby grp.Key
                                    select grp;

                foreach (var classNodeGroup in groupsByClass)
                {
                    writer.WriteStartElement(classNodeGroup.Key);

                    foreach (var node in classNodeGroup)
                    {
                        if (node.Value.BindedField == null &&
                            node.Value.BindedProperty == null)
                        {
                            logger.Error("Cannot save variable '{0}' because it has no binded field or property", node.Value.Path);
                            continue;
                        }

                        writer.WriteStartElement("Variable");
                        writer.WriteAttributeString("name", node.Value.Name);

                        var elementType = node.Value.BindedField != null
                                              ? node.Value.BindedField.FieldType
                                              : node.Value.BindedProperty.PropertyType;

                        // is primitive type
                        if (string.IsNullOrEmpty(node.Value.Type) && (elementType.HasInterface(typeof (IConvertible)) || elementType.IsEnum))
                        {
                            writer.WriteValue(node.Value.GetValue().ToString());
                        }
                        else
                        {
                            writer.WriteAttributeString("type", node.Value.Type);

                            var stringWriter = new StringWriter();
                            var xmlWriter = new XmlTextWriter(stringWriter)
                                                {
                                                    Formatting = Formatting.Indented,
                                                    IndentChar = '\t',
                                                    Indentation = 1
                                                };

                            new XmlSerializer(elementType).Serialize(xmlWriter, node.Value.GetValue());

                            var xmlReader =
                                new XmlTextReader(new StringReader(stringWriter.GetStringBuilder().ToString()));
                            XPathNavigator navigator = new XPathDocument(xmlReader).CreateNavigator();

                            writer.WriteNode(navigator, false);
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }
            }

            foreach (var ns in lastNamespace)
            {
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.Close();
        }

        private static object ReadElement(object element, Type type)
        {
            if (element is XmlNode)
            {
                return new XmlSerializer(type).Deserialize(new StringReader((element as XmlNode).InnerXml));
            }
            if (type.IsEnum)
            {
                return new XmlSerializer(type).Deserialize(new StringReader(element.ToString()));
            }

            if (element.ToString() == string.Empty)
                if (element is string)
                    return string.Empty;
                else
                    return null;

            return Convert.ChangeType(element, type, CultureInfo.InvariantCulture);
        }
    }
}