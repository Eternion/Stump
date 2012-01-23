using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights.Results.Data;
using Stump.Server.WorldServer.Handlers.Characters;
using FightResultAdditionalData = Stump.Server.WorldServer.Game.Fights.Results.Data.FightResultAdditionalData;

namespace Stump.Server.WorldServer.Game.Fights.Results
{
    public class FightPlayerResult : FightResult<CharacterFighter>
    {
        public FightPlayerResult(CharacterFighter fighter, FightOutcomeEnum outcome, FightLoot loot, FightExperienceData additionalData)
            : base(fighter, outcome, loot)
        {
            AdditionalData = additionalData;
        }

        public FightPlayerResult(CharacterFighter fighter, FightOutcomeEnum outcome, FightLoot loot, FightPvpData additionalData)
            : base(fighter, outcome, loot)
        {
            AdditionalData = additionalData;
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

        public FightResultAdditionalData AdditionalData
        {
            get;
            private set;
        }

        public override FightResultListEntry GetFightResultListEntry()
        {
            DofusProtocol.Types.FightResultAdditionalData[] additionalData = AdditionalData != null
                                                                                 ? new[] {AdditionalData.GetFightResultAdditionalData()}
                                                                                 : new DofusProtocol.Types.FightResultAdditionalData[0];

            return new FightResultPlayerListEntry((short) Outcome, Loot.GetFightLoot(), Id, Alive, Level, additionalData);
        }

        public override void Apply()
        {
            Loot.GiveLoot(Character);
            if (AdditionalData != null)
                AdditionalData.Apply();

            CharacterHandler.SendCharacterStatsListMessage(Character.Client);
        }
    }
}