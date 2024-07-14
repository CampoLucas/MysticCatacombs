using System;
using System.Collections;
using System.Collections.Generic;
using Game.UpdateManager.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UpdateManager
{
    public class UpdateManager : MonoBehaviour, IDisposable
    {
        public static UpdateManager Instance
        {
            get
            {
                if (_applicationQuitting)
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"UpdateManager instance is already destroyed. Returning null.");
#endif
                    return null;
                }

                lock (_padlock)
                {
                    if (_instance == null)
                    {
#if UNITY_EDITOR
                        Debug.LogWarning("Instance is null, creating instance.");
#endif
                        Debug.Log("Create");
                        
                        FindInstance();
                        //CreateInstance();
                        OnInitStatic();
                    }

                    return _instance;
                }
            }
        }

        [FormerlySerializedAs("layerSettings")] [SerializeField] private UpdatableLayersSO layersSettings;
        
        private static UpdateManager _instance;
        private static readonly object _padlock = new();
        private static bool _applicationQuitting;

        private bool _initialized;

        private Dictionary<UpdateLayer, UpdateableLayer> _updateLayersDictionary = new();
        private Dictionary<UpdateLayer, LateUpdateableLayer> _lateUpdateLayersDictionary = new();
        private Dictionary<UpdateLayer, FixedUpdateableLayer> _fixedUpdateLayersDictionary = new();
        private List<UpdateableLayer> _updateLayers = new();
        private List<LateUpdateableLayer> _lateUpdateLayers = new();
        private List<FixedUpdateableLayer> _fixedUpdateLayers = new();

        private static void FindInstance()
        {
            if (_instance != null) return;
            
            var managers = FindObjectsOfType<UpdateManager>();

            if (managers.Length > 0)
            {
                _instance = managers[0];
                if (managers.Length > 1)
                {
                    Debug.LogWarning($"Multiple instances of UpdateManager found, but only one is allowed.");
                    for (var i = 1; i < managers.Length; i++)
                    {
                        Destroy(managers[i]);
                    }
                }
            }
        }
        
        // private static void CreateInstance()
        // {
        //     if (_instance != null) return;
        //
        //     if (_instance == null)
        //     {
        //         var singletonObject = new GameObject($"UpdateManager");
        //         _instance = singletonObject.AddComponent<UpdateManager>();
        //         //DontDestroyOnLoad(singletonObject);
        //
        //     }
        //     else
        //     {
        //         Debug.Log("null");
        //     }
        //
        // }

        private static void OnInitStatic()
        {
            _instance.Init();
        }

        public void Init()
        {
            if (_initialized) return;
            _initialized = true;
            
            Debug.Log("UpdateManager Init");
        }

        private void Awake()
        {
            Debug.Log("UpdateManager Awake");
            lock (_padlock)
            {
                if (_instance == null)
                {
                    _instance = this;
                    Init();
                }
                else if (_instance != this)
                {
                    Destroy(this);
                }
            }
        }

        private void OnEnable()
        {
            _applicationQuitting = false;
        }

        private void Update()
        {
            if (_updateLayers.Count == 0) return;

            for (var i = 0; i < _updateLayers.Count; i++)
            {
                _updateLayers[i].Tick();
            }
        }

        private void FixedUpdate()
        {
            if (_fixedUpdateLayers.Count == 0) return;
            
            for (var i = 0; i < _fixedUpdateLayers.Count; i++)
            {
                _fixedUpdateLayers[i].Tick();
            }
        }

        private void LateUpdate()
        {
            if (_lateUpdateLayers.Count == 0) return;
            
            for (var i = 0; i < _lateUpdateLayers.Count; i++)
            {
                _lateUpdateLayers[i].Tick();
            }
        }

        public void AddUpdatable(IUpdatable updatable)
        {
            var updateLayer = updatable.UpdateLayer();
            if (!_updateLayersDictionary.TryGetValue(updateLayer, out var layer))
            {
                layer = new UpdateableLayer();
                _updateLayersDictionary[updateLayer] = layer;
                _updateLayers.Add(layer);
            }

            layer.Add(updatable);
        }
        
        public void AddFixedUpdatable(IFixedUpdatable updatable)
        {
            var updateLayer = updatable.UpdateLayer();
            if (!_fixedUpdateLayersDictionary.TryGetValue(updateLayer, out var layer))
            {
                layer = new FixedUpdateableLayer();
                _fixedUpdateLayersDictionary[updateLayer] = layer;
                _fixedUpdateLayers.Add(layer);
            }

            layer.Add(updatable);
        }

        public void AddLateUpdatable(ILateUpdatable updatable)
        {
            var updateLayer = updatable.UpdateLayer();
            if (!_lateUpdateLayersDictionary.TryGetValue(updateLayer, out var layer))
            {
                layer = new LateUpdateableLayer();
                _lateUpdateLayersDictionary[updateLayer] = layer;
                _lateUpdateLayers.Add(layer);
            }

            layer.Add(updatable);
        }
        
        public void RemoveUpdatable(IUpdatable updatable)
        {
            var updateLayer = updatable.UpdateLayer();
            if (_updateLayersDictionary.TryGetValue(updateLayer, out var layer))
            {
                layer.Remove(updatable);

                if (layer.Count == 0)
                {
                    _updateLayersDictionary.Remove(updateLayer);
                    _updateLayers.Remove(layer);
                    layer.Dispose();
                }
            }
        }

        public void RemoveFixedUpdatable(IFixedUpdatable updatable)
        {
            var updateLayer = updatable.UpdateLayer();
            if (_fixedUpdateLayersDictionary.TryGetValue(updateLayer, out var layer))
            {
                layer.Remove(updatable);

                if (layer.Count == 0)
                {
                    _fixedUpdateLayersDictionary.Remove(updateLayer);
                    _fixedUpdateLayers.Remove(layer);
                    layer.Dispose();
                }
            }
        }

        public void RemoveLateUpdatable(ILateUpdatable updatable)
        {
            var updateLayer = updatable.UpdateLayer();
            if (_lateUpdateLayersDictionary.TryGetValue(updateLayer, out var layer))
            {
                layer.Remove(updatable);

                if (layer.Count == 0)
                {
                    _lateUpdateLayersDictionary.Remove(updateLayer);
                    _lateUpdateLayers.Remove(layer);
                    layer.Dispose();
                }
            }
        }

        public void Test()
        {
            Debug.Log("Testing");
        }

        public void Dispose()
        {
            layersSettings = null;
            if (_instance == this)
            {
                _instance = null;
                _applicationQuitting = true;
            }

            _initialized = false;

            _updateLayers = null;
            _fixedUpdateLayers = null;
            _lateUpdateLayers = null;

            _updateLayersDictionary = null;
            _fixedUpdateLayersDictionary = null;
            _lateUpdateLayersDictionary = null;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
