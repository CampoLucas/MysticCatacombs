using System;
using System.Collections.Generic;
using Game.UpdateManager.Interfaces;

namespace Game.UpdateManager
{
    public class UpdateableLayer : IDisposable, IUpdateableLayer<IUpdatable>
    {
        public int Count => _updates.Count;
        
        private HashSet<IUpdatable> _updates = new();
        
        public void Tick()
        {
            foreach (var update in _updates)
            {
                update.DoUpdate();
            }
        }

        public bool Add(IUpdatable updatable)
        {
            return _updates.Add(updatable);
        }
        
        public bool Remove(IUpdatable updatable)
        {
            return _updates.Remove(updatable);
        }
        
        public void Dispose()
        {
            _updates = null;
        }
    }

    public class LateUpdateableLayer : IDisposable, IUpdateableLayer<ILateUpdatable>
    {
        public int Count => _updates.Count;
        
        private HashSet<ILateUpdatable> _updates = new();
        
        public void Tick()
        {
            foreach (var update in _updates)
            {
                update.DoLateUpdate();
            }
        }
        
        public bool Add(ILateUpdatable updatable)
        {
            return _updates.Add(updatable);
        }

        public bool Remove(ILateUpdatable updatable)
        {
            return _updates.Remove(updatable);
        }
        
        public void Dispose()
        {
            _updates = null;
        }
    }

    public class FixedUpdateableLayer : IDisposable, IUpdateableLayer<IFixedUpdatable>
    {
        public int Count => _updates.Count;
        
        private HashSet<IFixedUpdatable> _updates = new();
        
        public void Tick()
        {
            foreach (var update in _updates)
            {
                update.DoFixedUpdate();
            }
        }
        
        public bool Add(IFixedUpdatable updatable)
        {
            return _updates.Add(updatable);
        }
        
        public bool Remove(IFixedUpdatable updatable)
        {
            return _updates.Remove(updatable);
        }
        
        public void Dispose()
        {
            _updates = null;
        }
    }

    
}