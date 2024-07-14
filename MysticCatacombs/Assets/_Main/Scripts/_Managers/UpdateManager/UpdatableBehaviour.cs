using System;
using Game.UpdateManager.Interfaces;
using UnityEngine;

namespace Game.UpdateManager
{
    public class UpdatableBehaviour : MonoBehaviour, IUpdatable, ILateUpdatable, IFixedUpdatable
    {
        [SerializeField] [HideInInspector] private UpdateLayer layer;
        [SerializeField] [HideInInspector] private bool doUpdate;
        [SerializeField] [HideInInspector] private bool doFixedUpdate;
        [SerializeField] [HideInInspector] private bool doLateUpdate;

        private void OnEnable()
        {
            if (doUpdate) UpdateManager.Instance.AddUpdatable(this);
            if (doFixedUpdate) UpdateManager.Instance.AddFixedUpdatable(this);
            if (doLateUpdate) UpdateManager.Instance.AddLateUpdatable(this);
        }

        private void OnDisable()
        {
            UpdateManager.Instance.RemoveUpdatable(this);
            UpdateManager.Instance.RemoveFixedUpdatable(this);
            UpdateManager.Instance.RemoveLateUpdatable(this);
        }
        
        public void DoUpdate()
        {
            OnUpdate();
        }

        public void DoLateUpdate()
        {
            OnLateUpdate();
        }

        public void DoFixedUpdate()
        {
            OnFixedUpdate();
        }
        
        public UpdateLayer UpdateLayer()
        {
            return layer;
        }
        
        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnLateUpdate() { }
    }
}