using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;

namespace ArkalysPlugin
{
    public static class NpcRestatTrigger
    {
        private static ItemTemplate m_orbeTemplate;

        [Initialization(typeof(NpcManager), Silent = true)]
        public static void Initialize()
        {
            var npc = NpcManager.Instance.GetNpcTemplate(1223); // fée risette
            npc.NpcSpawned += OnNpcSpawned;
        }

        [Initialization(typeof(ItemManager), Silent = true)]
        public static void InitializeItem()
        {
            m_orbeTemplate = ItemManager.Instance.TryGetTemplate(10563); // orbe reconstituant
            m_orbeTemplate.IsLinkedToOwner = true;
        }

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            npc.Interacted += OnInteracted;
        }

        private static void OnInteracted(Npc npc, NpcActionTypeEnum actiontype, NpcAction action, Character character)
        {
            if (actiontype == NpcActionTypeEnum.ACTION_TALK)
            {
                character.SendServerMessage("La fée risette vous permet de remettre à 0 vos caractéristiques en échange d'un '<b>Orbe reconstituant</b>' disponible à la boutique.", Color.BlueViolet);

                var item = character.Inventory.TryGetItem(m_orbeTemplate);
                var count = item == null ? 0 : item.Stack;

                character.SendServerMessage(string.Format("Vous possédez <b>{0}</b> 'Orbe reconsituant'", count), Color.BlueViolet);

                if (count == 0)
                    character.SendServerMessage("Vous n'avez plus d'orbe !", Color.Red);
            }
        }
    }
}