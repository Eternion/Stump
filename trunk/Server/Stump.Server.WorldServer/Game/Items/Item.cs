using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items
{
    public interface IItem
    {
        IItemRecord Record
        {
            get;
        }

        int Guid
        {
            get;
        }

        int Stack
        {
            get;
            set;
        }

        ItemTemplate Template
        {
            get;
        }

        List<EffectBase> Effects
        {
            get;
        }
    }

    public abstract class Item<T> : IItem where T : ItemRecord<T>
    {
        protected Item()
        {
            
        }

        protected Item(T record)
        {
            Record = record;
        }

        IItemRecord IItem.Record
        {
            get { return Record; }
        }

        public T Record
        {
            get;
            protected set;
        }

        public virtual int Guid
        {
            get { return Record.Id; }
            protected set { Record.Id = value; }
        }

        public virtual int Stack
        {
            get { return Record.Stack; }
            set { Record.Stack = value; }
        }

        public virtual ItemTemplate Template
        {
            get { return Record.Template; }
            protected set { Record.Template = value; }
        }

        public virtual List<EffectBase> Effects
        {
            get { return Record.Effects; }
            protected set { Record.Effects = value; }
        }

        public ObjectItemInformationWithQuantity GetObjectItemInformationWithQuantity()
        {
            return new ObjectItemInformationWithQuantity((short) Template.Id, 0, false, Effects.Select(entry => entry.GetObjectEffect()).ToArray(), Stack);
        }
    }
}