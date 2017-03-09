using System.Threading;

namespace Elurnity
{
    public static class Pool<T> where T : class, new()
    {
        private const int PoolSize = 20;
        private static readonly T[] pool = new T[PoolSize];

        public static T Get()
        {
            for (int i = 0; i < pool.Length; i++)
            {
                object obj;
                if ((obj = Interlocked.Exchange(ref pool[i], null)) != null)
                {
                    return (T)obj;
                }
            }
            return new T();
        }

        public static void Release(ref T instance)
        {
            int num = 0;
            while (num < pool.Length && Interlocked.CompareExchange(ref pool[num], instance, null) != null)
            {
                num++;
            }
            instance = null;
        }
    }
}
