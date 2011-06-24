using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Items
{
    [Serializable]
    [ActiveRecord("weapon")]
    [AttributeAssociatedFile()]
    [D2OClass("Weapon", "com.ankamagames.dofus.datacenter.items")]
    public sealed class WeaponRecord : DataBaseRecord<WeaponRecord>
    {

       [D2OField("apCost")]
       [Property("ApCost")]
       public int ApCost
       {
           get;
           set;
       }

       [D2OField("minRange")]
       [Property("MinRange")]
       public int MinRange
       {
           get;
           set;
       }

       [D2OField("range")]
       [Property("Range")]
       public int Range
       {
           get;
           set;
       }

       [D2OField("castInLine")]
       [Property("CastInLine")]
       public Boolean CastInLine
       {
           get;
           set;
       }

       [D2OField("castInDiagonal")]
       [Property("CastInDiagonal")]
       public Boolean CastInDiagonal
       {
           get;
           set;
       }

       [D2OField("castTestLos")]
       [Property("CastTestLos")]
       public Boolean CastTestLos
       {
           get;
           set;
       }

       [D2OField("criticalHitProbability")]
       [Property("CriticalHitProbability")]
       public int CriticalHitProbability
       {
           get;
           set;
       }

       [D2OField("criticalHitBonus")]
       [Property("CriticalHitBonus")]
       public int CriticalHitBonus
       {
           get;
           set;
       }

       [D2OField("criticalFailureProbability")]
       [Property("CriticalFailureProbability")]
       public int CriticalFailureProbability
       {
           get;
           set;
       }

    }
}