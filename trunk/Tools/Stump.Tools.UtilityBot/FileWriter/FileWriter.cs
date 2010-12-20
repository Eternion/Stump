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