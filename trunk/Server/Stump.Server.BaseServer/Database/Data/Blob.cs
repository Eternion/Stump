using System;
using System.Runtime.Serialization;

namespace Stump.Server.BaseServer.Database.Data
{
    /// <summary>
    /// Binary large object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Blob<T> where T : IRawSerializable<T>, new()
    {
        protected T m_value;

        public virtual T Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
                m_data = m_value.Serialize();
            }
        }

        protected byte[] m_data;

        public virtual byte[] Data
        {
            get { return m_data; }
            set
            {
                m_data = value;
                m_value = new T();
                m_value.Deserialize(m_data);
            }
        }
    }
}