namespace Game.DesignPatterns.Factory
{
    public interface IProduct<T>
    {
        T Data { get; }
    }
}