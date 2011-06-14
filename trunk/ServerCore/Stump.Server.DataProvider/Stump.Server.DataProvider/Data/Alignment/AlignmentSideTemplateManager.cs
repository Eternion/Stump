
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Alignment
{
    //public class AlignmentSideTemplateManager : DataManager<int,AlignmentSide>
    //{
    //    protected override AlignmentSide InternalGetOne(int id)
    //    {
            
    //        // ROFL FLEMME
    //        return null;
    //    }

    //    protected override Dictionary<int, AlignmentSide> InternalGetAll()
    //    {
    //        var sides = D2OLoader.LoadData<DofusProtocol.D2oClasses.AlignmentSide>();
    //        var orders = D2OLoader.LoadData<DofusProtocol.D2oClasses.AlignmentOrder>();
    //        var ranks = D2OLoader.LoadData<DofusProtocol.D2oClasses.AlignmentRank>();
    //        var rankGifts = D2OLoader.LoadData<AlignmentRankJntGift>();
    //        var gifts = D2OLoader.LoadData<DofusProtocol.D2oClasses.AlignmentGift>();
    //        var effects = D2OLoader.LoadData<AlignmentEffect>();
    //        var sideTemplates = new Dictionary<int, AlignmentSide>(sides.Count());

    //        foreach (var sideData in sides)
    //        {
    //            var side = new AlignmentSide { Id = sideData.id, CanConquest = sideData.canConquest };
    //            foreach (var orderData in orders)
    //            {
    //                var order = new AlignmentOrder { AlignmentSide = side, Id = orderData.id };
    //                foreach (var rankData in ranks)
    //                {
    //                    var rank = new AlignmentRank
    //                    {
    //                        Id = rankData.id,
    //                        MinLevelAlignment = rankData.minimumAlignment,
    //                        AlignmentOrder = order
    //                    };
    //                    var rankGift = rankGifts.Single(rg => rg.id == rank.Id);
    //                    for (int i = 0; i < rankGift.gifts.Count; i++)
    //                    {
    //                        var effectId = gifts.First(g => g.id == rankGift.gifts[i]).effectId;
    //                        var effect = effects.FirstOrDefault(e => e.id == effectId);
    //                        var charactId = effect != null ? effect.characteristicId : 0;
    //                        rank.Gifts.Add(new AlignmentGift
    //                        {
    //                            Id = rankGift.id,
    //                            EffectId = effectId,
    //                            CharacteristicId = (int)charactId,
    //                            Value = rankGift.parameters[i]
    //                        });
    //                    }
    //                }
    //            }
    //            sideTemplates.Add(side.Id,side);
    //        }
    //        return sideTemplates; 
    //    }
    //}
}