using System;

namespace Stump.Core.Cache
{
    public class ObjectValidator<T>
    {
        private readonly object m_sync = new object();

        private bool m_isValid;

        private T m_instance;

        private readonly Func<T> m_creator;

        public ObjectValidator(Func<T> creator)
        {
            m_creator = creator;
        }

        public void Invalidate()
        {
            m_isValid = false;
        }

        public static implicit operator T(ObjectValidator<T> validator)
        {
            if (!validator.m_isValid)
            {
                lock (validator.m_sync)
                {
                    if (!validator.m_isValid) // if a thread is blocked on the lock then an other thread is recreating a new instance
                    {
                        validator.m_instance = validator.m_creator();
                        validator.m_isValid = true;
                    }
                }
            }

            return validator.m_instance;
        }
    }

    public class ObjectValidator<T, TContext>
    {
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
        }

        public static implicit operator T(ObjectValidator<T, TContext> validator)
        {
            if (!validator.m_isValid)
            {
                lock (validator.m_sync)
                {
                    if (!validator.m_isValid) // if a thread is blocked on the lock then an other thread is recreating a new instance
                    {
                        validator.m_instance = validator.m_creator(validator.m_context);
                        validator.m_isValid = true;
                    }
                }
            }

            return validator.m_instance;
        }
    }
    
}