using System;
using System.Collections;
using Game.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Elements
{
    public class HealthBar : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private Image healthBar;
        [SerializeField] private Image backgroung;

        [Header("Effect")] 
        [SerializeField] private Image effect;
        [SerializeField] private float normalSpeed;
        [SerializeField] private float fastSpeed;

        [Header("Damageable")] 
        [SerializeField] private Damageable damageable;
        
        private float _currSpeed;
        private float _catchHealth;
        private Coroutine _coroutine;

        private void Start()
        {
            EnableHealthBar(false);
            _catchHealth = damageable.CurrentHealth;
            damageable.OnHealthUpdated += UpdateHealthBarHandler;
        }

        public void EnableHealthBar(bool value)
        {
            healthBar.enabled = value;
            backgroung.enabled = value;
            effect.enabled = value;
        }

        private void UpdateHealthBarHandler(float currentHealth)
        {
            EnableHealthBar(true);
            var healthDiff = _catchHealth - currentHealth;
            _catchHealth = currentHealth;

            healthBar.fillAmount = currentHealth / damageable.MaxHealth();

            _currSpeed = currentHealth == 0 ? fastSpeed : normalSpeed;
            DoCoroutine(HealthBarEffect(_currSpeed));
        }

        private IEnumerator HealthBarEffect(float speed)
        {
            while (effect.fillAmount > healthBar.fillAmount)
            {
                effect.fillAmount -= speed * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.3f);
            EnableHealthBar(false);
        }

        private void DoCoroutine(IEnumerator enumerator)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(enumerator);
        }

        private void OnDestroy()
        {
            damageable.OnHealthUpdated -= UpdateHealthBarHandler;
            damageable = null;
            
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = null;

            healthBar = null;
            effect = null;
        }
    }
}