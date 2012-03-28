using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Game.Interactives;

namespace Stump.Server.WorldServer.Database.Interactives.Skills
{
    [ActiveRecord("skills_templates")]
    [D2OClass("Skill", "com.ankamagames.dofus.datacenter.jobs")]
    public class SkillTemplate : WorldBaseRecord<SkillTemplate>
    {
        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }


        [D2OField("nameId")]
        [Property("NameId")]
        public uint NameId
        {
            get;
            set;
        }

        [D2OField("parentJobId")]
        [Property("ParentJobId")]
        public int ParentJobId
        {
            get;
            set;
        }

        [D2OField("isForgemagus")]
        [Property("IsForgemagus")]
        public Boolean IsForgemagus
        {
            get;
            set;
        }

        [D2OField("modifiableItemType")]
        [Property("ModifiableItemType")]
        public int ModifiableItemType
        {
            get;
            set;
        }

        [D2OField("gatheredRessourceItem")]
        [Property("GatheredRessourceItem")]
        public int GatheredRessourceItem
        {
            get;
            set;
        }

        [D2OField("craftableItemIds")]
        [Property("CraftableItemIds", ColumnType = "Serializable")]
        public List<int> CraftableItemIds
        {
            get;
            set;
        }

        [D2OField("interactiveId")]
        [Property("InteractiveId")]
        public int InteractiveId
        {
            get;
            set;
        }

        private InteractiveTemplate m_interactive;
        public InteractiveTemplate Interactive
        {
            get
            {
                return m_interactive ?? (m_interactive = InteractiveManager.Instance.GetTemplate(InteractiveId));
            }
        }

        [D2OField("useAnimation")]
        [Property("UseAnimation")]
        public String UseAnimation
        {
            get;
            set;
        }

        [D2OField("isRepair")]
        [Property("`IsRepair`")]
        public Boolean IsRepair
        {
            get;
            set;
        }

        [D2OField("cursor")]
        [Property("`Cursor`")]
        public int Cursor
        {
            get;
            set;
        }

        [D2OField("availableInHouse")]
        [Property("AvailableInHouse")]
        public Boolean AvailableInHouse
        {
            get;
            set;
        }

        [D2OField("levelMin")]
        [Property("`LevelMin`")]
        public int LevelMin
        {
            get;
            set;
        }
    }
}