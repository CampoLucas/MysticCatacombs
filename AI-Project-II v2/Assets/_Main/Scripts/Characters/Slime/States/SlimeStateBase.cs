using Game.DecisionTree;
using Game.FSM;
using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Slime.States
{
    public abstract class SlimeStateBase<T> : State<T>
    {
        protected IModel Model { get; private set; }
        protected IController<T> Controller { get; private set; }
        protected IView View { get; private set; }

        public SlimeStateBase()
        {
            
        }

        public void Init(IController<T> controller)
        {
            Controller = controller;
            Model = controller.Model;
            View = controller.View;
        }

        public override void Start()
        {
            base.Start();
            if (Model.Damageable != null)
            {
                Model.Damageable.OnTakeDamage += OnDamageHandler;
                Model.Damageable.OnDie += OnDieHandler;
            }
            
        }
        
        public override void Exit()
        {
            base.Exit();
            UnsubscribeAll();
        }

        protected void UnsubscribeAll()
        {
            if (Model.Damageable != null)
            {
                Model.Damageable.OnTakeDamage -= OnDamageHandler;
                Model.Damageable.OnDie -= OnDieHandler;
            }
        }

        private void OnDamageHandler()
        {
            //Tree.Execute();
        }

        private void OnDieHandler()
        {
            //Tree.Execute();
        }
    }
}