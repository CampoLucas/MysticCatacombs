using Unity.VisualScripting;
using UnityEngine;

namespace Game.StateMachine
{
    public class State : IState
    {
        protected GameObject Owner;
        protected FSM Fsm;
        

        #region Public Methods

        public void Dispose()
        {
            OnDestroy();
            Owner = null;
            Fsm = null;
        }

        public void Awake(FSM stateMachine)
        {
            Fsm = stateMachine;
            Owner = Fsm.Owner;
            OnAwake();
        }

        public void Start()
        {
            OnStart();
        }

        public void Update()
        {
            OnUpdate();
        }

        public void FixedUpdate()
        {
            OnFixedUpdate();
        }

        public void Exit()
        {
            OnExit();
        }

        public virtual bool CanTransition()
        {
            return true;
        }

        public GameObject GetDefault(GameObject value)
        {
            if (value == null)
                value = Owner;
            return value;
        }

        #endregion


        #region Protected Methods

        protected virtual void OnAwake()
        {
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnUpdate()
        {
        }

        protected virtual void OnFixedUpdate()
        {
        }

        protected virtual void OnExit()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        #endregion
    }
}