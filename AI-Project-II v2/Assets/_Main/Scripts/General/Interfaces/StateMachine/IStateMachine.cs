using System;
using Game.FSM;

namespace Game.Interfaces
{
    public interface IStateMachine<T> : IDisposable
    {
        T CurrentID { get; }
        IState<T> Current { get; }

        void SetInitState(IState<T> state);
        void OnUpdate();
        void SetState(T input);
    }
}