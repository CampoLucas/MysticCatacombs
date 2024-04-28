using Game.Interfaces;
using UnityEngine;

namespace Game.Enemies.States
{
    public class WizardStateSeek<T> : WizardStatePursuit<T>
    {
        public WizardStateSeek(ISteering steering, ISteering obsAvoidance) : base(steering, obsAvoidance) {}

        public override void Start()
        {
            base.Start();
            Model.SetTimer(Random.Range(8f, 16f));
        }

        public override void Execute()
        {
            base.Execute();

            if (Model.GetTimerComplete())
            {
                Model.RunTimer();
            }
            else
            {
                Model.SetFollowing(false);
            }
        }

        protected override void Follow()
        {
            
        }
    }
}