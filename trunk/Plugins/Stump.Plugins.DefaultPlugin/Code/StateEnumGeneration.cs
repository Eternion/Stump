#region License GNU GPL
// StateEnumGeneration.cs
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
using System.Text;
using NLog;
using Stump.Core.Attributes;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Plugins.DefaultPlugin.Code
{
    public class StateEnumGeneration
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [Variable]
        public static bool Active = false;

        [Variable]
        public static string Output = "Gen/SpellStatesEnum.cs";

        [Initialization(typeof(SpellManager), Silent = true)]
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
                writer.WriteLine("\tpublic enum SpellStatesEnum");
                writer.WriteLine("\t{");
                foreach (var state in SpellManager.Instance.GetSpellStates())
                {
                    writer.WriteLine("\t\t/// <summary>");
                    writer.WriteLine("\t\t/// " + state.Name);
                    writer.WriteLine("\t\t/// </summary>");
                    writer.WriteLine("\t\t{0} = {1},", state.Name != "(not found)" ? EscapeString(state.Name) : "State_" + state.Id.ToString(), state.Id);
                }

                writer.WriteLine("\t}");
                writer.WriteLine("}");
                writer.Flush();
            }
        }

        private static string EscapeString(string str)
        {
            var builder = new StringBuilder(str);
            builder.Replace(" ", "_");
            builder.Replace("\"", "");
            builder.Replace("'", "");
            builder.Replace("-", "");
            return builder.ToString();
        }
    }
}