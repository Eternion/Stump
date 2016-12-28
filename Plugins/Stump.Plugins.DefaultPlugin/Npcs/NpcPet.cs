using System;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Npcs;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player.Custom;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Plugins.DefaultPlugin.Npcs
{
    public class NpcPet
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static int NpcId = 297; // Oshimo 

        [Initialization(typeof(NpcManager), Silent = true)]
        public static void Initialize()
        {
            var npc = NpcManager.Instance.GetNpcTemplate(NpcId);

            if (npc == null)
            {
                logger.Error($"NPC {NpcId} not found");
                return;
            }

            npc.NpcSpawned += OnNpcSpawned;
        }

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            npc.Actions.Add(new NpcPetExchangeAction());
            npc.Actions.Add(new NpcPetDropCollectAction());
        }
    }

    public class NpcPetExchangeAction : NpcAction
    {
        public override NpcActionTypeEnum[] ActionType => new[] {NpcActionTypeEnum.ACTION_EXCHANGE};

        public override void Execute(Npc npc, Character character)
        {
            var trade = new NpcPetExchange(character, npc);
            trade.Open();
        }
    }

    public class NpcPetExchange : NpcTrade
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NpcPetExchange(Character character, Npc npc)
            : base(character, npc)
        {
        }

        public override ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.NPC_RESURECT_PET;

        protected override void OnTraderItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            base.OnTraderItemMoved(trader, item, modified, difference);

            if (!(trader is PlayerTrader))
                return;

            AdjustLoots();
        }

        public void AdjustLoots()
        {
            SecondTrader.Clear();

            if (FirstTrader.Items.Count != 2)
                return;

            var eniripsaPowder = FirstTrader.Items.FirstOrDefault(x => x.Template.Id == (int) ItemIdEnum.POUDRE_DENIRIPSA_2239)?.Stack ?? 0;
            var resurectionPowder = FirstTrader.Items.FirstOrDefault(x => x.Template.Id == (int) ItemIdEnum.POUDRE_DE_RESURRECTION_8012)?.Stack ?? 0;

            if ((eniripsaPowder == 0 && resurectionPowder == 0) || (eniripsaPowder != 0 && resurectionPowder != 0))
                return;

             var ghost = FirstTrader.Items.OfType<PlayerTradeItem>().FirstOrDefault(x => x.Template.Type.ItemType == ItemTypeEnum.FANTÔME_DE_FAMILIER ||
                                                                                            x.Template.Type.ItemType == ItemTypeEnum.FANTÔME_DE_MONTILIER);
            var pet =  FirstTrader.Items.OfType<PlayerTradeItem>().FirstOrDefault(x => x.Template.Type.ItemType == ItemTypeEnum.FAMILIER ||
                                                                                            x.Template.Type.ItemType == ItemTypeEnum.MONTILIER);
                
            if (ghost != null)
            {
                var petTemplate = PetManager.Instance.Pets.Values.FirstOrDefault(x => x.GhostItemId == ghost.Template.Id);

                if (petTemplate == null)
                {
                    logger.Error($"Ghost {ghost.Template.Id} has no matching pet");
                    return;
                }

                var itemTemplate = ItemManager.Instance.TryGetTemplate(petTemplate.Id);

                if (itemTemplate == null)
                {
                    logger.Error($"Pet {petTemplate.Id} is no valid item !");
                    return;
                }

                var effects = ghost.Effects.Clone();
                var hpEffect = effects.OfType<EffectInteger>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LifePoints);

                if (hpEffect == null)
                    effects.Add(hpEffect = new EffectInteger(EffectsEnum.Effect_LifePoints, 0));

                var maxHp = itemTemplate.Effects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LifePoints)?.Value ?? 0;
                if (eniripsaPowder > 0)
                {
                    if (petTemplate.PossibleEffects.Count > 0)
                        effects.RemoveAll(x => x != hpEffect && x.EffectId != EffectsEnum.Effect_MealCount);

                    hpEffect.Value = (short)Math.Min(maxHp, eniripsaPowder);
                }
                else
                    hpEffect.Value = maxHp;

                SecondTrader.AddItem(itemTemplate, 1, effects);
            }
            else if (pet != null && eniripsaPowder > 0)
            {
                var petItem = pet.PlayerItem as PetItem;

                if (petItem == null)
                    return;
                
                var effects = pet.Effects.Clone();
                var hpEffect = effects.OfType<EffectInteger>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LifePoints);

                if (hpEffect == null)
                    return;

                hpEffect.Value += (short)eniripsaPowder;

                if (hpEffect.Value > petItem.MaxLifePoints)
                    hpEffect.Value = (short)petItem.MaxLifePoints;

                SecondTrader.AddItem(petItem.Template, 1, effects);
            }
        }

        protected override void Apply()
        {
            foreach (var tradeItem in FirstTrader.Items)
            {
                var item = FirstTrader.Character.Inventory.TryGetItem(tradeItem.Guid);
                FirstTrader.Character.Inventory.RemoveItem(item, (int)tradeItem.Stack);
            }

            foreach (var tradeItem in SecondTrader.Items)
            {
                FirstTrader.Character.Inventory.AddItem(tradeItem.Template, tradeItem.Effects, (int)tradeItem.Stack);
            }
        }
    }

    public class NpcPetDropCollectAction : NpcAction
    {
        public override NpcActionTypeEnum[] ActionType => new[] {NpcActionTypeEnum.ACTION_DROP_COLLECT_PET};

        public override void Execute(Npc npc, Character character)
        {
            var trade = new NpcPetDropCollectTrade(character, npc);
            trade.Open();
        }
    }

    public class NpcPetDropCollectTrade : NpcTrade
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NpcPetDropCollectTrade(Character character, Npc npc)
            : base(character, npc)
        {
        }

        protected override void OnTraderKamasChanged(Trader trader, uint amount)
        {
           
        }

        protected override void OnTraderItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            base.OnTraderItemMoved(trader, item, modified, difference);

            if (!(trader is PlayerTrader))
                return;

            AdjustLoots();
        }

        public void AdjustLoots()
        {
            SecondTrader.Clear();
            Kamas = 0;

            if (FirstTrader.Items.Count != 1 || FirstTrader.Items.Any(x => x.Stack != 1))
                return;

            var pet = FirstTrader.Items.OfType<PlayerTradeItem>().FirstOrDefault(x => x.PlayerItem is PetItem)?.PlayerItem as PetItem;

            if (pet != null)
            {
                Kamas = (int)((DateTime.Now - pet.LastMealDate)?.TotalHours ?? 0);

                if (FirstTrader.Character.Inventory.Kamas < Kamas)
                {
                    //Le montant de la transaction s'élève à %1 kamas. Malheureusement vous n'avez actuellement pas cette somme sur vous.
                    FirstTrader.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 128, Kamas);
                    return;
                }

                var certificateTemplate = ItemManager.Instance.TryGetTemplate(pet.PetTemplate.CertificateItemId.Value);

                if (certificateTemplate == null)
                    return;

                var effects = pet.Effects.Clone();

                var lastMealEffect = effects.OfType<EffectDate>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LastMealDate);

                if (lastMealEffect == null)
                    effects.Add(new EffectDate(EffectsEnum.Effect_LastMealDate, DateTime.Now));
                else
                    lastMealEffect.SetDate(DateTime.Now);

                SecondTrader.AddItem(certificateTemplate, 1, effects);
                InventoryHandler.SendExchangeKamaModifiedMessage(FirstTrader.Character.Client, false, Kamas);
            }
            else
            {
                var certificate = FirstTrader.Items.OfType<PlayerTradeItem>().FirstOrDefault(x => 
                    x.Template.Type.ItemType == ItemTypeEnum.CERTIFICAT_DE_FAMILIER || 
                    x.Template.Type.ItemType == ItemTypeEnum.CERTIFICAT_DE_MONTILIER);

                if (certificate == null)
                    return;

                var lastMealEffect = certificate.Effects.OfType<EffectDate>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LastMealDate)?.GetDate() ?? DateTime.Now;

                Kamas = (int)(DateTime.Now - lastMealEffect).TotalHours;

                if (FirstTrader.Character.Inventory.Kamas < Kamas)
                {
                    //Le montant de la transaction s'élève à %1 kamas. Malheureusement vous n'avez actuellement pas cette somme sur vous.
                    FirstTrader.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 128, Kamas);
                    return;
                }
                
                var petTemplate = PetManager.Instance.Pets.Values.FirstOrDefault(x => x.CertificateItemId == certificate.Template.Id);
                
                if (petTemplate == null)
                {
                    logger.Error($"Certificate {certificate.Template.Id} has no matching pet");
                    return;
                }

                var itemTemplate = ItemManager.Instance.TryGetTemplate(petTemplate.Id);

                if (itemTemplate == null)
                {
                    logger.Error($"Pet {petTemplate.Id} is no valid item !");
                    return;
                }

                SecondTrader.AddItem(itemTemplate, 1, certificate.Effects);
                InventoryHandler.SendExchangeKamaModifiedMessage(FirstTrader.Character.Client, false, Kamas);
            }
        }

        public int Kamas
        {
            get;
            private set;
        }

        protected override void Apply()
        {
            foreach (var tradeItem in FirstTrader.Items)
            {
                var item = FirstTrader.Character.Inventory.TryGetItem(tradeItem.Guid);
                FirstTrader.Character.Inventory.RemoveItem(item, (int)tradeItem.Stack);
            }

            foreach (var tradeItem in SecondTrader.Items)
            {
                FirstTrader.Character.Inventory.AddItem(tradeItem.Template, tradeItem.Effects, (int)tradeItem.Stack);
            }

            FirstTrader.Character.Inventory.SubKamas(Kamas);
        }
    }
}