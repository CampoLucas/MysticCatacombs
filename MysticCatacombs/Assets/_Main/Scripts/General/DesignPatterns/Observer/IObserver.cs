namespace Game.DesignPatterns.Observer
{
    public interface IObserver
    {
        void OnNotify(string message, params object[] args);
    }
}
