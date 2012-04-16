using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Server.WorldServer.Database.Items.Templates
{
    [ActiveRecord("items_pets_templates")]
    [D2OClass("Pet", "com.ankamagames.dofus.datacenter.pets")]
    public class PetTemplate
    {
        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OField("foodItems")]
        [Property("FoodItems", ColumnType = "Serializable")]
        public List<int> FoodItems
        {
            get;
            set;
        }

        [D2OField("foodTypes")]
        [Property("FoodTypes", ColumnType = "Serializable")]
        public List<int> FoodTypes
        {
            get;
            set;
        }
    }
}