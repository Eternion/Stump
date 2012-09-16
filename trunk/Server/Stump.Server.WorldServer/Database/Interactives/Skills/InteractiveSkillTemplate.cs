using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.WorldServer.Game.Interactives;

namespace Stump.Server.WorldServer.Database
{
    public class InteractiveSkillTemplateConfiguration : EntityTypeConfiguration<InteractiveSkillTemplate>
    {
        public InteractiveSkillTemplateConfiguration()
        {
            ToTable("interactives_skills_templates");
            Ignore(x => x.CraftableItemIds);
        }
    }

    public class InteractiveSkillTemplate : IAssignedByD2O
    {
        private List<int> m_craftableItemIds;
        private byte[] m_craftableItemIdsBin;
        private InteractiveTemplate m_interactive;

        public int Id
        {
            get;
            set;
        }

        public uint NameId
        {
            get;
            set;
        }

        public int ParentJobId
        {
            get;
            set;
        }

        public bool IsForgemagus
        {
            get;
            set;
        }

        public int ModifiableItemType
        {
            get;
            set;
        }

        public int GatheredRessourceItem
        {
            get;
            set;
        }

        public byte[] CraftableItemIdsBin
        {
            get { return m_craftableItemIdsBin; }
            set
            {
                m_craftableItemIdsBin = value;
                m_craftableItemIds = CraftableItemIdsBin.ToObject<List<int>>();
            }
        }

        public int InteractiveId
        {
            get;
            set;
        }

        public String UseAnimation
        {
            get;
            set;
        }

        public Boolean IsRepair
        {
            get;
            set;
        }

        public int Cursor
        {
            get;
            set;
        }

        public Boolean AvailableInHouse
        {
            get;
            set;
        }

        public uint LevelMin
        {
            get;
            set;
        }

        public List<int> CraftableItemIds
        {
            get { return m_craftableItemIds ?? (m_craftableItemIds = CraftableItemIdsBin.ToObject<List<int>>()); }
            set
            {
                m_craftableItemIds = value;
                CraftableItemIdsBin = value.ToBinary();
            }
        }


        public InteractiveTemplate Interactive
        {
            get { return m_interactive ?? (m_interactive = InteractiveManager.Instance.GetTemplate(InteractiveId)); }
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var interactive = (Skill) d2oObject;

            NameId = interactive.nameId;
            ParentJobId = interactive.parentJobId;
            IsForgemagus = interactive.isForgemagus;
            ModifiableItemType = interactive.modifiableItemType;
            GatheredRessourceItem = interactive.gatheredRessourceItem;
            CraftableItemIds = interactive.craftableItemIds;
            InteractiveId = interactive.interactiveId;
            UseAnimation = interactive.useAnimation;
            IsRepair = interactive.isRepair;
            Cursor = interactive.cursor;
            AvailableInHouse = interactive.availableInHouse;
            LevelMin = interactive.levelMin;
        }

        #endregion
    }
}