#region License GNU GPL

// EffectWrapper.cs
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

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using DBSynchroniser.Records;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools;
using WorldEditor.Annotations;
using WorldEditor.Database;
using WorldEditor.Editors.Items.Effects;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Editors.Items
{
    [Serializable]
    public abstract class EffectWrapper : ICloneable, INotifyPropertyChanged
    {
        private string m_description;
        private EffectRecord m_template;


        protected EffectWrapper(EffectInstance wrappedEffect)
        {
            WrappedEffect = wrappedEffect;
        }

        public EffectInstance WrappedEffect
        {
            get;
            private set;
        }

        public EffectRecord Template
        {
            get { return m_template ?? (m_template = DatabaseManager.Instance.Database.
                SingleOrDefault<EffectRecord>(string.Format("SELECT * FROM Effects WHERE Id={0}", EffectId))); }
        }

        public int EffectId
        {
            get
            {
                return (int)WrappedEffect.effectId;
            }
            set
            {
                WrappedEffect.effectId = (uint)value;
                m_template = DatabaseManager.Instance.Database.
                              SingleOrDefault<EffectRecord>(string.Format("SELECT * FROM Effects WHERE Id={0}", EffectId));
                OnPropertyChanged("Description");
                OnPropertyChanged("Template");
                OnPropertyChanged("Operator");
                OnPropertyChanged("Priority");
            }
        }

        public int TargetId
        {
            get { return WrappedEffect.targetId; }
            set { WrappedEffect.targetId = value; }
        }

        public int Duration
        {
            get { return WrappedEffect.duration; }
            set { WrappedEffect.duration = value; }
        }

        public int Delay
        {
            get { return WrappedEffect.delay; }
            set { WrappedEffect.delay = value; }
        }

        public int Random
        {
            get { return WrappedEffect.random; }
            set { WrappedEffect.random = value; }
        }

        public int Group
        {
            get { return WrappedEffect.group; }
            set { WrappedEffect.group = value; }
        }

        public int Modificator
        {
            get { return WrappedEffect.modificator; }
            set { WrappedEffect.modificator = value; }
        }

        public Boolean Trigger
        {
            get { return WrappedEffect.trigger; }
            set { WrappedEffect.trigger = value; }
        }

        public uint ZoneSize
        {
            get { return (uint)WrappedEffect.zoneSize; }
            set { WrappedEffect.zoneSize = value; }
        }

        public uint ZoneShape
        {
            get { return WrappedEffect.zoneShape; }
            set { WrappedEffect.zoneShape = value; }
        }

        public uint ZoneMinSize
        {
            get { return (uint)WrappedEffect.zoneMinSize; }
            set { WrappedEffect.zoneMinSize = value; }
        }

        public String RawZone
        {
            get { return WrappedEffect.rawZone; }
            set { WrappedEffect.rawZone = value; }
        }


        public string Description
        {
            get
            {
                if (m_description != null)
                    return m_description;

                string pattern = I18NDataManager.Instance.ReadText(Template.descriptionId);

                var decoder = new StringPatternDecoder(pattern, GetDescriptionValues());

                int? index;
                if ((index = decoder.CheckValidity(false)) != null)
                    return string.Format("Error in pattern '{0}' at index {1}", pattern, index);

                return m_description = decoder.Decode();
            }
        }

        public string Operator
        {
            get { return Template.@operator; }
        }

        public uint Priority
        {
            get { return Template.effectPriority; }
        }

        public abstract int ParametersCount
        {
            get;
        }

        public abstract object this[int index]
        {
            get;
            set;
        }

        public abstract object[] Parameters
        {
            get;
        }

        private object[] GetDescriptionValues()
        {
            return Parameters.Select(x => x is uint && (uint)x == 0 ? null : x).ToArray();
        }

        public object Clone()
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, WrappedEffect);
            stream.Position = 0;
            return Create((EffectInstance)formatter.Deserialize(stream));
        }

        public static EffectWrapper Create(EffectInstance effect)
        {
            var dice = effect as EffectInstanceDice;

            if (dice != null)
                return new EffectDiceWrapper(dice);

            var integer = effect as EffectInstanceInteger;
            if (integer != null)
                return new EffectValueWrapper(integer);

            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "Description")
                m_description = null;

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}