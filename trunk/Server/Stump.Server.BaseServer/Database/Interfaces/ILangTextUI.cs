using Castle.ActiveRecord;

namespace Stump.Server.BaseServer.Database.Interfaces
{
    public interface ILangTextUI
    {
        uint Id
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }

        string French
        {
            get;
            set;
        }

        string English
        {
            get;
            set;
        }

        string German
        {
            get;
            set;
        }

        string Spanish
        {
            get;
            set;
        }

        string Italian
        {
            get;
            set;
        }

        string Japanish
        {
            get;
            set;
        }

        string Dutsh
        {
            get;
            set;
        }

        string Portugese
        {
            get;
            set;
        }

        string Russish
        {
            get;
            set;
        }
    }

}