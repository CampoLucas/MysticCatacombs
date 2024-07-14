namespace Game.UpdateManager.Interfaces
{
    public interface ILateUpdatable : IBaseUpdatable
    {
        void DoLateUpdate();
    }
}