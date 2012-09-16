using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.Server.BaseServer.Database.Interfaces;

namespace Stump.Server.WorldServer.Database
{
    public class LangTextUiConfiguration : EntityTypeConfiguration<LangTextUi>
    {
        public LangTextUiConfiguration()
        {
            ToTable("langs_ui");
        }
    }

    public class LangTextUi : ILangTextUI
    {
        // Primitive properties
    
        public uint Id { get; set; }
        public string Name { get; set; }
        public string French { get; set; }
        public string English { get; set; }
        public string German { get; set; }
        public string Spanish { get; set; }
        public string Italian { get; set; }
        public string Japanish { get; set; }
        public string Dutsh { get; set; }
        public string Portugese { get; set; }
        public string Russish { get; set; }
    }
}