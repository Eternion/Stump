using System;
using Castle.ActiveRecord;

namespace Stump.Server.WorldServer.Database.World
{
    [ActiveRecord("maps_fight_positions")]
    public class MapFightPositionsRecord : WorldBaseRecord<MapFightPositionsRecord>
    {
        private short[] m_blueCells;
        private short[] m_redCells;

        [Field("BlueCells")]
        private byte[] m_rawBlueCells;

        [Field("RedCells")]
        private byte[] m_rawRedCells;

        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [OneToOne]
        public MapRecord Map
        {
            get;
            set;
        }

        public short[] BlueCells
        {
            get { return m_blueCells ?? (m_blueCells = Deserialize(m_rawBlueCells)); }
            set
            {
                m_blueCells = value;
                m_rawBlueCells = Serialize(value);
            }
        }

        public short[] RedCells
        {
            get { return m_redCells ?? (m_redCells = Deserialize(m_rawRedCells)); }
            set
            {
                m_redCells = value;
                m_rawRedCells = Serialize(value);
            }
        }

        private static byte[] Serialize(short[] cells)
        {
            var bytes = new byte[cells.Length*2];

            for (int i = 0; i < cells.Length; i++)
            {
                bytes[0] = (byte) (cells[i] & 0xFF00);
                bytes[1] = (byte) (cells[i] & 0xFF);
            }

            return bytes;
        }

        private static short[] Deserialize(byte[] bytes)
        {
            if ((bytes.Length%2) != 0)
                throw new ArgumentException("bytes.Length % 2 != 0");

            var cells = new short[bytes.Length/2];

            for (int i = 0, j = 0; i < bytes.Length; i += 2, j++)
                cells[j] = (short) (bytes[i] << 8 | bytes[i + 1]);

            return cells;
        }
    }
}