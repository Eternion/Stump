#region License GNU GPL
// PrestigeManager.cs
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
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Characters
{
    public class PrestigeManager : Singleton<PrestigeManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public const int ItemForBonus = 20214;

        public static ItemTemplate BonusItem;

        [Variable] public static short[] PrestigeTitles =
        {
            200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214
        };

        private static readonly EffectInteger[][] m_prestigesBonus =
        {
            new[] {new EffectInteger(EffectsEnum.Effect_AddDamageBonus, 2)},
            new[] {new EffectInteger(EffectsEnum.Effect_AddVitality, 25)},
            new[]
            {
                new EffectInteger(EffectsEnum.Effect_AddChance, 20),
                new EffectInteger(EffectsEnum.Effect_AddIntelligence, 20),
                new EffectInteger(EffectsEnum.Effect_AddWisdom, 20),
                new EffectInteger(EffectsEnum.Effect_AddAgility, 20),
                new EffectInteger(EffectsEnum.Effect_AddStrength, 20)
            },
            new[] {new EffectInteger(EffectsEnum.Effect_AddDamageBonus, 3)},
            new[] {new EffectInteger(EffectsEnum.Effect_AddVitality, 50)},
            new[]
            {
                new EffectInteger(EffectsEnum.Effect_AddChance, 30),
                new EffectInteger(EffectsEnum.Effect_AddIntelligence, 30),
                new EffectInteger(EffectsEnum.Effect_AddWisdom, 30),
                new EffectInteger(EffectsEnum.Effect_AddAgility, 30),
                new EffectInteger(EffectsEnum.Effect_AddStrength, 30)
            },
            new[] {new EffectInteger(EffectsEnum.Effect_AddDamageBonus, 5)},
            new[] {new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, 20)},
            new[] {new EffectInteger(EffectsEnum.Effect_AddDamageBonus, 10)},
            new[]
            {
                new EffectInteger(EffectsEnum.Effect_AddAirElementReduction, 10),
                new EffectInteger(EffectsEnum.Effect_AddEarthElementReduction, 10),
                new EffectInteger(EffectsEnum.Effect_AddFireElementReduction, 10),
                new EffectInteger(EffectsEnum.Effect_AddWaterElementReduction, 10),
                new EffectInteger(EffectsEnum.Effect_AddNeutralElementReduction, 10)
            },
            new[] {new EffectInteger(EffectsEnum.Effect_IncreaseDamage_138, 20)},
            new[] {new EffectInteger(EffectsEnum.Effect_AddVitality, 50)},
            new[] {new EffectInteger(EffectsEnum.Effect_AddCriticalHit, 7)},
            new[] {new EffectInteger(EffectsEnum.Effect_AddRange, 1)},
            new[] {new EffectInteger(EffectsEnum.Effect_AddMP_128, 1)},

        };

        private bool m_disabled;

        public bool PrestigeEnabled
        {
            get { return !m_disabled; }
        }

        [Initialization(typeof (ItemManager), Silent = true)]
        public void Initialize()
        {
            BonusItem = ItemManager.Instance.TryGetTemplate(ItemForBonus);

            if (BonusItem != null)
                return;

            logger.Error("Item {0} not found, prestiges disabled", ItemForBonus);
            m_disabled = true;
        }

        public EffectInteger[] GetPrestigeEffects(int rank)
        {
            return m_prestigesBonus.Take(rank).SelectMany(x => x.Select(y => (EffectInteger) y.Clone())).ToArray();
        }

        public short GetPrestigeTitle(int rank)
        {
            return PrestigeTitles[rank - 1];
        }
    }
}