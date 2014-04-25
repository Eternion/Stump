using System.Collections.Generic;
using System.Linq;
using Stump.Core.I18N;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.WorldServer.Database.I18n
{
    public class TextManager : DataManager<TextManager>
    {
        private Languages? m_defaultLanguages;
        private Dictionary<uint, LangText> m_texts = new Dictionary<uint, LangText>();
        private Dictionary<string, LangTextUi> m_textsUi = new Dictionary<string, LangTextUi>();

        [Initialization(InitializationPass.First)]
        public override void Initialize()
        {
            m_texts = Database.Fetch<LangText>(LangTextRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_textsUi = Database.Fetch<LangTextUi>(LangTextUiRelator.FetchQuery).ToDictionary(entry => entry.Name);
        }

        public void SetDefaultLanguage(Languages languages)
        {
            m_defaultLanguages = languages;
        }

        public Languages GetDefaultLanguage()
        {
            return m_defaultLanguages.HasValue ? m_defaultLanguages.Value : BaseServer.Settings.Language;
        }

        public string GetText(int id)
        {
            return GetText(id, GetDefaultLanguage());
        }

        public string GetText(int id, Languages lang)
        {
            return GetText((uint) id, lang);
        }

        public string GetText(uint id)
        {
            return GetText(id, GetDefaultLanguage());
        }

        public string GetText(uint id, Languages lang)
        {
            LangText record;
            return !m_texts.TryGetValue(id, out record) ? "(not found)" : GetText(record, lang);
        }

        public string GetText(LangText record)
        {
            return GetText(record, GetDefaultLanguage());
        }

        public string GetText(LangText record, Languages lang)
        {
            switch (lang)
            {
                case Languages.English:
                    return record.English ?? "(not found)";
                case Languages.French:
                    return record.French ?? "(not found)";
                case Languages.German:
                    return record.German ?? "(not found)";
                case Languages.Spanish:
                    return record.Spanish ?? "(not found)";
                case Languages.Italian:
                    return record.Italian ?? "(not found)";
                case Languages.Japanish:
                    return record.Japanish ?? "(not found)";
                case Languages.Dutsh:
                    return record.Dutsh ?? "(not found)";
                case Languages.Portugese:
                    return record.Portugese ?? "(not found)";
                case Languages.Russish:
                    return record.Russish ?? "(not found)";
                default:
                    return "(not found)";
            }
        }

        public string GetUiText(string id)
        {
            return GetUiText(id, GetDefaultLanguage());
        }

        public string GetUiText(string id, Languages lang)
        {
            LangTextUi record;
            if (!m_textsUi.TryGetValue(id, out record))
                return "(not found)";

            switch (lang)
            {
                case Languages.English:
                    return record.English ?? "(not found)";
                case Languages.French:
                    return record.French ?? "(not found)";
                case Languages.German:
                    return record.German ?? "(not found)";
                case Languages.Spanish:
                    return record.Spanish ?? "(not found)";
                case Languages.Italian:
                    return record.Italian ?? "(not found)";
                case Languages.Japanish:
                    return record.Japanish ?? "(not found)";
                case Languages.Dutsh:
                    return record.Dutsh ?? "(not found)";
                case Languages.Portugese:
                    return record.Portugese ?? "(not found)";
                case Languages.Russish:
                    return record.Russish ?? "(not found)";
                default:
                    return "(not found)";
            }
        }
    }
}