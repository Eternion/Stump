#region License GNU GPL
// MerchantBagOffline.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants;

namespace Stump.Server.WorldServer.Game.Items
{
    public class MerchantBag : ItemsCollection<MerchantItem>
    {
        public MerchantBag(Merchant owner)
        {
            Owner = owner;
        }

        public MerchantBag(Merchant owner, CharacterMerchantBag merchantBag)
        {
            Owner = owner;
            Items = merchantBag.ToDictionary(entry => entry.Guid); // just to copy properly
        }

        public Merchant Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Must be saved 
        /// </summary>
        public bool IsDirty
        {
            get;
            set;
        }

        public void LoadRecord()
        {
            var records = ItemManager.Instance.FindPlayerMerchantItems(Owner.Id);
            Items = records.Select(entry => new MerchantItem(entry)).ToDictionary(entry => entry.Guid);
        }

        // todo : move it in the dialog class
        /*public bool BuyItem(Character buyer, MerchantItem item, uint quantity)
        {
            if (quantity == 0)
                return false;

            RemoveItem(item, quantity);
            IsDirty = true;

            PlayerItem newItem = ItemManager.Instance.CreatePlayerItem(buyer, item.Template, quantity,
                                                            item.Effects);

            buyer.Inventory.AddItem(newItem);
            return true;
        }*/

        public override void Save()
        {
            base.Save();

            IsDirty = false;
        }
    }
}