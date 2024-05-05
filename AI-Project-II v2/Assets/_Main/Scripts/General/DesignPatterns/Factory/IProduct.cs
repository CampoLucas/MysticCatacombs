using UnityEngine;

public interface IProduct<T>
{
    T Data { get; }
}