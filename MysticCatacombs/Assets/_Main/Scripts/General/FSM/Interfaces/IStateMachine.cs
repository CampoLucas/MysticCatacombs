using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StateMachine.Interfaces
{
    /// <summary>
    /// Interface that represents a state machine.
    /// It includes methods and properties to manage states and transitions between them.
    /// </summary>
    public interface IStateMachine : IDisposable
    {
        /// <summary>
        /// The GameObject that owns this state machine.
        /// </summary>
        GameObject Owner { get; }
        /// <summary>
        /// The default state the state machine returns to.
        /// </summary>
        string DefaultState { get; }
        /// <summary>
        /// Name of the current state.
        /// </summary>
        string CurrentState { get; }
        /// <summary>
        /// Whether the state machine is enabled.
        /// When the state machine is enabled, it will update and process transitions.
        /// When disabled, the state machine will not update or transition states.
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        /// Called each frame to update the state machine.
        /// </summary>
        void Update();
        /// <summary>
        /// A method to set the default state of the state machine.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool SetDefaultState(string name);
        /// <summary>
        /// A method to set the current state by name.
        /// </summary>
        /// <param name="name">The name of the state to transition to.</param>
        /// <returns>Returns true if the state was successfully set.</returns>
        bool SetState(string name);
        /// <summary>
        /// A method to set the current state to the default.
        /// </summary>
        /// <param name="name">The name of the state to transition to.</param>
        /// <returns>Returns true if the state was successfully set.</returns>
        bool SetToDefault();
        /// <summary>
        /// Method to add a transition from one state to another based on a condition.
        /// </summary>
        /// <param name="from">The name of the state to transition from</param>
        /// <param name="to">The name of the state to transition to</param>
        /// <param name="condition">The condition that must be met for the transition to occur.</param>
        /// <param name="forceTransition">If it can transition even when the state is unable to transition.</param>
        /// <returns>Returns true if the transition was successfully added.</returns>
        bool AddTransition(string from, string to, IPredicate condition, bool forceTransition = false);
        /// <summary>
        /// Method to add a transition from one state to another based on a condition.
        /// This overload uses a Func for the condition.
        /// </summary>
        /// <param name="from">The name of the state to transition from</param>
        /// <param name="to">The name of the state to transition to</param>
        /// <param name="condition">The condition that must be met for the transition to occur.</param>
        /// <param name="forceTransition">If it can transition even when the state is unable to transition.</param>
        /// <returns>Returns true if the transition was successfully added.</returns>
        bool AddTransition(string from, string to, Func<bool> condition, bool forceTransition = false);
        /// <summary>
        /// Method to add a transition from any state to a specific state based on a condition.
        /// </summary>
        /// <param name="to">The name of the state to transition to.</param>
        /// <param name="condition">The condition that must be met for the transition to occur.</param>
        /// <param name="forceTransition">If it can transition even when the state is unable to transition.</param>
        /// <returns>Returns true if the transition was successfully added.</returns>
        bool AddAnyTransition(string to, IPredicate condition, bool forceTransition = false);
        /// <summary>
        /// Method to add a transition from any state to a specific state based on a condition.
        /// This overload uses a Func for the condition.
        /// </summary>
        /// <param name="to">The name of the state to transition to.</param>
        /// <param name="condition">The condition that must be met for the transition to occur.</param>
        /// <param name="forceTransition">If it can transition even when the state is unable to transition.</param>
        /// <returns>Returns true if the transition was successfully added.</returns>
        bool AddAnyTransition(string to, Func<bool> condition, bool forceTransition = false);
        /// <summary>
        /// Adds a state to the state machine.
        /// </summary>
        /// <param name="name">The name of the state to add.</param>
        /// <param name="state">The state instance to add.</param>
        /// <returns>Returns true if the state was successfully added.</returns>
        bool AddState(string name, IState state);
        /// <summary>
        /// Adds multiple states to the state machine.
        /// </summary>
        /// <param name="states">A dictionary of states to add, where the key is the state name and the value is the state instance.</param>
        void AddState(Dictionary<string, IState> states);
        /// <summary>
        /// Removes a state from the state machine.
        /// </summary>
        /// <param name="name">The name of the state to remove.</param>
        /// <returns>Returns true if the state was successfully removed.</returns>
        bool RemoveState(string name);
        /// <summary>
        /// Gets a state by name.
        /// </summary>
        /// <param name="name">The name of the state to retrieve.</param>
        /// <returns>The state instance corresponding to the specified name.</returns>
        IState GetState(string name);
        /// <summary>
        /// Gets a state by name with a specific type.
        /// </summary>
        /// <param name="name">The name of the state to retrieve.</param>
        /// <returns>The state instance corresponding to the specified name, cast to the specified type.</returns>
        TState GetStateType<TState>(string name) where TState : IState;
        /// <summary>
        /// Used to check if a state exists in the state machine.
        /// </summary>
        /// <param name="name">The name of the state to check.</param>
        /// <returns>Returns true if the state exists.</returns>
        bool ContainsState(string name);

        bool CanTransition();
        void Draw();
        void DrawOnSelected();
    }
}