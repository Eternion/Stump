using Castle.ActiveRecord;

namespace Stump.Server.BaseServer.Database.Interfaces
{
    public interface ITextUIRecord
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        uint Id
        {
            get;
            set;
        }

        [Property]
        string Name
        {
            get;
            set;
        }

        [Property("French")]
        string Fr
        {
            get;
            set;
        }

        [Property("English")]
        string En
        {
            get;
            set;
        }

        [Property("German")]
        string De
        {
            get;
            set;
        }

        [Property("Spanish")]
        string Es
        {
            get;
            set;
        }

        [Property("Italian")]
        string It
        {
            get;
            set;
        }

        [Property("Japanish")]
        string Ja
        {
            get;
            set;
        }

        [Property("Dutsh")]
        string Nl
        {
            get;
            set;
        }

        [Property("Portugese")]
        string Pt
        {
            get;
            set;
        }

        [Property("Russish")]
        string Ru
        {
            get;
            set;
        }

        void Save();
        void SaveAndFlush();
        object SaveCopy();
        object SaveCopyAndFlush();
        void Create();
        void CreateAndFlush();
        void Update();
        void UpdateAndFlush();
        void Delete();
        void DeleteAndFlush();
        void Refresh();
    }

}