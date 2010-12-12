using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stump.Tools.Proxy
{
    static class Commands
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
