using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Classes;

namespace Stump.DofusProtocol.Classes.Custom
{
    public class ExtendedLook
    {

        #region Fields

        private uint m_bonesId;
        public uint BonesId
        {
            get { return m_bonesId; }
            set
            {
                m_bonesId = value;
                m_updated = true;
            }
        }

        private List<uint> m_characterSkins;
        public List<uint> CharacterSkins
        {
            get { return m_characterSkins; }
            set
            {
                m_characterSkins = value;
                m_updated = true;
            }
        }

        private Dictionary<CharacterInventoryPositionEnum, uint> m_itemSkins;
        public Dictionary<CharacterInventoryPositionEnum, uint> Skins
        {
            get { return m_itemSkins; }
            set
            {
                m_itemSkins = value;
                m_updated = true;
            }
        }

        private List<int> m_colors;
        public List<int> Colors
        {
            get { return m_colors; }
            set
            {
                m_colors = value;
                m_updated = true;
            }
        }

        private List<int> m_scales;
        public List<int> Scales
        {
            get { return m_scales; }
            set
            {
                m_scales = value;
                m_updated = true;
            }
        }

        private List<SubEntity> m_subentities;
        public List<SubEntity> Subentities
        {
            get { return m_subentities; }
            set
            {
                m_subentities = value;
                m_updated = true;
            }
        }

        private bool m_updated = true;

        private EntityLook m_entityLook;
        public EntityLook EntityLook
        {
            get
            {
                if (m_updated)
                    UpdateEntityLook();
                return m_entityLook;
            }
        }

        #endregion

        #region Ctors

        public ExtendedLook()
        {
            m_bonesId = 0;
            m_characterSkins = new List<uint>(2);
            m_itemSkins = new Dictionary<CharacterInventoryPositionEnum, uint>();
            m_colors = new List<int>(5);
            m_scales = new List<int>(2);
            m_subentities = new List<SubEntity>();
        }

        public ExtendedLook(EntityLook baseLook)
        {
            m_bonesId = baseLook.bonesId;
            m_characterSkins = baseLook.skins;
            m_itemSkins = new Dictionary<CharacterInventoryPositionEnum, uint>();
            m_colors = baseLook.indexedColors;
            m_scales = baseLook.scales;
            m_subentities = baseLook.subentities;
        }

        public ExtendedLook(uint bonesId, List<uint> characterSkins, Dictionary<CharacterInventoryPositionEnum,uint> itemSkins, List<int> colors, List<int> scales, List<SubEntity> subEntities)
        {
            m_bonesId = bonesId;
            m_characterSkins = characterSkins;
            m_itemSkins = itemSkins;
            m_colors = colors;
            m_scales = scales;
            m_subentities = subEntities;
        }

        #endregion

        #region Update Look

        private void SetItemLook(uint look, CharacterInventoryPositionEnum position)
        {
            if (m_itemSkins.ContainsKey(position))
                m_itemSkins[position] = look;
            else
                m_itemSkins.Add(position, look);
            m_updated = true;
        }

        private void UpdateEntityLook()
        {
            m_entityLook = new EntityLook(m_bonesId,m_characterSkins.Concat(m_itemSkins.Values).ToList() , m_colors, m_scales, m_subentities);
            m_updated = false;
            OnUpdateEntityLook();
        }

        private void OnUpdateEntityLook()
        {
        }

        #endregion

    }
}
