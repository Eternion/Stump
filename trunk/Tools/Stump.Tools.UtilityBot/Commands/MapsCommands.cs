using System;
using System.IO;
using System.Threading.Tasks;
using Squishy.Irc.Commands;
using Stump.BaseCore.Framework.Attributes;
using Stump.Server.BaseServer.Data.MapTool;

namespace Stump.Tools.UtilityBot.Commands
{
    public class GenerateMapsCommand : Command
    {
        public GenerateMapsCommand()
            : base("genmaps")
        {
            Description = "Generate maps files";
        }

        /// <summary>
        /// Output path
        /// </summary>
        [Variable]
        public static string Output = "./../../content/maps/";

        public override void Process(CmdTrigger trigger)
        {
            trigger.Reply("Generating maps. It can take few minutes.");

            var file = new PakFile(Bot.DofusPath + @"\app\content\maps\maps0.d2p");

            file.ExtractAll(Output);

            trigger.Reply("Maps extracted (1/3)");

            Parallel.ForEach(Directory.EnumerateFiles(Output, "*.dlm", SearchOption.AllDirectories), dlm =>
            {
                var stream = File.OpenRead(dlm);
                int header = stream.ReadByte();
                stream.Close();

                string mapPath = Path.GetDirectoryName(dlm) + "/" + Path.GetFileNameWithoutExtension(dlm) + ".map";

                if (header != 77)
                {
                    Compressor.UnCompressDlmFile(dlm, mapPath);
                }
                else
                {
                    File.Copy(dlm, mapPath);
                }

                File.Delete(dlm);
            });

            trigger.Reply("Maps decompressed (2/3)");

            Parallel.ForEach(Directory.EnumerateFiles(Output, "*.map", SearchOption.AllDirectories), map =>
                MapRipper.RipMapFile(map, map));

            trigger.Reply("Maps ripped. Done (3/3)");
        }
    }
}