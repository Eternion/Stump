
using System.Linq;
using Stump.Database.WorldServer;
using Stump.Database.WorldServer.Character;
using Stump.DofusProtocol.Classes;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Helpers;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.Spells;
using Stump.Server.WorldServer.World.Actors;
using Stump.Server.WorldServer.World.Actors.Character;
using Stump.Server.WorldServer.World.Entities.Actors;
using Stump.Server.WorldServer.World.Storages;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.World.Entities.Characters
{
    public partial class Character : Actor, IInventoryOwner, ISpellsOwner, IAligned
    {

        public Character(WorldClient client, CharacterRecord record)
            : base(record.Id, record.Name, CharacterManager.GetStuffedCharacterLook(record), new ObjectPosition(Global.World.Instance.GetMap(record.MapId), record.CellId, record.Direction))
        {
            Client = client;
            m_record = record;
            Breed = record.Breed;
            Sex = record.Sex;
            Energy = record.Energy;
            EnergyMax = record.EnergyMax;
            Experience = record.Experience;
            Inventory = new Inventory(this, record.Inventory);
            SpellInventory = new SpellInventory(this, record.Spells, record.SpellsPoints);
            Restrictions = new CharacterRestrictions(record.Restrictions);
            Alignment = new ActorAlignment(this,record.Alignment);
            TitleId = record.TitleId;
            TitleParam = record.TitleParam;
        }


        public void Save()
        {
            m_record.Breed = Breed;
            m_record.Sex = Sex;
            m_record.Energy = Energy;
            m_record.EnergyMax = EnergyMax;
            m_record.Experience = Experience;
            m_record.Inventory = Inventory.Record;
            //Inventory.Save();
            //spells
            m_record.Alignment = Alignment.Record;
            Alignment.Save();
            m_record.TitleId = TitleId;
            m_record.TitleParam=TitleParam;
            m_record.SaveAndFlush();
        }


        public override GameRolePlayActorInformations ToGameRolePlayActor()
        {
            return new GameRolePlayCharacterInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, new HumanInformations(FollowingCharacters.Select(f => f.Look.EntityLook).ToList(), EmoteId, EmoteEndTime, Restrictions.ToActorRestrictionsInformations(), TitleId, TitleParam), Alignment.ToActorAlignmentInformations());
        }
    }
}