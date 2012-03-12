using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Characters;
using FightResultAdditionalData = Stump.Server.WorldServer.Game.Fights.Results.Data.FightResultAdditionalData;

namespace Stump.Server.WorldServer.Game.Fights.Results
{
    public class FightPlayerResult : FightResult<CharacterFighter>
    {
        public FightPlayerResult(CharacterFighter fighter, FightOutcomeEnum outcome, FightLoot loot,
                                 params FightResultAdditionalData[] additionalDatas)
            : base(fighter, outcome, loot)
        {
            AdditionalDatas = additionalDatas;
        }

        public FightPlayerResult(CharacterFighter fighter, FightOutcomeEnum outcome, FightLoot loot)
            : base(fighter, outcome, loot)
        {
        }

        public Character Character
        {
            get { return Fighter.Character; }
        }

        public byte Level
        {
            get { return Character.Level; }
        }

        public FightResultAdditionalData[] AdditionalDatas
        {
            get;
            private set;
        }

        public override FightResultListEntry GetFightResultListEntry()
        {
            var additionalDatas = AdditionalDatas != null ? 
                AdditionalDatas.Select(entry => entry.GetFightResultAdditionalData())
                : new DofusProtocol.Types.FightResultAdditionalData[0];

            return new FightResultPlayerListEntry((short) Outcome, Loot.GetFightLoot(), Id, Alive, Level,
                                                  additionalDatas);
        }

        public override void Apply()
        {
            Loot.GiveLoot(Character);
            if (AdditionalDatas != null)
                foreach (FightResultAdditionalData additionalData in AdditionalDatas)
                {
                    additionalData.Apply();
                }

            CharacterHandler.SendCharacterStatsListMessage(Character.Client);
        }
    }
}