
using System;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Message = Stump.DofusProtocol.Messages.Message;

namespace Stump.Tools.Sniffer
{
    public static class Parser
    {
        #region Message To TreeView

        public static void ToTreeView(TreeView treeView, Message message)
        {
            TreeNode classNode = treeView.Nodes.Add(message.GetType().Name);
            classNode.Tag = "Class";

            ToTreeNode(classNode, message);
        }

        private static void ToTreeNode(TreeNode node, object obj)
        {
            foreach(var field in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (field.FieldType.GetInterface("ICollection") != null)
                {
                    TreeNode collectionNode = node.Nodes.Add(field.Name);
                    var list = field.GetValue(obj) as ICollection;
                    if (list.Count == 0)
                        collectionNode.Tag = "List";

                    foreach (object element in list)
                    {
                        if (element.GetType().IsClass && element.GetType().GetField("protocolId") != null)
                        {
                            ToTreeNode(collectionNode.Nodes.Add(field.Name.Remove(field.Name.Length - 1)), element);
                        }

                        else
                        {
                            var lNode = new TreeNode(field.Name.Remove(field.Name.Length - 1));
                            collectionNode.Nodes.Add(lNode);
                            lNode.Nodes.Add(element.ToString());
                        }
                    }
                }
                else
                {
                    if (field.FieldType.IsClass && field.FieldType.GetField("protocolId") != null)
                    {
                        ToTreeNode(node.Nodes.Add(field.Name), field.GetValue(obj));
                    }
                    else
                    {
                        var fieldNode = new TreeNode(field.Name);
                        fieldNode.Nodes.Add(new TreeNode(field.GetValue(obj).ToString()));
                        node.Nodes.Add(fieldNode);
                    }
                }
            }
        }

        #endregion

        #region TreeNodeCollection To Xml

        private static XmlWriter m_xr;

        public static void TreeNodeToXml(TreeNode tn, string filename)
        {
            var settings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineOnAttributes = true
                };

            m_xr = XmlWriter.Create(filename, settings);

            m_xr.WriteStartDocument();
            m_xr.WriteComment("Date Creation : " + DateTime.Now);
            m_xr.WriteStartElement(tn.Text);

            WriteXmlNode(tn.Nodes);

            m_xr.WriteEndElement();
            m_xr.Close();
        }

        private static void WriteXmlNode(TreeNodeCollection tnc)
        {
            foreach (TreeNode node in tnc)
            {
                if (node.Nodes.Count > 0)
                {
                    m_xr.WriteStartElement(node.Text);
                    WriteXmlNode(node.Nodes);
                    m_xr.WriteEndElement();
                }
                else
                {
                    if (node.Tag != null && node.Tag.ToString() == "List")
                    {
                        m_xr.WriteStartElement(node.Text);
                        m_xr.WriteEndElement();
                    }
                    else if (node.Tag != null && node.Tag.ToString() == "Class")
                    {
                        m_xr.WriteStartElement(node.Text);
                        m_xr.WriteEndElement();
                    }
                    else
                    {
                        m_xr.WriteString(node.Text);
                    }
                }
            }
        }

        public static void TreeNodeToXml(TreeNode tn, XmlWriter writer)
        {
            writer.WriteComment("Date : " + DateTime.Now.ToLongTimeString());
            writer.WriteStartElement(tn.Text);

            WriteXmlNode(tn.Nodes, writer);

            writer.WriteEndElement();
        }

        private static void WriteXmlNode(TreeNodeCollection tnc, XmlWriter writer)
        {
            foreach (TreeNode node in tnc)
            {
                if (node.Nodes.Count > 0)
                {
                    writer.WriteStartElement(node.Text);
                    WriteXmlNode(node.Nodes, writer);
                    writer.WriteEndElement();
                }
                else
                {
                    if (node.Tag != null && node.Tag.ToString() == "List")
                    {
                        writer.WriteStartElement(node.Text);
                        writer.WriteEndElement();
                    }
                    else if (node.Tag != null && node.Tag.ToString() == "Class")
                    {
                        writer.WriteStartElement(node.Text);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteString(node.Text);
                    }
                }
            }
        }

        #endregion

        public static TreeNode AddParentNode(TreeNode parent, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                parent.Nodes.Add(node);
            }
            return parent;
        }
    }
}