using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.ORM;
using Stump.Server.BaseServer.Database;

namespace Stump.Server.WorldServer.Database
{
    public class PetTemplateConfiguration : EntityTypeConfiguration<PetTemplate>
    {
        public PetTemplateConfiguration()
        {
            ToTable("items_pets");
            Ignore(x => x.FoodItems);
            Ignore(x => x.FoodTypes);
        }
    }

    [D2OClass("Pet", "com.ankamagames.dofus.datacenter.pets")]
    public class PetTemplate : IAssignedByD2O, ISaveIntercepter
    {
        private List<int> m_foodItems;
        private byte[] m_foodItemsBin;
        private List<int> m_foodTypes;
        private byte[] m_foodTypesBin;

        public int Id
        {
            get;
            set;
        }

        public byte[] FoodItemsBin
        {
            get { return m_foodItemsBin; }
            set
            {
                m_foodItemsBin = value;
                m_foodItems = value.ToObject<List<int>>();
            }
        }

        public List<int> FoodItems
        {
            get { return m_foodItems; }
            set
            {
                m_foodItems = value;
                m_foodItemsBin = value.ToBinary();
            }
        }

        public byte[] FoodTypesBin
        {
            get { return m_foodTypesBin; }
            set
            {
                m_foodTypesBin = value;
                m_foodTypes = value.ToObject<List<int>>();
            }
        }

        public List<int> FoodTypes
        {
            get { return m_foodTypes; }
            set
            {
                m_foodTypes = value;
                m_foodTypesBin = value.ToBinary();
            }
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var pet = (Pet) d2oObject;
            Id = pet.id;
            FoodItems = pet.foodItems;
            FoodTypes = pet.foodTypes;
        }

        #endregion

        #region ISaveIntercepter Members

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            m_foodItemsBin = m_foodItems.ToBinary();
            m_foodTypesBin = m_foodTypes.ToBinary();
        }

        #endregion
    }
}