
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Npcs;
using Stump.Server.WorldServer.Threshold;

namespace Stump.Server.WorldServer.Data
{
    public class ThresholdLoader
    {
        /// <summary>
        ///   Name of Thresholds folder
        /// </summary>
        [Variable]
        public static string ThresholdsDir = "Thresholds/";


        public static Dictionary<string, ThresholdDictionnary> LoadThresholds()
        {
            var directory = new DirectoryInfo(Settings.StaticPath + ThresholdsDir);
            var dico = new Dictionary<string, ThresholdDictionnary>();

            foreach (FileInfo file in directory.GetFiles("*.xml"))
            {
                var name = file.Name.Split('.')[0];
                var doc = XDocument.Load(file.FullName);
                dico.Add(name, new ThresholdDictionnary(name, XDocument.Load(file.FullName)));
            }
            return dico;
        }
    }
}