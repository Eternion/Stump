
using System.Collections.Generic;
using Stump.Database.WorldServer;
using Stump.Database.WorldServer.Character;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Data.Threshold;
using Stump.Server.WorldServer.Spells;
using Stump.Server.WorldServer.World.Actors;
using Stump.Server.WorldServer.World.Actors.Character;
using Stump.Server.WorldServer.World.Entities.Actors;
using Stump.Server.WorldServer.World.Storages;

namespace Stump.Server.WorldServer.World.Entities.Characters
{
    public partial class Character
    {

        public readonly WorldClient Client;

        private readonly CharacterRecord m_record;

        public PlayableBreedEnum Breed { get; set; }

        public SexTypeEnum Sex { get; set; }

        public uint Energy { get; set; }

        public uint EnergyMax { get; set; }

        public long Experience { get; set; }

        public uint Level
        {
            get { return ThresholdManager.Instance["CharacterExp"].GetLevel(Experience); }
            set { Experience = ThresholdManager.Instance["CharacterExp"].GetLowerBound(value); }
        }

        public Inventory Inventory
        {
            get;
            set;
        }

        public SpellInventory SpellInventory
        {
            get;
            set;
        }



        //public Guild Guild;

        //public List<Job> Jobs;

        //public List<Quests> Quests;

        //public List<Zaap> Zaaps;

        public int Mood {  get; set; }

        public GameContextEnum Context { get; set; }

        public List<Actor> FollowingCharacters { get; set; }

        public int EmoteId { get; set; }

        public uint EmoteEndTime { get; set; }

        public CharacterRestrictions Restrictions { get; set; }

        public uint TitleId { get; set; }

        public string TitleParam { get; set; }

        public ActorAlignment Alignment { get; set; }

    }
}