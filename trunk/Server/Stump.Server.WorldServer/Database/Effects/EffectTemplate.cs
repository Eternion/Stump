using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Server.WorldServer.Database.Effects
{
    [Serializable]
    [ActiveRecord("effects")]
    [D2OClass("Effect", "com.ankamagames.dofus.datacenter.effects")]
    public class EffectTemplate : WorldBaseRecord<EffectTemplate>
    {
        [PrimaryKey(PrimaryKeyType.Assigned)]
        [D2OField("id")]
        public uint Id
        {
            get;
            set;
        }

        [Property]
        [D2OField("descriptionId")]
        public uint DescriptionId
        {
            get;
            set;
        }

        [Property]
        [D2OField("iconId")]
        public uint IconId
        {
            get;
            set;
        }

        [Property]
        [D2OField("characteristic")]
        public int Characteristic
        {
            get;
            set;
        }

        [Property]
        [D2OField("category")]
        public uint Category
        {
            get;
            set;
        }

        [Property]
        [D2OField("operator")]
        public string Operator
        {
            get;
            set;
        }

        [Property]
        [D2OField("showInTooltip")]
        public bool ShowInTooltip
        {
            get;
            set;
        }

        [Property]
        [D2OField("useDice")]
        public bool UseDice
        {
            get;
            set;
        }

        [Property]
        [D2OField("forceMinMax")]
        public bool ForceMinMax
        {
            get;
            set;
        }

        [Property]
        [D2OField("boost")]
        public bool Boost
        {
            get;
            set;
        }

        [Property]
        [D2OField("active")]
        public bool Active
        {
            get;
            set;
        }

        [Property]
        [D2OField("showInSet")]
        public bool ShowInSet
        {
            get;
            set;
        }

        [Property]
        [D2OField("bonusType")]
        public int BonusType
        {
            get;
            set;
        }

        [Property]
        [D2OField("useInFight")]
        public bool UseInFight
        {
            get;
            set;
        }
    }
}