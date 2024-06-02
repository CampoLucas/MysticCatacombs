using System;
using System.Collections.Generic;
using System.Linq;
using Game.StateMachine.Interfaces;
using UnityEngine;

namespace Game.StateMachine
{
    public class State : IState
    {
        public bool Initialized { get; private set; }

        protected GameObject Owner => StateMachine.Owner;
        protected IStateMachine StateMachine;

        #region Public Methods

        public void Awake(IStateMachine stateMachine)
        {
            if (Initialized) return;
            Initialized = true;

            StateMachine = stateMachine;
            if (StateMachine == null)
            {
#if UNITY_EDITOR
                Debug.LogError("State: the state machine is null.");
#endif
            }
            
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

        public void Exit()
        {
            OnExit();
        }

        public void Dispose()
        {
            OnDisposed();
            StateMachine = null;
        }

        public virtual bool CanTransition() => true;
        public virtual bool CanTransitionToItself() => false;

        public void Draw()
        {
#if UNITY_EDITOR
            OnDraw();
#endif
        }

        public void DrawSelected()
        {
#if UNITY_EDITOR
            OnDrawSelected();
#endif
        }

        #endregion

        #region Protected Methods

        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnExit() { }
        protected virtual void OnDisposed() { }
        protected virtual void OnDraw() { }
        protected virtual void OnDrawSelected() { }

        #endregion
    }
}
