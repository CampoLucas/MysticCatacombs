using System;
using Game.Interfaces;
using Unity.VisualScripting;
using State = Game.StateMachine.State;

namespace Game.Player.States
{
    /// <summary>
    /// A state class that is inherited by all the player state classes.
    /// </summary>
    public class EntityState : State
    {
        public Action OnStartState;
        public Action OnExitState;
        
        protected IController Controller;
        protected IModel Model;
        protected IView View;
        protected bool holdState;

        /// <summary>
        /// A method that initializes the provided parameters.
        /// </summary>
        public void Init(IController controller)
        {
            Controller = controller;
            Model = controller.Model;
            View = controller.View;
        }

        protected override void OnStart()
        {
            base.OnStart();
            OnStartState?.Invoke();
        }

        protected override void OnExit()
        {
            base.OnExit();
            OnExitState?.Invoke();
        }

        /// <summary>
        /// A method that nullifies all references and unsubscribes from all events
        /// </summary>
        protected override void OnDisposed()
        {
            Model = null;
            View = null;
            Controller = null;
        }

        public override bool CanTransition()
        {
            return !holdState;
        }
    }
}