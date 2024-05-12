using System;
using System.Collections.Generic;
using Game.DecisionTree;
using Game.Enemies.States;
using Game.Entities;
using Game.Entities.Flocking;
using Game.Entities.Steering;
using UnityEngine;
using Game.FSM;
using Game.Sheared;
using Game.Interfaces;
using Game.Managers;
using Game.Pathfinding;
using Game.Player;
using Game.Player.States;
using Game.SO;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class EnemyController : EntityController<EnemyStatesEnum>
    {
        public IModel Target { get; private set; }

        [SerializeField] private PlayerModel player;
        [SerializeField] private Pathfinder pathfinder;
        
        private EnemySO _data;
        private ISteering _currentSteering;

        protected override void InitFSM()
        {
            base.InitFSM();
            var states = new List<EnemyStateBase<EnemyStatesEnum>>();

            var idle = new EnemyStateIdle<EnemyStatesEnum>();
            var t = transform;
            
            var seek = new EnemyStateMove<EnemyStatesEnum>(_data.Seek.Get(t, pathfinder), Model.GetData().MoveSpeed, 
                new [] 
                { 
                    _data.ObstacleAvoidance.GetDecorator(t), 
                    _data.Flocking.GetDecorator(GetModel<EnemyModel>()) 
                });
            seek.OnStart += OnSeekStartHandler;
            seek.OnExecute += OnSeekExecuteHandler;
            
            var pursuit = new EnemyStateMove<EnemyStatesEnum>(_data.Pursuit.Get(t), Model.GetData().MoveSpeed, 
                new []
                {
                    _data.ObstacleAvoidance.GetDecorator(t),
                    _data.Flocking.GetDecorator(GetModel<EnemyModel>())
                });
            pursuit.OnStart += OnPursuitStartHandler;
            
            var damage = new EnemyStateDamage<EnemyStatesEnum>();
            var lightAttack = new EnemyStateAttack<EnemyStatesEnum>(Model.CurrentWeapon().Stats.LightAttack);
            var heavyAttack = new EnemyStateAttack<EnemyStatesEnum>(Model.CurrentWeapon().Stats.HeavyAttack);
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

            if (_currentSteering == null)
            {
                Debug.LogError("CurrentSteering is null", this);
                return Vector3.zero;
            }
            return _currentSteering.GetDir(Target.Transform).normalized;
        }

        public override float MoveAmount()
        {
            return 1;
        }

        public void SetSteering(ISteering steering)
        {
            _currentSteering = steering;
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
            
            if (_currentSteering != null) _currentSteering.Draw();
            
            
        }

        public override void Dispose()
        {
            base.Dispose();
            
            Target = null;
            if (_currentSteering != null) _currentSteering.Dispose();
            _currentSteering = null;
            
            if (pathfinder != null) pathfinder.Dispose();
            pathfinder = null;
        }
    }
}