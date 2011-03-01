// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.DataProvider.Core;
using Stump.Server.DataProvider.Data.D2oTool;

namespace Stump.Server.DataProvider.Data.Alignment
{
    public class AlignmentSideTemplateProvider : DataProvider<int,AlignmentSide>
    {
        protected override AlignmentSide GetData(int id)
        {
            // ROFL FLEMME
            return null;
        }

        protected override Dictionary<int, AlignmentSide> GetAllData()
        {
            var sides = D2OLoader.LoadData<DofusProtocol.D2oClasses.AlignmentSide>();
            var orders = D2OLoader.LoadData<DofusProtocol.D2oClasses.AlignmentOrder>();
            var ranks = D2OLoader.LoadData<DofusProtocol.D2oClasses.AlignmentRank>();
            var rankGifts = D2OLoader.LoadData<AlignmentRankJntGift>();
            var gifts = D2OLoader.LoadData<DofusProtocol.D2oClasses.AlignmentGift>();
            var effects = D2OLoader.LoadData<AlignmentEffect>();
            var sideTemplates = new Dictionary<int, AlignmentSide>(sides.Count());

            foreach (var sideData in sides)
            {
                var side = new AlignmentSide { Id = sideData.id, CanConquest = sideData.canConquest };
                foreach (var orderData in orders)
                {
                    var order = new AlignmentOrder { AlignmentSide = side, Id = orderData.id };
                    foreach (var rankData in ranks)
                    {
                        var rank = new AlignmentRank
                        {
                            Id = rankData.id,
                            MinLevelAlignment = rankData.minimumAlignment,
                            AlignmentOrder = order
                        };
                        var rankGift = rankGifts.Single(rg => rg.id == rank.Id);
                        for (int i = 0; i < rankGift.gifts.Count; i++)
                        {
                            var effectId = gifts.First(g => g.id == rankGift.gifts[i]).effectId;
                            var effect = effects.FirstOrDefault(e => e.id == effectId);
                            var charactId = effect != null ? effect.characteristicId : 0;
                            rank.Gifts.Add(new AlignmentGift
                            {
                                Id = rankGift.id,
                                EffectId = effectId,
                                CharacteristicId = (int)charactId,
                                Value = rankGift.parameters[i]
                            });
                        }
                    }
                }
                sideTemplates.Add(side.Id,side);
            }
            return sideTemplates; 
        }
    }
}