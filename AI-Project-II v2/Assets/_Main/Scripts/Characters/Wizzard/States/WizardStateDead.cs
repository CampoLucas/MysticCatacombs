using UnityEngine;

namespace Game.Enemies.States
{
    public class WizardStateDeath<T> : WizardStateBase<T>
    {
        
        public override void Start()
        {
            base.Start();
            View.CrossFade(Model.GetData().DeathAnimation.name);
            UnsubscribeAll();
        }
    }
}