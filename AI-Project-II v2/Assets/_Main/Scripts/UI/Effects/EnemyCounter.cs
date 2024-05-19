using System;
using System.Collections;
using System.Collections.Generic;
using Game.DesignPatterns.Observer;
using Game.Managers;
using TMPro;
using UnityEngine;

public class EnemyCounter : MonoBehaviour, IObserver
{
    [SerializeField] private TMP_Text text;
    
    private void Start()
    {
        var enemyManager = EnemyManager.Instance;
        if (enemyManager) enemyManager.Subscribe(this);
        UpdateText();
    }

    public void OnNotify(string message, params object[] args)
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (!EnemyManager.Instance) return;
        text.text = $"Enemies: {EnemyManager.Instance.CurrentEnemies}/{EnemyManager.Instance.TotalEnemies}";
    }

    private void OnDestroy()
    {
        var enemyManager = EnemyManager.Instance;
        if (enemyManager) enemyManager.Unsubscribe(this);
    }
}
