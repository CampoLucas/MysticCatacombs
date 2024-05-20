using System;
using UnityEngine;

namespace Game.WeaponSystem
{
    public abstract class Weapon : MonoBehaviour, IDisposable
    {
        public void Begin()
        {
            OnBegin();
        }

        public void End()
        {
            OnEnd();
        }
        
        public void Dispose()
        {
            OnDispose();
        }

        protected abstract void OnBegin();
        protected abstract void OnEnd();
        protected virtual void OnDispose() { }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}