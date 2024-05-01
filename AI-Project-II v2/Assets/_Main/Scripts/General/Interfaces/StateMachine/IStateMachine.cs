using System;
using System.Collections.Generic;
using Game.FSM;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IStateMachine<T> : IDisposable
    {
        T CurrentID { get; }
        IState<T> Current { get; }

        void SetInitState(IState<T> state);
        void OnUpdate();

        void AddState(IState<T> state);
        void AddState(List<IState<T>> states);
        void SetState(T input);
        void Draw(Transform origin);
    }
}