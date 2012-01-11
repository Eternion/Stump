using System;
using System.Reflection;
using Castle.ActiveRecord;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Tools.CacheManager
{
    public class D2OField
    {
        private readonly FieldInfo m_field;
        private readonly PropertyInfo m_property;

        public D2OField(FieldInfo field)
        {
            m_field = field;
            Name = field.Name;
            D2OAttr = field.GetCustomAttribute<D2OFieldAttribute>();
            DbFieldAttr = field.GetCustomAttribute<FieldAttribute>();
            DbBelongsAttr = field.GetCustomAttribute<BelongsToAttribute>();
        }

        public D2OField(PropertyInfo property)
        {
            m_property = property;
            Name = property.Name;
            D2OAttr = property.GetCustomAttribute<D2OFieldAttribute>();
            DbPropAttr = property.GetCustomAttribute<PropertyAttribute>();
            DbBelongsAttr = property.GetCustomAttribute<BelongsToAttribute>();
        }

        public string Name
        {
            get;
            set;
        }

        public D2OFieldAttribute D2OAttr
        {
            get;
            set;
        }

        public FieldAttribute DbFieldAttr
        {
            get;
            set;
        }

        public PropertyAttribute DbPropAttr
        {
            get;
            set;
        }

        public BelongsToAttribute DbBelongsAttr
        {
            get;
            set;
        }

        public string GetDatabaseFieldName()
        {
            if (DbPropAttr != null && DbPropAttr.Column != null)
                return DbPropAttr.Column;

            if (DbBelongsAttr != null && DbBelongsAttr.Column != null)
                return DbBelongsAttr.Column;

            if (DbFieldAttr != null && DbFieldAttr.Column != null)
                return DbFieldAttr.Column;

            if (m_field != null)
                return m_field.Name;

            return m_property.Name;
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