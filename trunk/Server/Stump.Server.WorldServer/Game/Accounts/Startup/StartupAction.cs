using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Startup;

namespace Stump.Server.WorldServer.Game.Accounts.Startup
{
    public class StartupAction
    {
        private readonly StartupActionRecord m_record;

        public StartupActionRecord Record
        {
            get { return m_record; }
        }

        public StartupAction(StartupActionRecord record)
        {
            m_record = record;
            Items = record.Items.Select(entry => new StartupActionItem(entry)).ToArray();
        }

        public int Id
        {
            get { return m_record.Id; }
            set { m_record.Id = value; }
        }

        public string Title
        {
            get { return m_record.Title; }
            set { m_record.Title = value; }
        }

        public string Text
        {
            get { return m_record.Text; }
            set { m_record.Text = value; }
        }

        public string DescUrl
        {
            get { return m_record.DescUrl; }
            set { m_record.DescUrl = value; }
        }

        public string PictureUrl
        {
            get { return m_record.PictureUrl; }
            set { m_record.PictureUrl = value; }
        }

        public StartupActionItem[] Items
        {
            get;
            private set;
        }

        public void GiveGiftTo(CharacterRecord character)
        {
            foreach (var item in Items)
            {
                item.GiveTo(character);
            }
        }

        public StartupActionAddObject GetStartupActionAddObject()
        {
            return new StartupActionAddObject(Id, Title, Text, DescUrl, PictureUrl, Items.Select(entry => entry.GetObjectItemInformationWithQuantity()).ToArray());
        }
    }
}