using System;
using System.Reflection;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Tools.CacheManager
{
    public class D2OTableField
    {
        private readonly FieldInfo m_field;
        private readonly PropertyInfo m_property;

        public D2OTableField(FieldInfo field)
        {
            m_field = field;
            Attribute = field.GetCustomAttribute<D2OFieldAttribute>();
        }

        public D2OTableField(PropertyInfo property)
        {
            m_property = property;
            Attribute = property.GetCustomAttribute<D2OFieldAttribute>();
        }

        public D2OFieldAttribute Attribute
        {
            get;
            set;
        }

        public object GetValue(object instance)
        {
            if (m_field != null)
                return m_field.GetValue(instance);

            else if (m_property != null)
                return m_property.GetValue(instance, new object[0]);

            else
                throw new Exception("Cannot get value : no property or field are binded");
        }

        public void SetValue(object instance, object value)
        {
            if (m_field != null)
                m_field.SetValue(instance, value);

            else if (m_property != null)
                m_property.SetValue(instance, value, new object[0]);

            else
                throw new Exception("Cannot set value : no property or field are binded");
        }
    }
}