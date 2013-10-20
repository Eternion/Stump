#region License GNU GPL
// I18NDataManager.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DBSynchroniser.Records.Langs;
using NLog;
using Stump.Core.Memory;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses.Tools.D2i;
using WorldEditor.Config;
using WorldEditor.Database;

namespace WorldEditor.Loaders.I18N
{
    public class I18NDataManager : Singleton<I18NDataManager>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Dictionary<uint, LangText> m_langs = new Dictionary<uint, LangText>();
        private Dictionary<string, LangTextUi> m_langsUi = new Dictionary<string, LangTextUi>();

        public void Initialize()
        {
            m_langs = DatabaseManager.Instance.Database.Query<LangText>("SELECT * FROM langs").ToDictionary(x => x.Id);
            foreach (var record in DatabaseManager.Instance.Database.Query<LangTextUi>("SELECT * FROM langs_ui"))
            {
                if (!m_langsUi.ContainsKey(record.Name))
                    m_langsUi.Add(record.Name, record);
            }
        }

        Languages m_defaultLanguage;
        public Languages DefaultLanguage
        {
            get
            {
                return m_defaultLanguage;
            }
            set
            {
                m_defaultLanguage = value;
            }
        }

        public LangText GetText(int id)
        {
            return GetText((uint) id);
        }

        public LangText GetText(uint id)
        {
            LangText record;
            if (!m_langs.TryGetValue(id, out record))
                return null;

            return record;
        }

        public LangTextUi GetText(string id)
        {
            LangTextUi record;
            if (!m_langsUi.TryGetValue(id, out record))
                return null;

            return record;
        }

        public string ReadText(int id, Languages? lang)
        {
            return ReadText((uint)id, lang);
        }

        public string ReadText(uint id, Languages? lang)
        {
            LangText record;
            if (!m_langs.TryGetValue(id, out record))
                return "{null}";

            return record.GetText(lang ?? DefaultLanguage);
        }

        public string ReadText(string id, Languages? lang)
        {
            LangTextUi record;
            if (!m_langsUi.TryGetValue(id, out record))
                return "{null}";

            return record.GetText(lang ?? DefaultLanguage);
        }

        public string ReadText(string id)
        {
            return ReadText(id, null);
        }

        public string ReadText(uint id)
        {
            return ReadText((int)id);
        }

        public string ReadText(int id)
        {
            return ReadText(id, null);
        }

        public void SaveText(LangText text)
        {
            DatabaseManager.Instance.Database.Save(text);
        }

        public void SaveText(LangTextUi text)
        {
            DatabaseManager.Instance.Database.Save(text);
        }
        public uint FindFreeId()
        {
            var id = m_langs.Keys.Max();

            return id < Settings.MinI18NId ? Settings.MinI18NId : id;
        }
    }
}