using System;
using Stump.Core.I18N;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records.Langs
{
    public class LangTextRelator
    {
        public static string FetchQuery = "SELECT * FROM langs";
    }

    [TableName("langs")]
    public class LangText : IAutoGeneratedRecord
    {
        // Primitive properties

        #region ILangText Members

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get;
            set;
        }

        
        [NullString]
        public string French
        {
            get;
            set;
        }

        
        [NullString]
        public string English
        {
            get;
            set;
        }

        
        [NullString]
        public string German
        {
            get;
            set;
        }

        
        [NullString]
        public string Spanish
        {
            get;
            set;
        }

        
        [NullString]
        public string Italian
        {
            get;
            set;
        }

        
        [NullString]
        public string Japanish
        {
            get;
            set;
        }

        
        [NullString]
        public string Dutsh
        {
            get;
            set;
        }

        
        [NullString]
        public string Portugese
        {
            get;
            set;
        }

        
        [NullString]
        public string Russish
        {
            get;
            set;
        }

        public void SetText(Languages language, string text)
        {
            switch (language)
            {
                case Languages.French:
                    French = text;
                    break;
                case Languages.German:
                    German = text;
                    break;
                case Languages.Dutsh:
                    French = text;
                    break;
                case Languages.Italian:
                    Italian = text;
                    break;
                case Languages.English:
                    English = text;
                    break;
                case Languages.Japanish:
                    Japanish = text;
                    break;
                case Languages.Russish:
                    Russish = text;
                    break;
                case Languages.Spanish:
                    Spanish = text;
                    break;
                case Languages.Portugese:
                    Portugese = text;
                    break;
                case Languages.All:
                    French =
                        German =
                            English =
                                Dutsh =
                                    Japanish = Spanish = Italian = Russish = Portugese = text;
                    break;
            }
        }

        public string GetText(Languages language, Languages defaultLanguage)
        {
            switch (language)
            {
                case Languages.French:
                    return French;
                case Languages.German:
                    return German;
                case Languages.Dutsh:
                    return French;
                case Languages.Italian:
                    return Italian;
                case Languages.English:
                    return English;
                case Languages.Japanish:
                    return Japanish;
                case Languages.Russish:
                    return Russish;
                case Languages.Spanish:
                    return Spanish;
                case Languages.Portugese:
                    return Portugese;
                default:
                    return GetText(defaultLanguage);
            }
        }
        public string GetText(Languages language)
        {
            switch (language)
            {
                case Languages.French:
                    return French;
                case Languages.German:
                    return German;
                case Languages.Dutsh:
                    return French;
                case Languages.Italian:
                    return Italian;
                case Languages.English:
                    return English;
                case Languages.Japanish:
                    return Japanish;
                case Languages.Russish:
                    return Russish;
                case Languages.Spanish:
                    return Spanish;
                case Languages.Portugese:
                    return Portugese;
                default:
                    throw new Exception(string.Format("Unknow language {0}", language));
            }
        }
        #endregion

        public LangText Copy()
        {
            return (LangText) MemberwiseClone();
        }
    }
}