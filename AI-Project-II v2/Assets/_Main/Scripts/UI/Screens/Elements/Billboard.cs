using System;
using UnityEngine;

namespace Game.Scripts.UI.Elements
{
    public class Billboard : MonoBehaviour
    {
        private Transform _cameraTransform;
        private Transform _transform;
        
        private void Start()
        {
            _cameraTransform = Camera.main.transform;
            _transform = transform;
        }

        private void Update()
        {
            _transform.LookAt(_cameraTransform);
            //_transform.rotation = Quaternion.Euler(0, _transform.rotation.eulerAngles.y, 0);
        }

        private void OnDestroy()
        {
            _cameraTransform = null;
            _transform = null;
        }
    }
}