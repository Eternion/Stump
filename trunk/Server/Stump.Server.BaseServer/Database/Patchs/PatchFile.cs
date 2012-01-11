using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Stump.Server.BaseServer.Database.Patchs
{
    public class PatchFile
    {
        public PatchFile(string path)
        {
            Path = path;
            FileName = System.IO.Path.GetFileNameWithoutExtension(Path);

            Parse();
        }

        private void Parse()
        {
            Match match;
            if (!( match = Regex.Match(FileName, "([0-9]+)_to_([0-9]+)") ).Success)
                throw new Exception(string.Format("Cannot parse file {0}, right syntax is : A_to_B.sql", FileName));

            ForRevision = uint.Parse(match.Groups[1].Value);
            ToRevision = uint.Parse(match.Groups[2].Value);
        }

        public string Path
        {
            get;
            private set;
        }

        public string FileName
        {
            get;
            private set;
        }

        public uint ForRevision
        {
            get;
            private set;
        }

        public uint ToRevision
        {
            get;
            private set;
        }   

        public static IEnumerable<PatchFile> GeneratePatchSequenceExecution(IEnumerable<PatchFile> files, uint forRevision, uint toRevision)
        {
            if (files.Count() <= 0)
                return Enumerable.Empty<PatchFile>();

            var possibleSequences = new List<IEnumerable<PatchFile>>();

            foreach (var file in files.Where(entry => entry.ForRevision == forRevision))
            {
                // direct update
                if (file.ForRevision == forRevision &&
                    file.ToRevision == toRevision)
                    return new[] { file };

                var patchs = new List<PatchFile>()
                {
                    file
                };

                patchs.AddRange(GeneratePatchSequenceExecution(files.Where(entry => entry.ForRevision >= file.ToRevision), file.ToRevision, toRevision));

                if (patchs.Count > 1)
                    possibleSequences.Add(patchs);
            }

            if (possibleSequences.Count <= 0)
                return Enumerable.Empty<PatchFile>();

            // return the sequence that have lesser patchs
            return possibleSequences.OrderBy(entry => entry.Count()).First();
        }
    }
}