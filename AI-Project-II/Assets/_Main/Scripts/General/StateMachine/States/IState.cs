using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StateMachine
{
    public interface IState : IDisposable
    {
        /// <summary>
        /// Called in the FSM awake method. 
        /// </summary>
        void Awake(FSM stateMachine);

        /// <summary>
        /// Called after changing to the state.
        /// </summary>
        void Start();

        /// <summary>
        /// Called in the update method of the fsm.
        /// </summary>
        void Update();

        /// <summary>
        /// Called in the fixed update of the fsm.
        /// </summary>
        void FixedUpdate();

        /// <summary>
        /// Called before changing to another state.
        /// </summary>
        void Exit();

        /// <summary>
        /// Called when the fsm object is enabled.
        /// </summary>
        void Enable();

        /// <summary>
        /// Called when the fsm object is disable.
        /// </summary>
        void Disable();

        /// <summary>
        /// Determines if the state is able to transition.
        /// </summary>
        /// <returns></returns>
        bool CanTransition();
    }
}