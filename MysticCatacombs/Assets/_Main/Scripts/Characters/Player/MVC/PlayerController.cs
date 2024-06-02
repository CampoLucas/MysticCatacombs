using System;
using System.Collections.Generic;
using Game.Entities;
using Game.Player.States;
using Game.StateMachine.Interfaces;
using Game.StateMachine.Predicates;
using UnityEngine;

namespace Game.Player
{
    public class PlayerController : EntityController
    {
        private PlayerInputHandler _inputs;

        protected override void InitFSM()
        {
            base.InitFSM();
           var states = new List<EntityState>();

            var idle = new IdleState();
            var move = new MoveState();
            var lightAttack = new AttackState(Model.CurrentWeapon().Stats.LightAttack, DoLightAttack);
            var heavyAttack = new AttackState(Model.CurrentWeapon().Stats.HeavyAttack, DoHeavyAttack);
            var damage = new DamageState();
            var dead = new DeathState();
            
            states.Add(idle);
            states.Add(move);
            states.Add(lightAttack);
            states.Add(heavyAttack);
            states.Add(damage);
            states.Add(dead);

            StateMachine.AddState(new Dictionary<string, IState>
            {
                { "Idle", idle },
                { "Move", move},
                { "LightAttack", lightAttack },
                { "HeavyAttack", heavyAttack},
                { "Damage", damage },
                { "Death", dead},
            });

            // Any transition
            StateMachine.AddAnyTransition("Death", new IsDeathPredicate(Model), true);
            StateMachine.AddAnyTransition("Damage", new TakenDamagePredicate(Model), true);
            
            // Idle
            StateMachine.AddTransition("Idle", "Move", () => MoveDirection() != Vector3.zero);
            StateMachine.AddTransition("Idle", "LightAttack", DoLightAttack);
            StateMachine.AddTransition("Idle", "HeavyAttack", DoHeavyAttack);
            
            // Move
            StateMachine.AddTransition("Move", "Idle", () => MoveDirection() == Vector3.zero);
            StateMachine.AddTransition("Move", "LightAttack", DoLightAttack);
            StateMachine.AddTransition("Move", "HeavyAttack", DoHeavyAttack);
            
            // LightAttack
            StateMachine.AddTransition("LightAttack", "HeavyAttack", DoHeavyAttack);
            StateMachine.AddTransition("LightAttack", "Move", () => MoveDirection() != Vector3.zero);
            StateMachine.AddTransition("LightAttack", "Idle", () => MoveDirection() == Vector3.zero);
            
            // HeavyAttack
            StateMachine.AddTransition("HeavyAttack", "LightAttack", DoLightAttack);
            StateMachine.AddTransition("HeavyAttack", "Move", () => MoveDirection() != Vector3.zero);
            StateMachine.AddTransition("HeavyAttack", "Idle", () => MoveDirection() == Vector3.zero);

            foreach (var state in states)
            {
                state.Init(this);
            }
            
            StateMachine.SetState("Idle");
        }

        public override Vector3 MoveDirection()
        {
            return _inputs.MoveDir;
        }

        public override bool DoLightAttack()
        {
            return _inputs.FlagLightAttack;
        }

        public override bool DoHeavyAttack()
        {
            return _inputs.FlagHeavyAttack;
        }

        public override float MoveAmount()
        {
            return _inputs.MoveAmount;
        }

        protected override void Awake()
        {
            base.Awake();
            _inputs = GetComponent<PlayerInputHandler>();
        }

        public override void Dispose()
        {
            base.Dispose();
            _inputs = null;
        }
    }
}