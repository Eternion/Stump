
namespace Stump.Server.DataProvider.Data.Map
{
    public class MapCapabilities
    {

        public MapCapabilities(int capabilities)
        {
            m_capabilities = capabilities;
        }

        private readonly int m_capabilities;

        public bool AllowChallenge
        {
            get { return (m_capabilities & 1) != 0; }
        }

        public bool AllowAggression
        {
            get { return (m_capabilities & 2) != 0; }
        }

        public bool AllowTeleportTo
        {
            get { return (m_capabilities & 4) != 0; }
        }

        public bool AllowTeleportFrom
        {
            get { return (m_capabilities & 8) != 0; }
        }

        public bool AllowExchangeBetweenPlayers
        {
            get { return (m_capabilities & 16) != 0; }
        }

        public bool AllowHumanVendor
        {
            get { return (m_capabilities & 32) != 0; }
        }

        public bool AllowCollector
        {
            get { return (m_capabilities & 64) != 0; }
        }

        public bool AllowSoulCapture
        {
            get { return (m_capabilities & 128) != 0; }
        }

        public bool AllowSoulSummon
        {
            get { return (m_capabilities & 256) != 0; }
        }

        public bool AllowTavernRegen
        {
            get { return (m_capabilities & 512) != 0; }
        }

        public bool AllowTombMode
        {
            get { return (m_capabilities & 1024) != 0; }
        }

        public bool TeleportEverywhere
        {
            get { return (m_capabilities & 2048) != 0; }
        }

        public bool AllowFightChallenge
        {
            get { return (m_capabilities & 4096) != 0; }
        }

    }
}