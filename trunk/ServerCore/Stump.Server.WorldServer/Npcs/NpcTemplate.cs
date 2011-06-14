
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Dialog;
using NpcEx = Stump.DofusProtocol.D2oClasses.Npc;

namespace Stump.Server.WorldServer.Npcs
{
    public class NpcTemplate
    {
        private NpcEx m_npc;
        public NpcTemplate(NpcEx npc)
        {
            m_npc = npc;
            Look = !string.IsNullOrEmpty(npc.look) && npc.look != "null" ? npc.look.ToEntityLook() : null;

            StartActions = new Dictionary<NpcActionTypeEnum, NpcStartAction>();
        }

        public int Id
        {
            get { return m_npc.id; }
        }

        public string Name
        {
            get;
            set;
        }

        public SexTypeEnum Sex
        {
            get { return m_npc.gender != 0 ? SexTypeEnum.SEX_FEMALE : SexTypeEnum.SEX_MALE; }
        }

        public EntityLook Look
        {
            get;
            set;
        }

        public Dictionary<NpcActionTypeEnum, NpcStartAction> StartActions
        {
            get;
            set;
        }

        public bool CanSpeak
        {
            get
            {
                return StartActions.ContainsKey(NpcActionTypeEnum.ACTION_TALK);
            }
        }
    }
}