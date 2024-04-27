using Game.DecisionTree;
using Game.FSM;
using Game.Interfaces;

namespace Game.Enemies.States
{
    /// <summary>
    /// A state class that is inherited by all the enemy state classes.
    /// </summary>
    public class EnemyStateBase<T> : State<T>
    {

        protected EnemyModel Model;
        protected IView View;
        protected EnemyController Controller;

        /// <summary>
        /// A method that initializes the provided parameters.
        /// </summary>
        public void Init(IController<T> controller)
        {
            Controller = controller as EnemyController;
            Model = controller.Model as EnemyModel;
            View = controller.View;
        }

        /// <summary>
        /// When it awakes it subscribes to the damage and dead events because any state has to be able to transition to those states.
        /// </summary>
        // public override void Start()
        // {
        //     base.Start();
        //     if (Model.Damageable != null)
        //     {
        //         Model.Damageable.OnTakeDamage += OnDamageHandler;
        //         Model.Damageable.OnDie += OnDeadHandler;
        //     }
        // }

        /// <summary>
        /// When it changes states it unsubscribes to all the events.
        /// </summary>
        // public override void Exit()
        // {
        //     base.Exit();
        //     UnsubscribeAll();
        // }

        /// <summary>
        /// A method that unsubscribes to all events
        /// </summary>
        // public void UnsubscribeAll()
        // {
        //     if (Model.Damageable != null)
        //     {
        //         Model.Damageable.OnTakeDamage -= OnDamageHandler;
        //         Model.Damageable.OnDie -= OnDeadHandler;
        //     }
        // }

        // private void OnDamageHandler() => Controller.StateMachine.S;
        // private void OnDeadHandler() => Tree.Execute();

        /// <summary>
        /// A method that nullifies all references and unsubscribes from all events
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            //UnsubscribeAll();
            Model = null;
            View = null;
            Controller = null;
            //Tree = null;
        }
    }
}