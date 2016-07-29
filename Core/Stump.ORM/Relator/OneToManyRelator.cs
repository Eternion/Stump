namespace Stump.ORM.Relator
{
    public class OneToManyRelator<T1, T2>
        where T1 : class, IOneToManyRecord1 <T2>
        where T2 : IJoined
    {
        private T1 m_current;

        public T1 Map(T1 one, T2 many)
        {
            if (one == null)
                return m_current;

            if (m_current != null && many.JoinedId == m_current.JoinedId)
            {
                m_current.ManyProperty1.Add(many);
                return null;
            }

            var previous = m_current;
            m_current = one;
            if (many.JoinedId == m_current.JoinedId)
                m_current.ManyProperty1.Add(many);

            return previous;
        }
    }
}