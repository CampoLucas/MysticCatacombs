using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateDead<T> : PlayerStateBase<T>
    {
        
        public override void Start()
        {
            base.Start();
            View.CrossFade(Model.GetData().DeathAnimation.name);
            UnsubscribeAll();
        }
    }
}