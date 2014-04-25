namespace Stump.DofusProtocol.Types.Custom
{
    public class ExtendedLook : EntityLook
    {
        // TODO : remake it

        /*#region Fields

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

    #endregion

    public ExtendedLook()
    {
        m_entityLook = new EntityLook();
        m_characterSkins = new List<uint>();
        m_itemSkins = new Dictionary<CharacterInventoryPositionEnum, uint>();
    }

    public ExtendedLook(EntityLook baseLook)
        : base(baseLook.bonesId, baseLook.skins, baseLook.indexedColors, baseLook.scales, baseLook.subentities)
    {
        m_characterSkins = baseLook.skins;
        m_itemSkins = new Dictionary<CharacterInventoryPositionEnum, uint>();
    }

    public ExtendedLook(ExtendedLook extendedLook)
        : this(extendedLook)
    {
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
        */
    }
}