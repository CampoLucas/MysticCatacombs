using System;
using Game.DesignPatterns.Observer;
using Game.Managers;
using UnityEngine;

namespace Game.UI.Effects
{
    public class DeactivateOnGameOver : MonoBehaviour, IObserver
    {
        private void Start()
        {
            GameManager.Instance.Subscribe(this);
        }
        
        public void OnNotify(string message, params object[] args)
        {
            if (message == GameManager.GameOverMessage) gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            var gameManager = GameManager.Instance;
            if (gameManager) gameManager.Unsubscribe(this);
        }
    }
}