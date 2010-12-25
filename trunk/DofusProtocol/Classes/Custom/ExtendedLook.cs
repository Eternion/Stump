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

        public uint BonesId
        {
            get { return m_entityLook.bonesId; }
            set { m_entityLook.bonesId = value; }
        }

        private List<uint> m_characterSkins;
        public List<uint> CharacterSkins
        {
            get { return m_characterSkins; }
            set
            {
                m_characterSkins = value;
                UpdateEntityLookSkins();
            }
        }

        private Dictionary<CharacterInventoryPositionEnum, uint> m_itemSkins;
        public Dictionary<CharacterInventoryPositionEnum, uint> ItemSkins
        {
            get { return m_itemSkins; }
            set
            {
                m_itemSkins = value;
                UpdateEntityLookSkins();
            }
        }

        public List<int> Colors
        {
            get { return m_entityLook.indexedColors; }
            set { m_entityLook.indexedColors = value; }     
           
        }

        public List<int> Scales
        {
            get { return m_entityLook.scales; }
            set { m_entityLook.scales = value; }       
        }

        public List<SubEntity> Subentities
        {
            get { return m_entityLook.subentities; }
            set { m_entityLook.subentities = value; }
        }

        private EntityLook m_entityLook;
        public EntityLook EntityLook
        {
            get{ return m_entityLook;}
        }
            
        #endregion

        public ExtendedLook()
        {   
            m_entityLook = new EntityLook();
            m_characterSkins = new List<uint>();
            m_itemSkins = new Dictionary<CharacterInventoryPositionEnum, uint>();
        }

        public ExtendedLook(EntityLook baseLook)
        {
            m_entityLook = baseLook;
            m_characterSkins = baseLook.skins;
            m_itemSkins = new Dictionary<CharacterInventoryPositionEnum, uint>();
        }

        public ExtendedLook(ExtendedLook extendedLook)
        {
            m_entityLook = extendedLook.EntityLook;
            m_characterSkins = extendedLook.CharacterSkins;
            m_itemSkins = extendedLook.ItemSkins;
        }

        public void SetItemLook(uint look, CharacterInventoryPositionEnum position)
        {
            if (m_itemSkins.ContainsKey(position))
                m_itemSkins[position] = look;
            else
                m_itemSkins.Add(position, look);
            UpdateEntityLookSkins();
        }

        public void UpdateEntityLookSkins()
        {
            m_entityLook.skins = m_characterSkins.Concat(m_itemSkins.Values).ToList();
        }

    }
}
