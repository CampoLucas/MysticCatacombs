using System;
using Game.StateMachine.Interfaces;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IController : IDisposable
    {
        IStateMachine StateMachine { get; }
        IModel Model { get; }
        IView View { get; }

        float MoveAmount();
        Vector3 MoveDirection();
        bool DoLightAttack();
        bool DoHeavyAttack();
        TModel GetModel<TModel>() where TModel : IModel;
        TView GetView<TView>() where TView : IView;
        void SetSteering(ISteering steering);
    }
}