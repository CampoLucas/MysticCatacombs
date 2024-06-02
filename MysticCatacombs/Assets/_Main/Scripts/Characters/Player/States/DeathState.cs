using UnityEngine;

namespace Game.Player.States
{
    public class DeathState : EntityState
    {
        protected override void OnStart()
        {
            base.OnStart();
            StateMachine.Enable = false;
            View.CrossFade(Model.GetData().DeathAnimation.name);
        }
    }
}