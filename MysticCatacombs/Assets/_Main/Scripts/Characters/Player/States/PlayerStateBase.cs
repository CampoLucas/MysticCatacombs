using Game.FSM;
using Game.Interfaces;

namespace Game.Player.States
{
    /// <summary>
    /// A state class that is inherited by all the player state classes.
    /// </summary>
    [System.Serializable]
    public class PlayerStateBase<T> : State<T>
    {
        protected IController<T> Controller;
        protected IModel Model;
        protected IView View;
        private T _inDamage;
        private T _inDead;

        /// <summary>
        /// A default constructor that creates a new PlayerStateBase object.
        /// </summary>
        public PlayerStateBase() {}
        /// <summary>
        /// A constructor that creates a new FSM object and takes the input for damage and death.
        /// </summary>
        public PlayerStateBase(T inDamage, T inDead)
        {
            _inDamage = inDamage;
            _inDead = inDead;
        }

        /// <summary>
        /// A method that initializes the provided parameters.
        /// </summary>
        public void Init(IController<T> controller)
        {
            Controller = controller;
            Model = controller.Model;
            View = controller.View;
        }

        /// <summary>
        /// When it awakes it subscribes to the damage and dead events because any state has to be able to transition to those states.
        /// </summary>
        public override void Start()
        {
            base.Start();
            if (Model.Damageable != null)
            {
                Model.Damageable.OnTakeDamage += OnDamageHandler;
                Model.Damageable.OnDie += OnDeadHandler;
            }
        }

        /// <summary>
        /// When it changes states it unsubscribes to all the events.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
            UnsubscribeAll();
        }
        
        /// <summary>
        /// A method that unsubscribes to all events
        /// </summary>
        public void UnsubscribeAll()
        {
            if (Model == null) return;
            if (Model.Damageable != null)
            {
                Model.Damageable.OnTakeDamage -= OnDamageHandler;
                Model.Damageable.OnDie -= OnDeadHandler;
            }
        }

        private void OnDamageHandler() => Controller.StateMachine.SetState(_inDamage);
        private void OnDeadHandler() => Controller.StateMachine.SetState(_inDead);

        /// <summary>
        /// A method that nullifies all references and unsubscribes from all events
        /// </summary>
        public override void Dispose()
        {
            UnsubscribeAll();
            base.Dispose();
            Model = null;
            View = null;
            Controller = null;
        }
    }
}