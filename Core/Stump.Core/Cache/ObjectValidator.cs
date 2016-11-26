using System;

namespace Stump.Core.Cache
{
    public sealed class ObjectValidator<T>
    {

        public event Action<ObjectValidator<T>> ObjectInvalidated;

        private void NotifyObjectInvalidated()
        {
            var handler = ObjectInvalidated;
            if (handler != null)
                handler(this);
        }


        private readonly object m_sync = new object();
        private bool m_isValid;
        private T m_instance;
        private DateTime m_lastCreationDate;
        private readonly Func<T> m_creator;

        public ObjectValidator(Func<T> creator, TimeSpan? lifeTime = null)
        {
            LifeTime = lifeTime;
            m_creator = creator;
        }

        private bool IsValid => m_isValid && (LifeTime == null || DateTime.Now - m_lastCreationDate < LifeTime.Value);
        
        public TimeSpan? LifeTime
        {
            get;
            set;
        }

        public void Invalidate()
        {
            m_isValid = false;

            NotifyObjectInvalidated();
        }

        public static implicit operator T(ObjectValidator<T> validator)
        {
            if (validator.IsValid)
                return validator.m_instance;

            lock (validator.m_sync)
            {
                if (validator.IsValid)
                    return validator.m_instance;

                validator.m_instance = validator.m_creator();
                validator.m_lastCreationDate = DateTime.Now;
                validator.m_isValid = true;
            }

            return validator.m_instance;
        }
    }

    public class ObjectValidator<T, TContext>
    {
        public event Action<ObjectValidator<T, TContext>> ObjectInvalidated;

        private void NotifyObjectInvalidated()
        {
            var handler = ObjectInvalidated;
            if (handler != null)
                handler(this);
        }

        private readonly object m_sync = new object();

        private bool m_isValid;

        private T m_instance;

        private readonly TContext m_context;
        private readonly Func<TContext, T> m_creator;

        public ObjectValidator(TContext context, Func<TContext, T> creator)
        {
            m_context = context;
            m_creator = creator;
        }

        public void Invalidate()
        {
            m_isValid = false;

            NotifyObjectInvalidated();
        }

        public static implicit operator T(ObjectValidator<T, TContext> validator)
        {
            if (validator.m_isValid)
                return validator.m_instance;

            lock (validator.m_sync)
            {
                if (validator.m_isValid)
                    return validator.m_instance;

                validator.m_instance = validator.m_creator(validator.m_context);
                validator.m_isValid = true;
            }

            return validator.m_instance;
        }
    }
    
}