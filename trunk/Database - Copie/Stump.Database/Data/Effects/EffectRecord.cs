using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Effects
{
    [Serializable]
    [ActiveRecord("effect")]
    [AttributeAssociatedFile("Effects")]
    [D2OClass("Effect", "com.ankamagames.dofus.datacenter.effects")]
    public sealed class EffectRecord : DataBaseRecord<EffectRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("descriptionId")]
       [Property("DescriptionId")]
       public uint DescriptionId
       {
           get;
           set;
       }

       [D2OField("iconId")]
       [Property("IconId")]
       public uint IconId
       {
           get;
           set;
       }

       [D2OField("characteristic")]
       [Property("Characteristic")]
       public int Characteristic
       {
           get;
           set;
       }

       [D2OField("category")]
       [Property("Category")]
       public uint Category
       {
           get;
           set;
       }

       [D2OField("operator")]
       [Property("Operator")]
       public String Operator
       {
           get;
           set;
       }

       [D2OField("showInTooltip")]
       [Property("ShowInTooltip")]
       public Boolean ShowInTooltip
       {
           get;
           set;
       }

       [D2OField("useDice")]
       [Property("UseDice")]
       public Boolean UseDice
       {
           get;
           set;
       }

       [D2OField("forceMinMax")]
       [Property("ForceMinMax")]
       public Boolean ForceMinMax
       {
           get;
           set;
       }

       [D2OField("showInSet")]
       [Property("ShowInSet")]
       public Boolean ShowInSet
       {
           get;
           set;
       }

       [D2OField("bonusType")]
       [Property("BonusType")]
       public int BonusType
       {
           get;
           set;
       }

    }
}