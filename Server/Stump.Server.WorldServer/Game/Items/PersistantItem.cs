using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items
{
    public interface IPersistantItem : IItem
    {
        IItemRecord Record
        {
            get;
        }
        bool IsTemporarily
        {
            get;
        }

        void OnPersistantItemAdded();
        void OnPersistantItemUpdated();
        void OnPersistantItemDeleted();
    }

    public interface IItem
    {
        int Guid
        {
            get;
        }

        uint Stack
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
            set;
        }

        ObjectItem GetObjectItem();
    }

    public abstract class Item : IItem
    {
        
        public virtual int Guid
        {
            get;
            protected set;
        }

        public virtual uint Stack
        {
            get;
            set;
        }

        public virtual ItemTemplate Template
        {
            get;
            protected set;
        }

        public virtual List<EffectBase> Effects
        {
            get;
            set;
        }

        public abstract ObjectItem GetObjectItem();
    }

    public abstract class PersistantItem<T> : IPersistantItem where T : ItemRecord<T>
    {
        protected PersistantItem()
        {
            
        }

        protected PersistantItem(T record)
        {
            Record = record;
        }

        IItemRecord IPersistantItem.Record
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

        public virtual uint Stack
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
            set { Record.Effects = value; }
        }

        public bool IsTemporarily
        {
            get;
            protected set;
        }

        public abstract ObjectItem GetObjectItem();

        public virtual void OnPersistantItemAdded()
        {
        }

        public virtual void OnPersistantItemUpdated()
        {
        }

        public virtual void OnPersistantItemDeleted()
        {
        }

        public ObjectItemInformationWithQuantity GetObjectItemInformationWithQuantity()
        {
            return new ObjectItemInformationWithQuantity((short) Template.Id, Effects.Select(entry => entry.GetObjectEffect()).ToArray(), (int)Stack);
        }
    }
}