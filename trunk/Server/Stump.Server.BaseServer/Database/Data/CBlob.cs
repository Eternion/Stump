using Stump.Core.IO;

namespace Stump.Server.BaseServer.Database.Data
{
    /// <summary>
    /// Compressed binary large object
    /// </summary>
    public class CBlob<T> : Blob<T>
        where T : IRawSerializable<T>, new()
    {
        public override byte[] Data
        {
            get { return base.Data; }
            set
            {
                m_data = value;
                var rawData = ZipHelper.Uncompress(m_data);

                m_value = new T();
                m_value.Deserialize(rawData);
            }
        }

        public override T Value
        {
            get { return base.Value; }
            set
            {
                m_value = value;
                m_data = ZipHelper.Compress(m_value.Serialize());
            }
        }
    }
}