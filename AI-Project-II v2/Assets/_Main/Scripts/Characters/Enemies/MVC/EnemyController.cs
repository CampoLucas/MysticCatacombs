using System;
using System.Collections.Generic;
using Game.DecisionTree;
using Game.Enemies.States;
using Game.Entities;
using UnityEngine;
using Game.FSM;
using Game.Sheared;
using Game.Entities.Steering;
using Game.Interfaces;
using Game.Player;
using Game.SO;
using Unity.VisualScripting;

namespace Game.Enemies
{
    public class EnemyController : EntityController<EnemyStatesEnum>
    {
        public IModel Target { get; private set; }

        protected ITreeNode Root;

        protected ISteering Seek;
        protected ISteering Pursuit;
        protected ISteering ObsAvoidance;
        
        private EnemySO _data;
        [SerializeField] private PlayerModel player;

        
        protected virtual void InitSteering()
        {
            var transform1 = transform;
            var transform2 = Target.Transform;
            
            if (Seek != null) Seek.Dispose();
            Seek = new Seek(transform1, transform2);
            
            if (Pursuit != null) Pursuit.Dispose();
            Pursuit = new Pursuit(transform1, Target, _data.PursuitTime);
            
            if (ObsAvoidance != null) ObsAvoidance.Dispose();
            ObsAvoidance = new ObstacleAvoidance(transform1, _data.ObsAngle, _data.ObsRange, _data.MaxObs, _data.ObsMask);
        }

