using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.I18n;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.WorldServer.Database.I18n
{
    public class TextManager : Singleton<TextManager>
    {
        private Dictionary<uint, TextRecord> m_texts = new Dictionary<uint, TextRecord>();
        private Dictionary<string, TextUIRecord> m_textsUi = new Dictionary<string, TextUIRecord>();

        [Initialization(InitializationPass.First)]
        public void Intialize()
        {
            m_texts = TextRecord.FindAll().ToDictionary(entry => entry.Id);
            m_textsUi = TextUIRecord.FindAll().ToDictionary(entry => entry.Name);
        }

        public string GetText(int id)
        {
            return GetText(id, BaseServer.Settings.Language);
        }

        public string GetText(int id, Languages lang)
        {
            return GetText((uint)id, lang);
        }

        public string GetText(uint id)
        {
            return GetText(id, BaseServer.Settings.Language);
        }

        public string GetText(uint id, Languages lang)
        {
            TextRecord record;
            if (!m_texts.TryGetValue(id, out record))
                return "(not found)";

            switch (lang)
            {
                case Languages.English:
                    return record.En;
                case Languages.French:
                    return record.Fr;
                case Languages.German:
                    return record.De;
                case Languages.Spanish:
                    return record.Es;
                case Languages.Italian:
                    return record.It;
                case Languages.Japanish:
                    return record.Ja;
                case Languages.Dutsh:
                    return record.Nl;
                case Languages.Portugese:
                    return record.Pt;
                case Languages.Russish:
                    return record.Ru;
                default:
                    return "(not found)";
            }
        }

        public string GetUiText(string id)
        {
            return GetUiText(id, BaseServer.Settings.Language);
        }

        public string GetUiText(string id, Languages lang)
        {
            TextUIRecord record;
            if (!m_textsUi.TryGetValue(id, out record))
                return "(not found)";

            switch (lang)
            {
                case Languages.English:
                    return record.En;
                case Languages.French:
                    return record.Fr;
                case Languages.German:
                    return record.De;
                case Languages.Spanish:
                    return record.Es;
                case Languages.Italian:
                    return record.It;
                case Languages.Japanish:
                    return record.Ja;
                case Languages.Dutsh:
                    return record.Nl;
                case Languages.Portugese:
                    return record.Pt;
                case Languages.Russish:
                    return record.Ru;
                default:
                    return "(not found)";
            }
        }
    }
}