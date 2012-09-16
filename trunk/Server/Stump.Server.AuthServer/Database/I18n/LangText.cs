using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Database.Interfaces;

namespace Stump.Server.AuthServer.Database
{
    public class LangTextConfiguration : EntityTypeConfiguration<LangText>
    {
        public LangTextConfiguration()
        {
            ToTable("langs");   
        }
    }

    public partial class LangText
    {        
        // Primitive properties

        public int Id
        {
            get;
            set;
        }
        public string French
        {
            get;
            set;
        }
        public string English
        {
            get;
            set;
        }
        public string German
        {
            get;
            set;
        }
        public string Spanish
        {
            get;
            set;
        }
        public string Italian
        {
            get;
            set;
        }
        public string Japanish
        {
            get;
            set;
        }
        public string Dutsh
        {
            get;
            set;
        }
        public string Portugese
        {
            get;
            set;
        }
        public string Russish
        {
            get;
            set;
        }
    }
}