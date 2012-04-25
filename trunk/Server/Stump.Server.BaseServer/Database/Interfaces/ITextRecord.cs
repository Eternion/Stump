using Castle.ActiveRecord;

namespace Stump.Server.BaseServer.Database.Interfaces
{
    public interface ITextRecord
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        uint Id
        {
            get;
            set;
        }

        [Property("French")]
        string French
        {
            get;
            set;
        }

        [Property("English")]
        string English
        {
            get;
            set;
        }

        [Property("German")]
        string German
        {
            get;
            set;
        }

        [Property("Spanish")]
        string Spanish
        {
            get;
            set;
        }

        [Property("Italian")]
        string Italian
        {
            get;
            set;
        }

        [Property("Japanish")]
        string Japanish
        {
            get;
            set;
        }

        [Property("Dutsh")]
        string Dutsh
        {
            get;
            set;
        }

        [Property("Portugese")]
        string Portugese
        {
            get;
            set;
        }

        [Property("Russish")]
        string Russish
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