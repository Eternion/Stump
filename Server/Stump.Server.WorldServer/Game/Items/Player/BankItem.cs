using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player
{
    public class BankItem : PersistantItem<BankItemRecord>
    {
        public BankItem(Character owner, BankItemRecord record)
        {
            Owner = owner;
            Record = record;

            if (record.Effects == null)
                record.Effects = ItemManager.Instance.GenerateItemEffects(Record.Template);
        }

        public Character Owner
        {
            get;
            private set;
        }

        public virtual int Weight
        {
            get { return (int) (Template.RealWeight*Stack); }
        }

        public override ObjectItem GetObjectItem()
        {
            return new ObjectItem(63, (short) Template.Id, 0, false, Effects.Select(x => x.GetObjectEffect()), Guid, (int) Stack);
        }
    }
}