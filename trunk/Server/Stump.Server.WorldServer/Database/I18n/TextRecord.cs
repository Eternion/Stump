using Castle.ActiveRecord;
using Stump.Server.BaseServer.Database.Interfaces;

namespace Stump.Server.WorldServer.Database.I18n
{
    [ActiveRecord("texts")]
    public class TextRecord : WorldBaseRecord<TextRecord>, ITextRecord
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("French", ColumnType = "StringClob", SqlType = "MediumText")]
        public string French
        {
            get;
            set;
        }

        [Property("English", ColumnType = "StringClob", SqlType = "MediumText")]
        public string English
        {
            get;
            set;
        }

        [Property("German", ColumnType = "StringClob", SqlType = "MediumText")]
        public string German
        {
            get;
            set;
        }

        [Property("Spanish", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Spanish
        {
            get;
            set;
        }

        [Property("Italian", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Italian
        {
            get;
            set;
        }

        [Property("Japanish", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Japanish
        {
            get;
            set;
        }

        [Property("Dutsh", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Dutsh
        {
            get;
            set;
        }

        [Property("Portugese", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Portugese
        {
            get;
            set;
        }

        [Property("Russish", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Russish
        {
            get;
            set;
        }
    }
}