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
using System.Linq;

namespace Stump.Tools.Proxy
{
    internal static class Commands
    {
        public static void ProcessClientCommand(string[] data, DerivedConnexion sender)
        {
            /* Switch for Main command */
            switch (data[0].ToUpper())
            {
                    /* Enable or disable Packet analysis */
                case ("ANALYSE"):
                {
                    sender.Analyse = !sender.Analyse;
                    sender.SendClientChatMessage("L'analyse est maintenant " + (sender.Analyse ? "ACTIF" : "INACTIF"));
                    break;
                }
                    /* Enable or disable Debug logs */
                case ("DEBUG"):
                {
                    sender.Debug = !sender.Debug;
                    sender.SendClientChatMessage("Le mode DEBUG est maintenant " + (sender.Debug ? "ACTIF" : "INACTIF"));
                    break;
                }
                    /* Display how many players are connected */
                case ("COUNT"):
                {
                    sender.SendClientChatMessage(Proxy.clientList.Count + " connecté(s).");
                    break;
                }
                    /* Display message foreach connected players */
                case ("ALL"):
                {
                    if (data.Length < 2) return;
                    foreach (WorldDerivedConnexion conn in Proxy.clientList.Where(dc => dc.Infos != null))
                        sender.SendClientChatMessage((sender as WorldDerivedConnexion).Infos.name + " => " + data[1]);
                    break;
                }
            }
        }
    }
}