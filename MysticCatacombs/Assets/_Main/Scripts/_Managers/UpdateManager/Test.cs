using System;
using Game.UpdateManager.Interfaces;
using UnityEngine;

namespace Game.UpdateManager
{
    public class Test : UpdatableBehaviour
    {
        public int aa;
        
        protected override void OnUpdate()
        {
            Debug.Log($"Update: {gameObject.name}");
        }

        protected override void OnLateUpdate()
        {
            Debug.Log($"Late: {gameObject.name}");

        }

        protected override void OnFixedUpdate()
        {
            Debug.Log($"Fixed: {gameObject.name}");

        }
    }
    
    
}