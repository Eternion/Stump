using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Stump.Core.Xml;
using Uplauncher.Patcher;

namespace PatchBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                Console.WriteLine("Give the patch directory in argument");
                Console.Read();
                Environment.Exit(-1);
            }

            var patchDir = args[0];

            UpdateMeta meta;
            if (File.Exists(Path.Combine(patchDir, "updates.xml")))
            {
                meta = XmlUtils.Deserialize<UpdateMeta>(Path.Combine(patchDir, "updates.xml"));
            }
            else
            {
                meta = new UpdateMeta();
                meta.LastVersion = 1;
            }

            foreach (var directory in Directory.GetDirectories(patchDir))
            {
                var directoryName = Path.GetFileName(directory);
                var match = Regex.Match(directoryName, "([0-9]+)_to_([0-9]+)");
                if (!match.Success)
                    continue;

                var from = int.Parse(match.Groups[1].Captures[0].Value);
                var to = int.Parse(match.Groups[1].Captures[1].Value);

                if (meta.Updates.Any(x => x.FromVersion == from && x.ToVersion == to))
                    continue;

                var tasks = Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).Select(x => new AddFileTask()
                {
                    LocalURL = GetRelativePath(x, directory),
                    RelativeURL = GetRelativePath(x, patchDir),
                }).ToArray();

                var patch = new Patch()
                {
                    Tasks = tasks,
                };

                XmlUtils.Serialize(Path.Combine(directory, "patch.xml"), patch);
                Console.WriteLine(string.Format("Create Patch from '{0}' to '{1}' : {2} !", from, to, Path.Combine(directory, "patch.xml")));

                meta.Updates.Add(new UpdateEntry()
                {
                    FromVersion = from,
                    ToVersion = to,
                    PatchRelativURL = directoryName + "/patch.xml",
                });


            }


            XmlUtils.Serialize(Path.Combine(patchDir, "updates.xml"), meta);

            Console.WriteLine(string.Format("Meta file {0} updated !", Path.Combine(patchDir, "updates.xml")));

        }

        static string GetRelativePath(string fullPath, string relativeTo)
        {
            string folder = Path.GetDirectoryName(fullPath);
            string[] foldersSplitted = folder.Split(new[] { relativeTo.Replace("/", "\\") }, StringSplitOptions.RemoveEmptyEntries); // cut the source path and the "rest" of the path

            return foldersSplitted.Length > 1 ? foldersSplitted[1] : ""; // return the "rest"
        }
    }
}
