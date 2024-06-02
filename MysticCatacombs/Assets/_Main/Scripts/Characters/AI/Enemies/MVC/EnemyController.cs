using System.Collections.Generic;
using Game.Entities;
using UnityEngine;
using Game.Sheared;
using Game.Interfaces;
using Game.Managers;
using Game.Pathfinding;
using Game.Player;
using Game.Player.States;
using Game.SO;
using Game.StateMachine.Interfaces;
using Game.StateMachine.Predicates;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class EnemyController : EntityController
    {
        public IModel Target { get; private set; }

        [SerializeField] private PlayerModel player;
        [SerializeField] private Pathfinder pathfinder;
        
        private EnemySO _data;
        private ISteering _currentSteering;

        protected override void InitFSM()
        {
            base.InitFSM();
            var states = new List<EntityState>();

            var idle = new IdleState();
            var t = transform;
            
            var seek = new SteeringMoveState(_data.Seek.Get(t, pathfinder), Model.GetData().MoveSpeed, 
                new [] 
                { 
                    _data.ObstacleAvoidance.GetDecorator(t), 
                    _data.Flocking.GetDecorator(GetModel<EnemyModel>()) 
                });
            seek.OnStartState += OnSeekStartHandler;
            seek.OnExitState += OnSeekExecuteHandler;
            
            var pursuit = new SteeringMoveState(_data.Pursuit.Get(t), Model.GetData().MoveSpeed, 
                new []
                {
                    _data.ObstacleAvoidance.GetDecorator(t),
                    _data.Flocking.GetDecorator(GetModel<EnemyModel>())
                });
            pursuit.OnStartState += OnPursuitStartHandler;
            
            var lookAtTarget = new SteeringMoveState(_data.Pursuit.Get(t), Model.GetData().MoveSpeed, move: false, moveAmount: 0.5f);
            lookAtTarget.OnStartState += OnPursuitStartHandler;
            
            var damage = new DamageState();
            var lightAttack = new EnemyAttackState(Model.CurrentWeapon().Stats.LightAttack);
            var heavyAttack = new EnemyAttackState(Model.CurrentWeapon().Stats.HeavyAttack);
            var dead = new DeathState();
            
            states.Add(idle);
            states.Add(seek);
            states.Add(pursuit);
            states.Add(damage);
            states.Add(lightAttack);
            states.Add(heavyAttack);
            states.Add(dead);
            states.Add(lookAtTarget);
            
            StateMachine.AddState(new Dictionary<string, IState>
            {
                { "Idle", idle },
                { "Seek", seek},
                { "Pursuit", pursuit},
                { "LookAtTarget", lookAtTarget},
                { "LightAttack", lightAttack },
                { "HeavyAttack", heavyAttack},
                { "Damage", damage },
                { "Death", dead},
            });
            
            // Any transition
            StateMachine.AddAnyTransition("Death", new IsDeathPredicate(Model), true);
            StateMachine.AddAnyTransition("Damage", new TakenDamagePredicate(Model), true);

            foreach (var state in states)
            {
                state.Init(this);
            }
            
            StateMachine.SetState("Idle");
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            _data = Model.GetData<EnemySO>();
        }

        protected override void Start()
        {
            SetNewTarget(player);
            
            base.Start();
            
            pathfinder.InitPathfinder(transform);
            if (EnemyManager.Instance)
            {
                EnemyManager.Instance.AddEnemy(this);
            }

            Model.OnDie += OnDieHandler;
            Model.OnTakeDamage += OnTakeDamageHandler;
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
                return Vector3.zero;
            }
            return _currentSteering.GetDir(Target.Transform).normalized;
        }

        public override float MoveAmount()
        {
            return MoveDirection().magnitude;
        }

        public override void SetSteering(ISteering steering)
        {
            base.SetSteering(steering);
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

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (pathfinder != null) pathfinder.OnDrawGizmos();
            if (StateMachine != null) StateMachine.Draw();
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            if (pathfinder != null) pathfinder.OnDrawGizmosSelected();
            if (_currentSteering != null) _currentSteering.Draw();
        }

        private void OnDieHandler()
        {
            
            StateMachine.SetState("Death");
            if (EnemyManager.Instance) EnemyManager.Instance.RemoveEnemy(this);
        }
        
        private void OnTakeDamageHandler()
        {
            StateMachine.SetState("Damage");
        }

        public override void Dispose()
        {
            Model.OnDie -= OnDieHandler;
            Model.OnTakeDamage -= OnTakeDamageHandler;
            
            Target = null;
            if (_currentSteering != null) _currentSteering.Dispose();
            _currentSteering = null;
            
            if (pathfinder != null) pathfinder.Dispose();
            pathfinder = null;
            
            base.Dispose();
        }
    }
}