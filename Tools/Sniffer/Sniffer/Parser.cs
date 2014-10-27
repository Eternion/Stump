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
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Message = Stump.DofusProtocol.Messages.Message;

namespace Stump.Tools.Sniffer
{
    public static class Parser
    {
        #region Message To TreeView

        public static void ToTreeView(TreeView treeView, Message message)
        {
            TreeNode classNode = treeView.Nodes.Add(message.ToString()/*GetType().Name*/);
            classNode.Tag = "Class";

            ToTreeNode(classNode, message);
           /* if (message.Error != null && !String.IsNullOrEmpty(message.Error.ToString())) // In case of error, add error description in the tree
            {
                var fieldNode = new TreeNode("_Error_");
                fieldNode.Nodes.Add(new TreeNode(message.Error.ToString()));
                classNode.Nodes.Add(fieldNode);
            }*/
        }

        private static void ToTreeNode(TreeNode node, object obj)
        {
            foreach (var field in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (field.FieldType == typeof (byte[]))
                {
                    var bytes = field.GetValue(obj) as byte[];
                    var fieldNode = new TreeNode(field.Name);
                    fieldNode.Nodes.Add(new TreeNode(bytes != null ? string.Join(" ", bytes.Select(x => x.ToString("X2"))) : "null"));
                    node.Nodes.Add(fieldNode);
                }
                else if (field.FieldType == typeof (IEnumerable<sbyte>))
                {
                    var bytes = field.GetValue(obj) as IEnumerable<sbyte>;
                    var fieldNode = new TreeNode(field.Name);
                    fieldNode.Nodes.Add(new TreeNode(bytes != null ? string.Join(" ", bytes.Select(x => x.ToString("X2"))) : "null"));
                    node.Nodes.Add(fieldNode);
                }
                else if (field.FieldType.GetInterface("IEnumerable") != null && field.FieldType != typeof (string))
                {
                    TreeNode collectionNode = node.Nodes.Add(field.Name);
                    var list = field.GetValue(obj) as IEnumerable;

                    int count = 0;
                    foreach (object element in list)
                    {
                        if (element.GetType().IsClass && element.GetType().GetProperty("TypeId") != null)
                        {
                            ToTreeNode(collectionNode.Nodes.Add(field.FieldType.GetGenericArguments()[0].Name), element);
                        }

                        else
                        {
                            var lNode = new TreeNode(field.Name.Remove(field.Name.Length - 1));
                            collectionNode.Nodes.Add(lNode);
                            lNode.Nodes.Add(element.ToString());
                        }

                        count++;
                    }

                    if (count == 0)
                        collectionNode.Tag = "list";
                }
                else
                {
                    if (field.FieldType.IsClass && field.FieldType.GetProperty("TypeId") != null)
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