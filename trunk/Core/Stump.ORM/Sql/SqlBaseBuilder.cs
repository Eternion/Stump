namespace Stump.ORM.Sql
{
    public abstract class SqlBaseBuilder
    {
        public const char QuoteChar = '`';

        public abstract SqlString ToSqlString();

        protected string Quote(string str)
        {
            if (IsQuoted(str))
                return str;

            return QuoteChar + str + QuoteChar;
        }

        protected string UnQuote(string str)
        {
            if (IsQuoted(str))
                return str.Substring(1, str.Length - 2);

            return str;
        }

        protected bool IsQuoted(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            return str[0] == QuoteChar && str.Length > 1 && str[str.Length - 1] == QuoteChar;
        }
    }
}