using System;
using UnityEngine;
using Game.Interfaces;
using Game.StateMachine;
using Game.StateMachine.Interfaces;

namespace Game.Entities
{
    public class EntityController : MonoBehaviour, IController
    {
        public IStateMachine StateMachine { get; protected set; }
        public IModel Model { get; private set; }
        public IView View { get; private set; }

        protected virtual void InitFSM()
        {
            StateMachine = new FSM(gameObject);
        }

        protected virtual void Awake()
        {
            Model = GetComponent<IModel>();
            View = GetComponent<IView>();
        }

        protected virtual void Start()
        {
            InitFSM();
        }

        protected virtual void Update()
        {
            if (StateMachine != null)
                StateMachine.Update();
        }

        public TModel GetModel<TModel>() where TModel : IModel
        {
            var model = (TModel)Model;
#if UNITY_EDITOR
            if (model == null) Debug.LogError("Model is null", this);
#endif
            return model;
        }

        public TView GetView<TView>() where TView : IView
        {
            var view = (TView)View;
#if UNITY_EDITOR
            if (view == null) Debug.LogError("View is null", this);
#endif
            return view;
        }
        
        public string GetCurrentState() => StateMachine.CurrentState;

        public virtual float MoveAmount()
        {
            return 0;
        }

        public virtual Vector3 MoveDirection()
        {
            return Vector3.zero;
        }

        public virtual bool DoLightAttack()
        {
            return false;
        }

        public virtual bool DoHeavyAttack()
        {
            return false;
        }
        
        public virtual void SetSteering(ISteering steering)
        {
            //_currentSteering = steering;
        }
        
        private void OnDestroy()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            if (StateMachine != null)
                StateMachine.Dispose();
            StateMachine = null;
            
            
            Model = null;
            View = null;
            
        }

        protected virtual void OnDrawGizmos()
        {
            if (StateMachine != null) StateMachine.Draw();
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (StateMachine != null) StateMachine.DrawOnSelected();
        }
    }
}