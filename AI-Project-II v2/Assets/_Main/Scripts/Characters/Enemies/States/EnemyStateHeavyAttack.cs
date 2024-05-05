namespace Game.Enemies.States
{
    public class EnemyStateHeavyAttack<T> : EnemyStateLightAttack<T>
    {
        protected override void Attack()
        {
            Model.HeavyAttack();
            View.CrossFade(Model.CurrentWeapon().Stats.HeavyAttack01.EventHash);
            var timer = Model.CurrentWeapon().Stats.HeavyAttack01.Duration;
            Model.SetTimer(timer);
        }

        protected override void CancelAttack()
        {
            base.CancelAttack();
            Model.CancelHeavyAttack();
        }
    }
}