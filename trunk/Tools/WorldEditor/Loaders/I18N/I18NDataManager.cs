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

using System.Collections.Generic;
using System.Linq;
using DBSynchroniser.Records.Langs;
using Stump.Core.I18N;
using Stump.Core.Reflection;
using WorldEditor.Config;
using WorldEditor.Database;

namespace WorldEditor.Loaders.I18N
{
    public class I18NDataManager : Singleton<I18NDataManager>
    {
        private Dictionary<uint, LangText> m_langs = new Dictionary<uint, LangText>();
        private readonly Dictionary<string, LangTextUi> m_langsUi = new Dictionary<string, LangTextUi>();

        public void Initialize()
        {
            m_langs = DatabaseManager.Instance.Database.Query<LangText>("SELECT * FROM langs").ToDictionary(x => x.Id);
            foreach (var record in DatabaseManager.Instance.Database.Query<LangTextUi>("SELECT * FROM langs_ui").Where(record => !m_langsUi.ContainsKey(record.Name)))
            {
                m_langsUi.Add(record.Name, record);
            }
        }

        public Languages DefaultLanguage
        {
            get;
            set;
        }

        public LangText GetText(int id)
        {
            return GetText((uint) id);
        }

        public LangText GetText(uint id)
        {
            LangText record;
            return !m_langs.TryGetValue(id, out record) ? null : record;
        }

        public LangTextUi GetText(string id)
        {
            LangTextUi record;
            return !m_langsUi.TryGetValue(id, out record) ? null : record;
        }

        public string ReadText(int id, Languages? lang)
        {
            return ReadText((uint)id, lang);
        }

        public string ReadText(uint id, Languages? lang)
        {
            LangText record;
            return !m_langs.TryGetValue(id, out record) ? "{null}" : record.GetText(lang ?? DefaultLanguage);
        }

        public string ReadText(string id, Languages? lang)
        {
            LangTextUi record;
            return !m_langsUi.TryGetValue(id, out record) ? "{null}" : record.GetText(lang ?? DefaultLanguage);
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
            DatabaseManager.Instance.Database.Update(text);
        }

        public void SaveText(LangTextUi text)
        {
            DatabaseManager.Instance.Database.Update(text);
        }

        public void CreateText(LangText text)
        {
            DatabaseManager.Instance.Database.Insert(text);
        }

        public void CreateText(LangTextUi text)
        {
            DatabaseManager.Instance.Database.Insert(text);
        }

        public uint FindFreeId()
        {
            var id = m_langs.Keys.Max();

            return id < Settings.MinI18NId ? Settings.MinI18NId : id;
        }
    }
}