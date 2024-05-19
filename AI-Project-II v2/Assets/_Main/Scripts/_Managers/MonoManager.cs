using System;
using System.Collections.Generic;
using System.Linq;
using Game.DesignPatterns.Observer;
using UnityEngine;

namespace Game.Managers
{
    public class MonoManager<T> : MonoBehaviour, IDisposable, IObserver, ISubject
        where T : MonoManager<T>
    {
        public List<IObserver> Subscribers { get; private set; } = new();
        
        private static T _instance;
        private static readonly object _padlock = new();
        private static bool _applicationQuitting;

        private HashSet<IObserver> _subscribers = new();
        private Dictionary<string, Action<object[]>> _events = new();
        
        public static T Instance
        {
            get
            {
                if (_applicationQuitting)
                {
    #if UNITY_EDITOR
                    Debug.LogWarning($"Manager Instance '{typeof(T)}' already destroyed. Returning null.");
    #endif
                    return null;
                }

                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        Debug.LogWarning("Instance is null");
                        //_instance = CreateInstance();
                    }
                    return _instance;
                }
            }
        }

        // private static T CreateInstance()
        // {
        //     if (_instance != null) return _instance;
        //
        //     // var managers = FindObjectsOfType<T>();
        //     //
        //     // if (managers.Length > 0)
        //     // {
        //     //     _instance = managers[0];
        //     //     if (managers.Length > 1)
        //     //     {
        //     //         Debug.LogWarning($"Multiple instances of singleton {typeof(T)} found, but only one is allowed.");
        //     //         for (int i = 1; i < managers.Length; i++)
        //     //         {
        //     //             Destroy(managers[i]);
        //     //         }
        //     //     }
        //     // }
        //
        //     if (_instance == null)
        //     {
        //         var singletonObject = new GameObject($"{typeof(T)} (Manager)");
        //         _instance = singletonObject.AddComponent<T>();
        //         if (!DestroyOnLoadStatic())
        //         {
        //             DontDestroyOnLoad(singletonObject);
        //         }
        //
        //         OnCreatedStatic();
        //     }
        //     else
        //     {
        //         Debug.Log("null");
        //     }
        //
        //     return _instance;
        // }

        private void Awake()
        {
            //if (_padlock == null) _padlock = new();
            lock (_padlock)
            {
                if (_instance == null)
                {
                    _instance = this as T;
                    if (!DestroyOnLoad())
                    {
                        DontDestroyOnLoad(gameObject);
                    }
                    OnAwake();
                }
                else if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnEnable()
        {
            _applicationQuitting = false;
        }

        protected virtual bool DestroyOnLoad() => true;
        protected virtual bool CreateIfNull() => false;

        //private static bool DontDestroyOnLoadStatic() => (_instance as MonoManager<T>)?.DontDestroyOnLoad() ?? false;
        //private static bool DestroyOnLoadStatic() => _instance && _instance.DestroyOnLoad();
        //private static void OnCreatedStatic() => _instance.OnCreated();

        //protected virtual void OnCreated() { }
        protected virtual void OnAwake() { }
        protected virtual void OnDisposed() { }
        
        public void Dispose()
        {
            OnDisposed();
            //_padlock = null;
            for (var i = 0; i < Subscribers.Count; i++)
            {
                Unsubscribe(Subscribers[i]);
            }

            Subscribers = null;
            _subscribers = null;
            _events = null;
        }

        private void OnDestroy()
        {
            _applicationQuitting = true;
            Dispose();
        }

        public void OnNotify(string message, params object[] args)
        {
            if (_events.TryGetValue(message, out var ev)) ev(args);
            
        }
        
        public void Subscribe(IObserver observer)
        {
            if (_subscribers.Add(observer))
            {
                Subscribers.Add(observer);
            }
        }

        public void Unsubscribe(IObserver observer)
        {
            if (_subscribers.Remove(observer))
            {
                Subscribers.Remove(observer);
            }
        }

        public void NotifyAll(string message, params object[] args)
        {
            for (var i = 0; i < Subscribers.Count; i++)
            {
                Subscribers[i].OnNotify(message, args);
            }
        }

        protected bool AddEvent(string message, Action<object[]> action)
        {
            return _events.TryAdd(message, action);
        }
    }
    
}