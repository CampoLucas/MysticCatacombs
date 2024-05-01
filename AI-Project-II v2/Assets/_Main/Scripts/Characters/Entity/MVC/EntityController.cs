using System;
using UnityEngine;
using Game.FSM;
using Game.Interfaces;

namespace Game.Entities
{
    public class EntityController<T> : MonoBehaviour, IController<T>
    {
        public IStateMachine<T> StateMachine { get; protected set; }
        public IModel Model { get; private set; }
        public IView View { get; private set; }

        protected virtual void InitFSM()
        {
            StateMachine = new FSM<T>();
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
                StateMachine.OnUpdate();
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
        
        public string GetCurrentState() => StateMachine.CurrentID.ToString();

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
    }
}