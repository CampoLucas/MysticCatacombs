using UnityEngine;

namespace Game.Enemies.States
{
    public class EnemyStateDeath<T> : EnemyStateBase<T>
    {
        
        public override void Start()
        {
            base.Start();
            View.CrossFade(Model.GetData().DeathAnimation.name);
            //UnsubscribeAll();
        }
    }
}