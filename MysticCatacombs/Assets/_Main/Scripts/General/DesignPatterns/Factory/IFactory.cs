namespace Game.DesignPatterns.Factory
{
    public interface IFactory<T1, T2> where T1 : IProduct<T2>
    {
        T1 Product { get; }

        T1 Create();
        T1[] Create(int quantity, float delay = 0);
    }
}