using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UpdateManager
{
    public class UpdateManager : MonoBehaviour
    {
        public static UpdateManager Instance { get; private set; }

        [SerializeField] private EnumDataContainer<UpdateLayer, UpdateInfo> layers;
        
        private List<UpdatableLayer> _layers = new();
        private Dictionary<UpdateLayer, UpdatableLayer> _layersDictionary = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            
            InitLayers();
        }

        private void InitLayers()
        {
            
        }

        private void OnDestroy()
        {
            layers.Dispose();
            layers = null;
        }
    }
}