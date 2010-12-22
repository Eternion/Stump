// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;

namespace Stump.Server.BaseServer.Commands
{
    public class CommandParameter<T> : ICommandParameter<T>
    {
        private string m_stringValue;
        private T m_value;

        public CommandParameter(string name, string shortName = "", string description = "", bool isOptional = false,
                                T defaultValue = default(T),
                                Func<string, TriggerBase, T> converter = null)
        {
            Name = name;
            ShortName = shortName;
            Description = description;
            IsOptional = isOptional;
            DefaultValue = defaultValue;
            Converter = converter;
        }

        #region ICommandParameter<T> Members

        public Func<string, TriggerBase, T> Converter
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string ShortName
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public bool IsOptional
        {
            get;
            private set;
        }

        public bool IsValueDefined
        {
            get;
            private set;
        }

        object ICommandParameter.DefaultValue
        {
            get { return DefaultValue; }
        }

        public T DefaultValue
        {
            get;
            private set;
        }

        public Type ValueType
        {
            get { return typeof (T); }
        }

        object ICommandParameter.Value
        {
            get { return Value; }
        }

        public T Value
        {
            get { return m_value; }
            private set
            {
                IsValueDefined = true;
                m_value = value;
            }
        }

        public string StringValue
        {
            get { return m_stringValue; }
        }

        public void SetStringValue(string str, TriggerBase trigger)
        {
            if (ValueType == typeof (string))
                Value = (T) (object) str;
            else if (Converter != null && trigger != null)
                Value = Converter(str, trigger);
            else if (Value is IConvertible)
                Value = (T) Convert.ChangeType(str, typeof (T));
            else if (DefaultValue != null)
                Value = DefaultValue;
            else
                Value = default(T);
        }


        /// <summary>
        ///   Sets parameter's value to default value
        /// </summary>
        /// <returns>False if <see cref = "IsOptional" /> is false or <see cref = "DefaultValue" /> is not assigned</returns>
        public bool SetDefaultValue()
        {
            // if not optional and default value isn't set we leave
            if ((Equals(DefaultValue, default(T))) && !IsOptional)
                return false;

            if (Equals(DefaultValue, default(T)) && IsOptional)
                return true;

            Value = DefaultValue;
            return true;
        }

        public string GetUsage()
        {
            string usage = Name != ShortName ? Name + "/" + ShortName : Name;

            if (!Equals(DefaultValue, default(T)))
            {
                usage += "=" + DefaultValue;
            }

            return !IsOptional ? usage : "[" + usage + "]";
        }

        public T GetValue()
        {
            return Value;
        }

        public object Clone()
        {
            return new CommandParameter<T>(Name, ShortName, Description, IsOptional, DefaultValue, Converter)
                {
                    m_stringValue = m_stringValue,
                    Value = Value
                };
        }

        #endregion

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}