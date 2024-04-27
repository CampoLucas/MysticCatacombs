using System;
using System.Linq;
using Game.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

namespace Game.Entities
{
    public class EntityView : MonoBehaviour, IView
    {
        private Animator _animator;
        private static readonly int MoveAmount = Animator.StringToHash("MoveAmount");
        
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void UpdateMovementValues(float moveAmount)
        {
            var amount = moveAmount switch
            {
                > 0f and < 0.55f => 0.5f,
                > 0.55f => 1,
                < 0 and > -0.55f => -0.5f,
                < -0.55f => -1,
                _ => 0
            };

            _animator.SetFloat(MoveAmount, amount, 0.1f, Time.deltaTime);
        }
        
        public void CrossFade(string stateName, float transitionDuration = 0.2f)
        {
            _animator.CrossFade(stateName, transitionDuration);
        }

        public void CrossFade(int stateHash, float transitionDuration = 0.2f)
        {
            _animator.CrossFade(stateHash, transitionDuration);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            _animator = null;
        }
    }
}