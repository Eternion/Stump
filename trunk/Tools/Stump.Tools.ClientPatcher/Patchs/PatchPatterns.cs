using System;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace Stump.Tools.ClientPatcher.Patchs
{
    [Serializable]
    public class PatchPatterns
    {
        private byte[] m_findPattern;
        private string m_findPatternString;
        private byte[] m_replacePattern;
        private string m_replacePatternString;

        public PatchPatterns()
        {
            
        }

        public PatchPatterns(string findPatternString, string replacePatternString, string mask)
        {
            FindPatternString = findPatternString;
            ReplacePatternString = replacePatternString;
            Mask = mask;
        }

        public string FindPatternString
        {
            get { return m_findPatternString; }
            set
            {
                m_findPatternString = value;
                m_findPattern = StringPatternToBytes(value);
            }
        }

        [XmlIgnore]
        public byte[] FindPattern
        {
            get { return m_findPattern; }
            set
            {
                m_findPattern = value;
                FindPatternString = BytesPatternToString(value);
            }
        }

        public string ReplacePatternString
        {
            get { return m_replacePatternString; }
            set
            {
                m_replacePatternString = value;
                m_replacePattern = StringPatternToBytes(value);
            }
        }

        [XmlIgnore]
        public byte[] ReplacePattern
        {
            get { return m_replacePattern; }
            set
            {
                m_replacePattern = value;
                m_replacePatternString = BytesPatternToString(value);
            }
        }

        public string Mask
        {
            get;
            set;
        }

        private static byte[] StringPatternToBytes(string pattern)
        {
            return pattern.Split(new[] {"/x"}, StringSplitOptions.RemoveEmptyEntries).
                Select(entry => byte.Parse(entry, NumberStyles.HexNumber)).ToArray();
        }

        private static string BytesPatternToString(byte[] pattern)
        {
            return pattern.Select(entry => "/x" + entry.ToString("x")).ToString();
        }
    }
}