using System;
using System.Collections.Generic;
using Game.Interfaces;

namespace Game.StateMachine.Interfaces
{
    /// <summary>
    /// Interface for a state in a state machine.
    /// Includes methods and properties that define the lifecycle of a state.
    /// </summary>
    public interface IState : IDisposable
    {
        /// <summary>
        /// Used to check if it has been initialized.
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// Method called when the state is first added to the state machine.
        /// </summary>
        /// <param name="stateMachine">The state machine it belongs to.</param>
        void Awake(IStateMachine stateMachine);
        
        /// <summary>
        /// Method called when the state becomes active.
        /// Intended to perform any actions required when the state starts.
        /// </summary>
        void Start();
        
        /// <summary>
        /// Method called on each frame while the state is active.
        /// Intended to handle the state's behavior that needs to be updated regularly
        /// </summary>
        void Update();
        
        /// <summary>
        /// Method called when the state exits or transitions to another state.
        /// Intended to perform any cleanup or actions required when the state is about to become inactive.
        /// </summary>
        void Exit();
        
        /// <summary>
        /// Method to check if the state can transition to another state.
        /// </summary>
        /// <returns>Returns true if the state is ready to transition, otherwise false.</returns>
        bool CanTransition();
        /// <summary>
        /// Method to check if the state can transition to itself.
        /// </summary>
        /// <returns>Returns true if the state can transition to itself, otherwise false.</returns>
        bool CanTransitionToItself();

        void Draw();
        void DrawSelected();
    }
}
