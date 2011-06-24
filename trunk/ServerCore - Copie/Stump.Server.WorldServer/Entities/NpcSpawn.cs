
using System;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Npcs;

namespace Stump.Server.WorldServer.Entities
{
    public class NpcSpawn : Entity, ISpawnEntry
    {
        public NpcSpawn(Map map, NpcTemplate template, int contextualId, ObjectPosition position, bool sex,
                        int artworkId, EntityLook look)
            : base(contextualId)
        {
            Map = map;
            Context = map;

            Template = template;
            Look = new ExtendedLook(look);
            Position = position;
            Sex = sex;
            SpecialArtworkId = artworkId;

            IsQuestGiver = false;
        }

        public NpcSpawn(Map map, NpcTemplate template, int contextualId, ObjectPosition position, bool sex,
                        int artworkId)
            : base(contextualId)
        {
            Map = map;
            Context = map;

            Template = template;
            Look = new ExtendedLook(template.Look);
            Position = position;
            Sex = sex;
            SpecialArtworkId = artworkId;

            IsQuestGiver = false;
        }

        public NpcTemplate Template
        {
            get;
            protected set;
        }

        public int SpecialArtworkId
        {
            get;
            protected set;
        }

        public bool IsQuestGiver
        {
            get;
            protected set;
        }

        public bool Sex
        {
            get;
            protected set;
        }

        #region ISpawnEntry Members

        public int ContextualId
        {
            get { return (int) Id; }
            set { Id = value; }
        }

        public override GameRolePlayActorInformations ToNetworkActor(WorldClient client)
        {
            return new GameRolePlayNpcInformations(
                ContextualId,
                Look.EntityLook,
                GetEntityDisposition(),
                (uint) Template.Id,
                Sex,
                (uint) SpecialArtworkId,
                IsQuestGiver);
        }

        #endregion

        public void Interact(NpcActionTypeEnum actionTypeEnum, Character dialoger)
        {
            try
            {
                if (!Template.StartActions.ContainsKey(actionTypeEnum))
                    throw new NotImplementedException(string.Format("Npc action '{0}' is not implemented for npc '{1}",
                                                                    actionTypeEnum, Template.Id));

                Template.StartActions[actionTypeEnum].Execute(this, dialoger);
            }
            catch (Exception e)
            {
                logger.Error("Can't interact with npc '{0}' : {1}", Template.Id, e.Message);
            }
        }
    }
}