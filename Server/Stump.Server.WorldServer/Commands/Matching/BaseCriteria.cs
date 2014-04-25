namespace Stump.Server.WorldServer.Commands.Matching
{
    public abstract class BaseCriteria<T>
    {
        public BaseCriteria(BaseMatching<T> matching, string pattern)
        {
            Matching = matching;
            Pattern = pattern;
        }

        public BaseMatching<T> Matching
        {
            get;
            protected set;
        }

        public string Pattern
        {
            get;
            protected set;
        }

        public abstract T[] GetMatchings();
    }
}