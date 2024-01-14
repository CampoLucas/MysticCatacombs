using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private HeartContainer heartPrefab;
        [SerializeField] private List<HeartContainer> hearts;

        private int _maxHearts;
        private float _currentHearts;
        private HeartContainer _currentContainer;

        public void SetUpHearts(int heartsIn)
        {
            hearts.Clear();

            for (var i = transform.childCount - 1; i >= 0; i--) Destroy(transform.GetChild(i).gameObject);

            _maxHearts = heartsIn;
            _currentHearts = _maxHearts;

            for (var i = 0; i < _maxHearts; i++)
            {
                var current = Instantiate(heartPrefab, transform);
                hearts.Add(current);

                if (_currentContainer != null) _currentContainer.SetNext(current);

                _currentContainer = current;
            }

            _currentContainer = hearts[0];
        }

        public void SetCurrentHealth(float health)
        {
            _currentHearts = health;
            _currentContainer.SetHeart(_currentHearts);
        }

        public void AddHearts(float healthUp)
        {
            _currentHearts += healthUp;
            if (_currentHearts > _maxHearts) _currentHearts = _maxHearts;

            _currentContainer.SetHeart(_currentHearts);
        }

        public void RemoveHearts(float healthDown)
        {
            _currentHearts -= healthDown;
            if (_currentHearts < 0) _currentHearts = 0f;

            _currentContainer.SetHeart(_currentHearts);
        }

        public void AddContainer()
        {
            var newHeart = Instantiate(heartPrefab, transform);
            _currentContainer = hearts[^1];
            hearts.Add(newHeart);

            if (_currentContainer != null) _currentContainer.SetNext(newHeart);

            _currentContainer = hearts[0].GetComponent<HeartContainer>();

            _maxHearts++;
            _currentHearts = _maxHearts;
            SetCurrentHealth(_currentHearts);
        }
    }
}