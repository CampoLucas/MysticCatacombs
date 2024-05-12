using System.Collections;
using System.Collections.Generic;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Game/Attacks/Attack")]
public class AttackSO : ScriptableObject
{
    [Header("General")] 
    [SerializeField] private string name;
    
    [Header("Animation")] 
    [SerializeField] private AnimationModule animationModule;

    [Header("Movement")]
    [SerializeField] private MovementModule movementModule;
        
    [Header("Events")] 
    [SerializeField] private EventModule eventModule;

    [Header("Transition")] 
    [SerializeField] private TransitionModuleData transitionModule;

    public Attack GetAttack() => new (name, animationModule, movementModule, eventModule, transitionModule);
}
