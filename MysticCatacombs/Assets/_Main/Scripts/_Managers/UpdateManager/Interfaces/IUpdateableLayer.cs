using System;

namespace Game.UpdateManager.Interfaces
{
    public interface IUpdateableLayer<T> : IDisposable
    {
        int Count { get; }
        void Tick();

        bool Add(T updatable);
        bool Remove(T updatable);
    }
}