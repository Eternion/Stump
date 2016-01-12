namespace Stump.Core.Memory
{
    public class Ref<T>
    {
        public Ref(T target)
        {
            Target = target;
        }

         public T Target
         {
             get;
             set;
         }
    }
}