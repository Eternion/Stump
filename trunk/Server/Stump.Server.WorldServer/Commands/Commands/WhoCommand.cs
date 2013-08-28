using System;
using System.Text.RegularExpressions;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class WhoCommand : CommandBase
    {
        private static Regex m_numberRegex = new Regex("^[0-9]+$", RegexOptions.Compiled);
        private static Regex m_numberRangeRegex = new Regex("^([0-9]+)-([0-9]+)$", RegexOptions.Compiled);

        [Variable]
        public static int DisplayedElementsLimit = 50;

        public WhoCommand()
        {
            Aliases = new [] { "who" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Return a list of playe based on the given arguments";
            AddParameter<string>("parameters", "params", "Level (exact or range x-y), Breed, Area or Name (partial)", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            Predicate<Character> predicate = x => true;
            
            if (trigger.IsArgumentDefined("params"))
            {
                var parameters = trigger.Get<string>("params");
                if (m_numberRegex.IsMatch(parameters))
                {
                    int level = int.Parse(parameters);
                    predicate = x => x.Level == level;
                }
                else
                {
                    var match = m_numberRangeRegex.Match(parameters);
                    if (match.Success)
                    {
                        int min = int.Parse(match.Groups[1].Value);
                        int max = int.Parse(match.Groups[2].Value);
                        predicate = x => x.Level >= min && x.Level <= max;
                    }
                    else
                    {
                        PlayableBreedEnum breed;
                        if (Enum.TryParse(parameters, true, out breed))
                            predicate = x => x.BreedId == breed;
                        else
                        {
                            var area = World.Instance.GetArea(parameters);
                            if (area != null)
                            {
                                predicate = x => x.Area == area;
                            }
                            else
                            {
                                predicate =
                                    x => x.Name.IndexOf(parameters, StringComparison.InvariantCultureIgnoreCase) != -1;
                            }
                        }
                    }
                }
            }

            var list = World.Instance.GetCharacters(predicate);
            int counter = 0;

            foreach (var character in list)
            {
                trigger.ReplyBold(" - {0} ({1}) in {2}", character.Name, character.Level, character.Area.Name);
                counter++;

                if (counter >= DisplayedElementsLimit)
                {
                    trigger.Reply("(...)");
                    break;
                }
            }

            if (counter == 0)
                trigger.ReplyError("No results found");
        }
    }
}