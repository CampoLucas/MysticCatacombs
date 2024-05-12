using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.DesignPatterns.Pool
{
    public class Pool<T> : IDisposable where T : new()
    {
        public List<T> Used { get; private set; } = new();
        public List<T> UnUsed { get; private set; } = new();

        private Func<T> _create;

        public Pool(Func<T> create)
        {
            _create = create;
        }

        public T Get()
        {
            T t;
            if (UnUsed.Count > 0)
            {
                t = UnUsed.First();
                UnUsed.Remove(t);
            }
            else
            {
                t = _create();
            }

            Used.Add(t);
            return t;
        }

        public void Recicle(T t)
        {
            Used.Remove(t);
            UnUsed.Add(t);
        }

        public void Add(T t)
        {
            Used.Add(t);
        }

        public void Dispose()
        {
            Used.Clear();
            UnUsed.Clear();
            _create = null;
        }
    }
}