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

        [Property("French")]
        public string Fr
        {
            get;
            set;
        }

        [Property("English")]
        public string En
        {
            get;
            set;
        }

        [Property("German")]
        public string De
        {
            get;
            set;
        }

        [Property("Spanish")]
        public string Es
        {
            get;
            set;
        }

        [Property("Italian")]
        public string It
        {
            get;
            set;
        }

        [Property("Japanish")]
        public string Ja
        {
            get;
            set;
        }

        [Property("Dutsh")]
        public string Nl
        {
            get;
            set;
        }

        [Property("Portugese")]
        public string Pt
        {
            get;
            set;
        }

        [Property("Russish")]
        public string Ru
        {
            get;
            set;
        }
    }
}