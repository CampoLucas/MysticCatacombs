namespace Game.WeaponSystem.Modules
{
    public class WeaponModule : Weapon
    {
        protected BaseWeapon MainWeapon;

        public void Init(BaseWeapon weapon)
        {
            MainWeapon = weapon;
            OnInit();
        }

        public void SetOwner()
        {
            OnSetOwner();
        }

        protected virtual void OnInit() { }
        protected override void OnBegin() { }
        protected override void OnEnd() { }

        protected override void OnDispose()
        {
            MainWeapon = null;
        }

        protected virtual void OnSetOwner() { }
    }
}