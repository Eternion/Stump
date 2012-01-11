using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace CustomUplauncher.Updater
{
    public class MetaFile
    {
        public MetaFile(string filePath)
        {
            FilePath = filePath;
            HashedFiles = new List<string>();
        }

        public MetaFile(string filePath, IEnumerable<string> hashedFiles)
        {
            FilePath = filePath;
            HashedFiles = hashedFiles.ToList();
        }

        public string FilePath
        {
            get;
            set;
        }

        public List<string> HashedFiles
        {
            get;
            private set;
        }

        public void Open()
        {
            if (!File.Exists(FilePath))
                throw new Exception("File not found " + FilePath);

            var document = new XmlDocument();
            document.LoadXml(FilePath);

            var navigator = document.CreateNavigator();
            var iterator = navigator.Select("file");
            var files = new List<string>();

            while (iterator.MoveNext())
            {
                files.Add(iterator.Current.GetAttribute("name", ""));
            }

            HashedFiles = files;
        }

        public void Save()
        {
            var writer = new XmlTextWriter(FilePath, Encoding.UTF8) 
                {Indentation = 1, IndentChar = '\t'};

            writer.WriteStartDocument();

            writer.WriteStartElement("meta");
            writer.WriteStartElement("filesVersions");

            foreach (var file in HashedFiles)
            {
                var filename = Path.GetFileName(file);
                var hash = GetFileHash(file);

                writer.WriteStartElement("file");

                writer.WriteStartAttribute("name");
                writer.WriteString(filename);
                writer.WriteEndAttribute();

                writer.WriteString(hash);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
        }

        private static string GetFileHash(string file)
        {
            var content = File.ReadAllText(file);

            return GetMD5Hash(content);
        }

        private static string GetMD5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2", CultureInfo.CurrentCulture));
            }

            return sBuilder.ToString();
        }

    }
}