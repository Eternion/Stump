#region License GNU GPL

// D2IGridRow.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
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

using DBSynchroniser.Records.Langs;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Editors.Langs
{
    public sealed class LangTextRow : LangGridRow
    {
        private readonly LangText m_record;

        public LangTextRow()
        {
        }

        public LangTextRow(LangText record)
        {
            m_record = record;
        }

        public uint Id
        {
            get { return m_record.Id; }
            set { m_record.Id = value; }
        }


        public override string French
        {
            get { return m_record.French; }
            set { m_record.French = value; }
        }


        public override string English
        {
            get { return m_record.English; }
            set { m_record.English = value; }
        }


        public override string German
        {
            get { return m_record.German; }
            set { m_record.German = value; }
        }


        public override string Spanish
        {
            get { return m_record.Spanish; }
            set { m_record.Spanish = value; }
        }


        public override string Italian
        {
            get { return m_record.Italian; }
            set { m_record.Italian = value; }
        }


        public override string Japanish
        {
            get { return m_record.Japanish; }
            set { m_record.Japanish = value; }
        }


        public override string Dutsh
        {
            get { return m_record.Dutsh; }
            set { m_record.Dutsh = value; }
        }


        public override string Portugese
        {
            get { return m_record.Portugese; }
            set { m_record.Portugese = value; }
        }


        public override string Russish
        {
            get { return m_record.Russish; }
            set { m_record.Russish = value; }
        }

        public override string GetKey()
        {
            return Id.ToString();
        }
        
        public override void Save()
        {
            switch (State)
            {
                case RowState.Added:
                    I18NDataManager.Instance.CreateText(m_record);
                    break;
                case RowState.Dirty:
                    I18NDataManager.Instance.SaveText(m_record);
                    break;
                case RowState.Removed:
                    I18NDataManager.Instance.DeleteText(m_record);
                    break;
            }

            State = RowState.None;
        }
    }
}