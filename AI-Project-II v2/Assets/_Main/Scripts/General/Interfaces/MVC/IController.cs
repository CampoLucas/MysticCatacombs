using System;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IController<T> : IDisposable
    {
        IStateMachine<T> StateMachine { get; }
        IModel Model { get; }
        IView View { get; }

        float MoveAmount();
        Vector3 MoveDirection();
        bool DoLightAttack();
        bool DoHeavyAttack();
        TModel GetModel<TModel>() where TModel : IModel;
        TView GetView<TView>() where TView : IView;
    }
}