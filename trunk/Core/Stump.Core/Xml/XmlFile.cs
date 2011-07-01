using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Stump.Core.Xml
{
	/// <remarks>Exported from WCell project (www.wcell.org)</remarks>
    [Serializable]
	public class XmlFile<T> : XmlFileBase
		where T : XmlFileBase
	{
		protected XmlFile()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="fileName">The name of the configuration file.</param>
		public XmlFile(string fileName)
		{
			m_filename = fileName;
		}

		public XmlFile(XmlFileBase parentFile)
		{
			MParentFile = parentFile;
		}

		/// <summary>
		/// Returns whether or not the file exists
		/// </summary>
		public virtual bool FileExists(string path)
		{
			return File.Exists((String.IsNullOrEmpty(path) ? "" : path + "\\") + m_filename);
		}

		/// <summary>
		/// Writes the configuration file to disk.
		/// </summary>
		public override void Save()
		{
			if (MParentFile != null)
			{
				MParentFile.Save();
			}
			else
			{
				var ser = new XmlSerializer(GetType());
				var path = Path.GetDirectoryName(m_filename);

				if (path.Length > 0 && !Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				//have to use TextWriter so the resulting XML file's format isn't all jacked up
				//I dno wtf XmlWriter's problem is but it sux when it comes to proper formatting of whitespace
				//and indentation
				using (TextWriter writer = new StreamWriter(m_filename, false, Encoding.UTF8))
				{
					ser.Serialize(writer, this);
				}
			}
		}

		/// <summary>
		/// Writes the configuration file to disk with the specified name.
		/// </summary>
		/// <param name="fileName">The name of the file on disk to write to.</param>
		public override void SaveAs(string fileName)
		{
			m_filename = fileName;
			Save();
		}

		/// <summary>
		/// Writes the configuration file to disk with the specified name.
		/// </summary>
		/// <param name="fileName">The name of the file on disk to write to.</param>
		/// <param name="location">The directory to write the file to.</param>
		public virtual void SaveAs(string fileName, string location)
		{
			if (String.IsNullOrEmpty(location))
			{
				throw new ArgumentException("Location cannot be be null or empty!", "location");
			}

			m_filename = fileName;

			var ser = new XmlSerializer(GetType());

			if (!Directory.Exists(location))
			{
				Directory.CreateDirectory(location);
			}
			if (location[location.Length - 1] != Path.DirectorySeparatorChar)
			{
				location += Path.DirectorySeparatorChar;
			}

			location += m_filename;

			using (TextWriter writer = new StreamWriter(location, false, Encoding.UTF8))
			{
				ser.Serialize(writer, this);
				writer.Close();
			}
		}

		protected override void OnLoad()
		{
		}

		/// <summary>
		/// Returns the serialized XML of this XmlConfig for further processing, etc.
		/// </summary>
		public override string ToString()
		{
			return FileName;
		}

		public static T Load(string filename)
		{
			T cfg;
			var ser = new XmlSerializer(typeof(T));
			using (var rdr = XmlReader.Create(filename, new XmlReaderSettings()))
			{
				cfg = (T)ser.Deserialize(rdr);
			}
			cfg.FileName = filename;
			(((XmlFile<T>)(XmlFileBase)cfg)).OnLoad();

			return cfg;
		}

		public static ICollection<T> LoadAll(string dir)
		{
			var list = new List<T>();
			LoadAll(dir, list);

			return list;
		}

		public static ICollection<T> LoadAll(DirectoryInfo dir)
		{
			var list = new List<T>();
			LoadAll(dir, list);

			return list;
		}

		public static void LoadAll(string dir, ICollection<T> cfgs)
		{
			LoadAll(new DirectoryInfo(dir), cfgs);
		}

		public static void LoadAll(DirectoryInfo dir, ICollection<T> cfgs)
		{
			foreach (var file in dir.GetFileSystemInfos())
			{
				if (file is DirectoryInfo)
				{
					LoadAll((DirectoryInfo)file, cfgs);
				}
				else
				{
					if (file.Extension.EndsWith("xml", StringComparison.InvariantCultureIgnoreCase))
					{
						try
						{
							var def = Load(file.FullName);
							cfgs.Add(def);
						}
						catch (Exception e)
						{
							e = new Exception("Error when loading XML-file: " + file, e);
							throw e;
						}
					}
				}
			}
		}
	}

	public abstract class XmlFileBase
	{
		/// <summary>
		/// The file name of the configuration file.
		/// </summary>
		protected string m_filename;
		protected XmlFileBase MParentFile;

		[XmlIgnore]
		public string FileName
		{
			get
			{
				return m_filename;
			}
			set
			{
				m_filename = value;
			}
		}

		[XmlIgnore]
		public string ActualFile
		{
			get
			{
				if (MParentFile != null)
				{
					return MParentFile.FileName;
				}
				return m_filename;
			}
		}

		public abstract void Save();

		public abstract void SaveAs(string filename);

		protected abstract void OnLoad();
	}
}