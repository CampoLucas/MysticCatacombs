using System;
using System.Collections.Generic;
using Game.DecisionTree;
using Game.Enemies.States;
using Game.Entities;
using Game.Entities.Steering;
using UnityEngine;
using Game.FSM;
using Game.Sheared;
using Game.Interfaces;
using Game.Pathfinding;
using Game.Player;
using Game.Player.States;
using Game.SO;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class EnemyController : EntityController<EnemyStatesEnum>
    {
        public IModel Target { get; private set; }

        [SerializeField] private PlayerModel player;
        [SerializeField] private Pathfinder pathfinder; 
        private EnemySO _data;
        private ISteering CurrentSteering;

        protected override void InitFSM()
        {
            base.InitFSM();
            var states = new List<EnemyStateBase<EnemyStatesEnum>>();

            var idle = new EnemyStateIdle<EnemyStatesEnum>();
            //var seek = new EnemyStateSeek<EnemyStatesEnum>(Seek, ObsAvoidance);
            var t = transform;
            var seek = new EnemyStateMove<EnemyStatesEnum>(
                new SeekPathfinder(t, 1, pathfinder), new[]
                {
                    new ObstacleAvoidanceDecorator(t, _data.ObsAngle, _data.ObsRange, _data.MaxObs, 0.5f,
                        _data.ObsMask)
                });
            seek.OnStart += OnSeekStartHandler;
            seek.OnExecute += OnSeekExecuteHandler;
            
            
            //var pursuit = new EnemyStatePursuit<EnemyStatesEnum>(Pursuit, ObsAvoidance);
            var pursuit = new EnemyStateMove<EnemyStatesEnum>(
                new Pursuit(t, 1, _data.PursuitTime), new []
                {
                    new ObstacleAvoidanceDecorator(t, _data.ObsAngle, _data.ObsRange, _data.MaxObs, 0.5f,
                        _data.ObsMask)
                });
            pursuit.OnStart += OnPursuitStartHandler;
            
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
            StateMachine.AddState(new List<IState<EnemyStatesEnum>>
            {
                idle, seek, pursuit, damage, lightAttack, heavyAttack, dead, followRoute
            });
            
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
            base.Start();
            
            pathfinder.InitPathfinder(transform);
        }

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

        public override Vector3 MoveDirection()
        {
            var targetPos = Target.Transform.position;

            if (CurrentSteering == null)
            {
                Debug.LogError("CurrentSteering is null", this);
                return Vector3.zero;
            }
            return CurrentSteering.GetDir(Target.Transform);
        }

        public override float MoveAmount()
        {
            return 1;
        }

        public void SetSteering(ISteering steering)
        {
            CurrentSteering = steering;
        }

        public void SetNewTarget(IModel newTarget)
        {
            if (newTarget == null || Target == newTarget) return;
            Target = newTarget;
        }

        private void OnPursuitStartHandler()
        {
            GetModel<EnemyModel>().SetFollowing(true);
        }

        private void OnSeekStartHandler()
        {
            Model.SetTimer(Random.Range(8f, 16f));
        }
        
        private void OnSeekExecuteHandler()
        {
            if (Model.GetTimerComplete())
            {
                Model.RunTimer();
            }
            else
            {
                GetModel<EnemyModel>().SetFollowing(false);
            }
        }

        private void OnDrawGizmos()
        {
            if (pathfinder != null) pathfinder.OnDrawGizmos();
            if (StateMachine != null) StateMachine.Draw(transform);
        }

        private void OnDrawGizmosSelected()
        {
            if (pathfinder != null) pathfinder.OnDrawGizmosSelected();
            
            if (CurrentSteering != null) CurrentSteering.Draw();
            
            
        }

        public override void Dispose()
        {
            base.Dispose();
            
            Target = null;
            if (CurrentSteering != null) CurrentSteering.Dispose();
            CurrentSteering = null;
        }
    }
}