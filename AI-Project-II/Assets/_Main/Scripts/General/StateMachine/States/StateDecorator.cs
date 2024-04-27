using UnityEngine;

namespace Game.StateMachine
{
    public class StateDecorator : IState
    {
        protected IState State;
        protected GameObject Owner;
        protected FSM Fsm;

        public StateDecorator(IState state)
        {
            State = state;
        }

        public void Awake(FSM stateMachine)
        {
            Fsm = stateMachine;
            Owner = Fsm.Owner;
            OnAwake();
            State.Awake(stateMachine);
        }

        public void Start()
        {
            OnStart();
            State.Start();
        }

        public void Update()
        {
            OnUpdate();
            State.Update();
        }

        public void FixedUpdate()
        {
            OnFixedUpdate();
            State.FixedUpdate();
        }

        public void Exit()
        {
            OnExit();
            State.Exit();
        }

        public bool CanTransition()
        {
            return OnCanTransition() && State.CanTransition();
        }
        
        public void Dispose()
        {
            OnDestroy();
            Owner = null;
            Fsm = null;
            State.Dispose();
        }

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

        protected virtual bool OnCanTransition() => true;

        protected virtual void OnDestroy()
        {
            
        }
    }
}