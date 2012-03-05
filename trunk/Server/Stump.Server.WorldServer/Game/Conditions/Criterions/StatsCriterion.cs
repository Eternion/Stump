﻿using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Conditions.Criterions
{
    public class StatsCriterion : Criterion
    {
        private static readonly Dictionary<string, PlayerFields> CriterionsBinds = new Dictionary<string, PlayerFields>()
        {
            {"CA", PlayerFields.Agility},
            {"CC", PlayerFields.Chance},
            {"CS", PlayerFields.Strength},
            {"CI", PlayerFields.Intelligence},
            {"CW", PlayerFields.Wisdom},
            {"CV", PlayerFields.Vitality},
            {"CL", PlayerFields.Health},
            {"CM", PlayerFields.MP},
            {"CP", PlayerFields.AP},
            {"Ct", PlayerFields.TackleEvade},
            {"CT", PlayerFields.TackleBlock},
        };

        private static readonly Dictionary<string, PlayerFields> CriterionsStatsBaseBinds = new Dictionary<string, PlayerFields>()
        {
            {"Ca", PlayerFields.Agility},
            {"Cc", PlayerFields.Chance},
            {"Cs", PlayerFields.Strength},
            {"Ci", PlayerFields.Intelligence},
            {"Cw", PlayerFields.Wisdom},
            {"Cv", PlayerFields.Vitality},
        };

        private static readonly string[] ExtraCriterions = new[]
        {
            "Ce",
            "CE",
            "CD",
            "CH",
        };

        public StatsCriterion(string identifier)
        {
            Identifier = identifier;
            Field = null;
        }

        public string Identifier
        {
            get;
            private set;
        }

        public PlayerFields? Field
        {
            get;
            set;
        }

        public bool Base
        {
            get;
            set;
        }

        public int Comparand
        {
            get;
            set;
        }

        public static bool IsStatsIdentifier(string identifier)
        {
            return CriterionsBinds.ContainsKey(identifier) || CriterionsStatsBaseBinds.ContainsKey(identifier) || ExtraCriterions.Any(entry => entry == identifier);
        }

        public override bool Eval(Character character)
        {
            // extra field
            if (!Field.HasValue)
            {
                switch (Identifier)
                {
                    case "Ce":
                        return Compare(character.Energy, Comparand);
                    case "CE":
                        return Compare(character.EnergyMax, Comparand);
                    case "CD":
                        return true; // dishonor
                    case "CH":
                        return true; // honnor
                    default:
                        throw new Exception(string.Format("Cannot eval StatsCriterion {0}, {1} is not a stats identifier", this, Identifier));
                }
            }

            return Compare(Base ? character.Stats[Field.Value].Base : character.Stats[Field.Value].Total, Comparand);
        }

        public override void Build()
        {
            if (CriterionsBinds.ContainsKey(Identifier))
                Field = CriterionsBinds[Identifier];
            else if (CriterionsStatsBaseBinds.ContainsKey(Identifier))
            {
                Field = CriterionsStatsBaseBinds[Identifier];
                Base = true;
            }
            else if (!ExtraCriterions.Any(entry => entry == Identifier))
                throw new Exception(string.Format("Cannot build StatsCriterion, {0} is not a stats identifier", Identifier));

            int comparand;

            if (!int.TryParse(Literal, out comparand))
                throw new Exception(string.Format("Cannot build StatsCriterion, {0} is not a valid integer", Literal));

            Comparand = comparand;
        }

        public override string ToString()
        {
            return FormatToString(Identifier);
        }
    }
}