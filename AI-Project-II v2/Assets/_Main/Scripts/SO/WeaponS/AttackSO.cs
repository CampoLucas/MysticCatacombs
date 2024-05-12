using System.Collections;
using System.Collections.Generic;
using Game.Interfaces;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "SO/Items/Attack")]
public class AttackSO : ScriptableObject
{
    public string Animation => animation;
    public float Duration => duration;
    public bool Move => move;
    public float Speed => speed;
    public AnimationCurve SpeedCurve => speedCurve;
    public float TransitionTime => transitionTime;
        
    public AnimationCurve TriggerTime => triggerTime;

    [Header("Animation")] 
    [SerializeField] private string animation;
    [SerializeField] private float duration;

    [Header("Movement")]
    [SerializeField] private bool move;
    [SerializeField] private float speed;
    [SerializeField] private AnimationCurve speedCurve = new();
        
    [Header("Events")] 
    [SerializeField] private AnimationCurve triggerTime = new();

    [Header("Transition")] 
    [SerializeField] private float transitionTime;
    [SerializeField] private AttackSO attackToTransition;

    public void AttackMove(IModel model, float t)
    {
        if (!move) return;

        var s = Speed * (speedCurve?.Evaluate(t) ?? 1);
        model.Move(model.GetForward(), s);
    }

    public bool TryGetTransition(out AttackSO transition)
    {
        transition = attackToTransition;
        return transition != null;
    }
}
