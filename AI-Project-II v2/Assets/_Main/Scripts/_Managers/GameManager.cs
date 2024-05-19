using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : MonoManager<GameManager>
    {
        public static readonly string GameWonMessage = "GameWon";
        public static readonly string GameLostMessage = "GameLost";
        
        public bool IsGameOver { get; private set; }
        
        [SerializeField] private GameOverScreen gameOverPrefab;
        [SerializeField] private GameOverScreen gameWonPrefab;
        
        private GameOverScreen _gameOverScreen;
        private GameOverScreen _gameWonScreen;
        private Transform _canvas;

        protected override void OnAwake()
        {
            base.OnAwake();
            AddEvent(GameWonMessage, GameWonHandler);
            AddEvent(GameLostMessage, GameLostHandler);
        }

        public void InitScreens()
        {
            _gameOverScreen = Instantiate(gameOverPrefab, _canvas);
            _gameOverScreen.gameObject.SetActive(false);
            _gameWonScreen = Instantiate(gameWonPrefab, _canvas);
            _gameWonScreen.gameObject.SetActive(false);
        }

        public void SetCanvasTransform(Transform canvas)
        {
            _canvas = canvas;
            InitScreens();
        }

        private void GameLostHandler(object[] args)
        {
            GameOver();
        }
        
        private void GameWonHandler(object[] args)
        {
            GameWon();
        }
        
        public void GameOver()
        {
            if (!IsGameOver)
            {
                _gameOverScreen.gameObject.SetActive(true);
                _gameOverScreen.Init();
                IsGameOver = true;
            }
            
        }
        
        public void GameWon()
        {
            if (!IsGameOver)
            {
                _gameWonScreen.gameObject.SetActive(true);
                _gameWonScreen.Init();
                IsGameOver = true;
            }
        }
    }
}
