using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Stump.Core.Cryptography;
using Stump.Core.Xml;
using Uplauncher.Patcher;

namespace PatchBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            string patchDir;
            if (args.Length == 0)
            {
                Console.WriteLine(@"Give the patch directory in argument");
                patchDir = Console.ReadLine();
            }
            else
            {
                patchDir = args[0];
            }

            if (File.Exists(Path.Combine(patchDir, "patch.xml")))
            {
                File.Delete(Path.Combine(patchDir, "patch.xml"));
            }

            foreach (var directory in Directory.GetDirectories(patchDir))
            {
                var directoryName = Path.GetFileName(directory);

                if (directoryName != "app")
                    continue;

                var tasks = Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).Select(x => new AddFileTask
                {
                    LocalURL = GetRelativePath(x, directory + "\\"),
                    RelativeURL = GetRelativePath(x, patchDir + "\\"),
                    FileMD5 = Cryptography.GetFileMD5HashBase64(x)
                }).ToArray();

                var patch = new Patch
                {
                    Tasks = tasks,
                };

                foreach (var task in tasks)
                {
                    Console.WriteLine(@"Add " + task.RelativeURL);
                }

                XmlUtils.Serialize(Path.Combine(patchDir, "patch.xml"), patch);
                Console.WriteLine(@"Created Patch in {0} !", Path.Combine(patchDir, "patch.xml"));

                File.WriteAllText(Path.Combine(patchDir, "checksum.arkalys"), GetMD5Dir(directory));
            }

            Console.Read();
        }

        static string GetRelativePath(string fullPath, string relativeTo)
        {
            var foldersSplitted = fullPath.Split(new[] { relativeTo.Replace("/", "\\").Replace("\\\\", "\\") }, StringSplitOptions.RemoveEmptyEntries); // cut the source path and the "rest" of the path

            return foldersSplitted.Length > 0 ? foldersSplitted.Last() : ""; // return the "rest"
        }

        static string GetMD5Dir(string path)
        {
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                                 .OrderBy(p => p).ToList();

            var md5 = MD5.Create();

            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];

                var relativePath = file.Substring(path.Length + 1);
                var pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
                md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

                var contentBytes = File.ReadAllBytes(file);
                if (i == files.Count - 1)
                    md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
                else
                    md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
            }

            return files.Count != 0 ? BitConverter.ToString(md5.Hash).Replace("-", "").ToLower() : "0";
        }
    }
}
