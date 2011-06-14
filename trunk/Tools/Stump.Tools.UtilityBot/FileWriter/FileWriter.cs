
using System;
using System.IO;
using System.Text;

namespace Stump.Tools.UtilityBot.FileWriter
{
    public enum ControlSequenceType
    {
        IF,
        ELSE,
        ELSEIF,
        WHILE,
        BREAK,
        RETURN
    } ;

    public class FileWriter : IDisposable
    {
        protected StringBuilder m_indentation = new StringBuilder();
        protected TextWriter m_writer;

        public FileWriter(string outputPath)
        {
            m_writer = new StreamWriter(File.Open(outputPath, FileMode.Create));
            m_indentation = new StringBuilder("");
        }

        public FileWriter(Stream stream)
        {
            m_writer = new StreamWriter(stream);
            m_indentation = new StringBuilder("");
        }

        //releases stream in case one is open
        ~FileWriter()
        {
            if (m_writer != null)
            {
                m_writer.Close();
                m_writer = null;
            }
        }

        public void Dispose()
        {
            if (m_writer != null)
            {
                m_writer.Close();
                m_writer = null;
            }
        }

        protected void IncreaseIntendation()
        {
            m_indentation.Append("\t");
        }

        protected void DecreaseIntendation()
        {
            m_indentation.Remove(m_indentation.Length - 1, 1);
        }

        public void WriteLineWithIndent(string str)
        {
            m_writer.WriteLine(m_indentation + str);
        }

        public void WriteLineWithIndent()
        {
            m_writer.WriteLine(m_indentation.ToString());
        }

        public void WriteWithIndent(string str)
        {
            m_writer.Write(m_indentation + str);
        }

        public void WriteWithIndent()
        {
            m_writer.Write(m_indentation.ToString());
        }
    }
}