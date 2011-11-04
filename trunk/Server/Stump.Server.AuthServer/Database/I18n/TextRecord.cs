using Castle.ActiveRecord;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Database.Interfaces;

namespace Stump.Server.AuthServer.Database.I18n
{
    [ActiveRecord("texts")]
    public class TextRecord : AuthBaseRecord<TextRecord>, ITextRecord
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("French", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Fr
        {
            get;
            set;
        }

        [Property("English", ColumnType = "StringClob", SqlType = "MediumText")]
        public string En
        {
            get;
            set;
        }

        [Property("German", ColumnType = "StringClob", SqlType = "MediumText")]
        public string De
        {
            get;
            set;
        }

        [Property("Spanish", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Es
        {
            get;
            set;
        }

        [Property("Italian", ColumnType = "StringClob", SqlType = "MediumText")]
        public string It
        {
            get;
            set;
        }

        [Property("Japanish", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Ja
        {
            get;
            set;
        }

        [Property("Dutsh", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Nl
        {
            get;
            set;
        }

        [Property("Portugese", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Pt
        {
            get;
            set;
        }

        [Property("Russish", ColumnType = "StringClob", SqlType = "MediumText")]
        public string Ru
        {
            get;
            set;
        }
    }
}