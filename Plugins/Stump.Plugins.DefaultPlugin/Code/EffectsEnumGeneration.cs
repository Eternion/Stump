#region License GNU GPL
// EffectsEnumGeneration.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System.IO;
using NLog;
using Stump.Core.Attributes;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Effects;

namespace Stump.Plugins.DefaultPlugin.Code
{
    public class EffectsEnumGeneration
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [Variable]
        public static bool Active = false;

        [Variable]
        public static string Output = "Gen/EffectsEnum.cs";

        [Initialization(typeof(EffectManager), Silent=true)]
        public static void Initialize()
        {
            if (!Active)
                return;

            logger.Debug("Generate {0} ...", Output);

            var file = Path.Combine(Plugin.CurrentPlugin.GetPluginDirectory(), Output);

            if (!Directory.Exists(Path.GetDirectoryName(file)))
                Directory.CreateDirectory(Path.GetDirectoryName(file));

            using (var writer = File.CreateText(file))
            {
                writer.WriteLine("namespace Stump.DofusProtocol.Enums");
                writer.WriteLine("{");
                writer.WriteLine("\tpublic enum EffectsEnum : short");
                writer.WriteLine("\t{");
                foreach (var effect in EffectManager.Instance.GetTemplates())
                {
                    var description = TextManager.Instance.GetText(effect.DescriptionId);

                    writer.WriteLine("\t\t/// <summary>");
                    writer.WriteLine("\t\t/// " + description);
                    writer.WriteLine("\t\t/// </summary>");
                    writer.WriteLine("\t\tEffect_{0} = {0},", effect.Id);
                }

                writer.WriteLine("\t}");
                writer.WriteLine("}");
                writer.Flush();
            }
        }
    }
}