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
using System.Linq;
using System.Xml;
using NLog;

namespace Stump.BaseCore.Framework.XmlUtils
{
    public class XmlParser
    {
        private readonly string m_xmlFileName;
        protected Logger logger = LogManager.GetCurrentClassLogger();
        protected XmlDocument m_xmlDocument;


        public XmlParser(string xmlFileName)
        {
            m_xmlFileName = xmlFileName;
            m_xmlDocument = new XmlDocument();
            try
            {
                m_xmlDocument.Load(m_xmlFileName);
            }
            catch (Exception e)
            {
                logger.Error("Exception occurred while parsing XML : {0} {1}", m_xmlFileName, e.Message);
            }
        }

        /// <summary>
        ///   Read the data within the given node.
        /// </summary>
        protected string Read(string key)
        {
            XmlNodeList xData = null;
            try
            {
                xData = m_xmlDocument.GetElementsByTagName(key);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Exception raised when reading data at node : {0}", key), ex);
            }

            return xData[0].InnerText;
        }

        public List<T> ReadList<T>(string parentkey, string childkey)
        {
            XmlNodeList nodes = m_xmlDocument.SelectNodes("//" + parentkey);

            return (from XmlNode node in nodes
                    let event1 = node["event"].InnerText
                    from XmlNode n in node
                    select n[childkey].InnerText
                    into obj select (T) Convert.ChangeType(obj, typeof (T))).ToList();
        }

        /// <summary>
        ///   Read the data as a boolean.
        /// </summary>
        public bool ReadBoolean(string key)
        {
            return bool.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a byte
        /// </summary>
        public byte ReadByte(string key)
        {
            return byte.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a char
        /// </summary>
        public char ReadChar(string key)
        {
            return char.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a decimal
        /// </summary>
        public decimal ReadDecimal(string key)
        {
            return decimal.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a double
        /// </summary>
        public double ReadDouble(string key)
        {
            return double.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a short
        /// </summary>
        public short ReadInt16(string key)
        {
            return short.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a int
        /// </summary>
        public int ReadInt32(string key)
        {
            return int.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a long
        /// </summary>
        public long ReadInt64(string key)
        {
            return long.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a sbyte
        /// </summary>
        public sbyte ReadSByte(string key)
        {
            return sbyte.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a single
        /// </summary>
        public Single ReadSingle(string key)
        {
            return Single.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a string
        /// </summary>
        public string ReadString(string key)
        {
            return Read(key);
        }

        /// <summary>
        ///   Read the data as a ushort
        /// </summary>
        public ushort ReadUInt16(string key)
        {
            return ushort.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a uint
        /// </summary>
        public uint ReadUInt32(string key)
        {
            return uint.Parse(Read(key));
        }

        /// <summary>
        ///   Read the data as a ulong
        /// </summary>
        public ulong ReadUInt64(string key)
        {
            return ulong.Parse(Read(key));
        }
    }
}