        protected override void InitFSM()
        {
            base.InitFSM();
            var states = new List<EnemyStateBase<EnemyStatesEnum>>();

            var idle = new EnemyStateIdle<EnemyStatesEnum>();
            var seek = new EnemyStateSeek<EnemyStatesEnum>(Seek, ObsAvoidance);
            var pursuit = new EnemyStatePursuit<EnemyStatesEnum>(Pursuit, ObsAvoidance);
            var damage = new EnemyStateDamage<EnemyStatesEnum>();
            var lightAttack = new EnemyStateLightAttack<EnemyStatesEnum>();
            var heavyAttack = new EnemyStateHeavyAttack<EnemyStatesEnum>();
            var dead = new EnemyStateDeath<EnemyStatesEnum>();
            var followRoute = new EnemyStateFollowRoute<EnemyStatesEnum>();
            
            states.Add(idle);
            states.Add(seek);
            states.Add(pursuit);
            states.Add(damage);
            states.Add(lightAttack);
            states.Add(heavyAttack);
            states.Add(dead);
            states.Add(followRoute);
            
            idle.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Seek, seek },
                { EnemyStatesEnum.Pursuit, pursuit },
                { EnemyStatesEnum.LightAttack, lightAttack },
                { EnemyStatesEnum.HeavyAttack, heavyAttack },
                { EnemyStatesEnum.Damage, damage },
                { EnemyStatesEnum.Die, dead },
                { EnemyStatesEnum.FollowRoute, followRoute },
            });
            
            seek.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.Pursuit, pursuit },
                { EnemyStatesEnum.LightAttack, lightAttack },
                { EnemyStatesEnum.HeavyAttack, heavyAttack },
                { EnemyStatesEnum.Damage, damage },
                { EnemyStatesEnum.Die, dead},
                { EnemyStatesEnum.FollowRoute, followRoute },
            });
            
            pursuit.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.Seek, seek },
                { EnemyStatesEnum.LightAttack, lightAttack },
                { EnemyStatesEnum.HeavyAttack, heavyAttack },
                { EnemyStatesEnum.Damage, damage },
                { EnemyStatesEnum.Die, dead},
                { EnemyStatesEnum.FollowRoute, followRoute },
            });
            
            lightAttack.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.Pursuit, pursuit },
                { EnemyStatesEnum.Seek, seek },
                { EnemyStatesEnum.Damage, damage },
                { EnemyStatesEnum.FollowRoute, followRoute },
                { EnemyStatesEnum.Die, dead},
            });
            
            heavyAttack.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.Pursuit, pursuit },
                { EnemyStatesEnum.Seek, seek },
                { EnemyStatesEnum.Damage, damage },
                { EnemyStatesEnum.Die, dead},
                { EnemyStatesEnum.FollowRoute, followRoute },
            });
            
            damage.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.Pursuit, pursuit },
                { EnemyStatesEnum.Seek, seek },
                { EnemyStatesEnum.Die, dead},
                { EnemyStatesEnum.FollowRoute, followRoute },
            });
            
            followRoute.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.Pursuit, pursuit },
                { EnemyStatesEnum.Seek, seek },
                { EnemyStatesEnum.Damage, damage },
                { EnemyStatesEnum.Die, dead},
            });

            foreach (var state in states)
            {
                state.Init(this);
            }
            StateMachine.SetInitState(idle);
        }

        // protected virtual void InitTree()
        // {
        //     var idle = new TreeAction(ActionIdle);
        //     var chase = new TreeAction(ActionSeek);
        //     var pursuit = new TreeAction(ActionPursuit);
        //     var damage = new TreeAction(ActionDamage);
        //     var lightAttack = new TreeAction(ActionLightAttack);
        //     var heavyAttack = new TreeAction(ActionHeavyAttack);
        //     var die = new TreeAction(ActionDie);
        //     var followRoute = new TreeAction(ActionFollowRoute);
        //
        //     var isHeavyAttack = new TreeQuestion(DoHeavyAttack, heavyAttack, lightAttack);
        //     var willAttack = new TreeQuestion(WillAttack, isHeavyAttack, idle);
        //     var isInAttackRange = new TreeQuestion(IsInAttackingRange, willAttack, pursuit);
        //     var hasARoute = new TreeQuestion(HasARoute, followRoute, idle);
        //     var isPlayerOutOfSight = new TreeQuestion(IsPlayerOutOfSight, chase, hasARoute);
        //     var isPlayerInSight = new TreeQuestion(IsPlayerInSight, isInAttackRange, isPlayerOutOfSight);
        //     var isPlayerAlive = new TreeQuestion(IsTargetAlive, isPlayerInSight, hasARoute);
        //     var hasTakenDamage = new TreeQuestion(HasTakenDamage, damage, isPlayerAlive);
        //     var isAlive = new TreeQuestion(IsAlive, hasTakenDamage, die);
        //
        //     Root = isAlive;
        // }
        
        protected override void Awake()
        {
            base.Awake();
            
            _data = Model.GetData<EnemySO>();
        }

        protected override void Start()
        {
            if (player == null) Debug.LogError("Player is null");
            if (player as IModel == null) Debug.LogError("Player as IModel is null");
            SetNewTarget(player);
            //InitSteering();
            //InitTree();
            base.Start();
            
            //_model.Spawn();
        }

        public ISteering GetSeek() => Seek;
        public ISteering GetPursuit() => Pursuit;
        public ISteering GetObsAvoid() => ObsAvoidance;

        // protected bool IsInAttackingRange()
        // {
        //     return Model != null && GetModel<EnemyModel>().TargetInRange(Player.Transform);
        // }
        //
        // protected bool HasARoute()
        // {
        //     if (Model != null)
        //         return GetModel<EnemyModel>().HasARoute();
        //     return false;
        // }
        //
        // protected bool IsPlayerInSight()
        // {
        //     return Model != null && GetModel<EnemyModel>().IsTargetInSight(Player.Transform);
        // }
        //
        // protected bool IsPlayerOutOfSight()
        // {
        //     if (Model != null)
        //         return !GetModel<EnemyModel>().IsTargetInSight(Player.Transform) && GetModel<EnemyModel>().IsFollowing();
        //     return false;
        // }

        public override bool DoLightAttack()
        {
            return MyRandoms.Roulette(new Dictionary<bool, float>
            {
                { true, 10f },
                { false, 0.5f },
            });
        }

        public bool WillAttack()
        {
            return MyRandoms.Roulette(new Dictionary<bool, float>
            {
                { true, 1.5f },
                { false, 10f },
            });
        }

        public void SetNewTarget(IModel newTarget)
        {
            if (newTarget == null) Debug.LogError("target is null");
            if (Target == newTarget == null) Debug.LogError("target is the same");
            if (newTarget == null || Target == newTarget) return;
            
            Target = newTarget;
            InitSteering();
        }

        // protected bool IsTargetAlive()
        // {
        //     if (Model != null)
        //         return GetModel<EnemyModel>().IsTargetAlive(Player);
        //     return false;
        // }
        //
        // protected bool HasTakenDamage()
        // {
        //     if (Model != null)
        //         return Model.HasTakenDamage();
        //     return false;
        // }
        //
        // protected bool IsAlive()
        // {
        //     if (Model != null)
        //         return Model.IsAlive();
        //     return false;
        // }
        //
        // protected void ActionSeek() => StateMachine.SetState(EnemyStatesEnum.Seek);
        // protected void ActionPursuit() => StateMachine.SetState(EnemyStatesEnum.Pursuit);
        // protected void ActionLightAttack() => StateMachine.SetState(EnemyStatesEnum.LightAttack);
        // protected void ActionHeavyAttack() => StateMachine.SetState(EnemyStatesEnum.HeavyAttack);
        // protected void ActionDamage() => StateMachine.SetState(EnemyStatesEnum.Damage);
        // protected void ActionDie() => StateMachine.SetState(EnemyStatesEnum.Die);
        // protected void ActionIdle() => StateMachine.SetState(EnemyStatesEnum.Idle);
        // protected void ActionFollowRoute() => StateMachine.SetState(EnemyStatesEnum.FollowRoute);

        public override void Dispose()
        {
            base.Dispose();
            if (Root != null) Root.Dispose();
            
            if (Seek != null) Seek.Dispose();
            if (Pursuit != null) Pursuit.Dispose();
            if (ObsAvoidance != null) ObsAvoidance.Dispose();
            Target = null;
            Root = null;
            Seek = null;
            Pursuit = null;
            ObsAvoidance = null;
        }
    }
}