using System;
using Game.Interfaces;
using UnityEngine;

namespace Game.SO
{
    public class Attack : IDisposable
    {
        public int Name { get; private set; }
        public AnimationModule AnimModule { get; private set; }
        public MovementModule MoveModule { get; private set; }
        public EventModule EvModule { get; private set; }
        public TransitionModule TrModule { get; private set; }

        public Attack(string name, AnimationModule anim, MovementModule move, EventModule evs, TransitionModuleData transition)
        {
            Name = name.GetHashCode();
            AnimModule = anim;
            AnimModule.SetAnimId();
            MoveModule = move;
            EvModule = evs;
            TrModule = transition.GetTransitionModule();
        }
        
        public void Dispose()
        {
            if (AnimModule != null)
            { 
                AnimModule.Dispose();
                AnimModule = null;
            }
            
            if (MoveModule != null)
            { 
                MoveModule.Dispose();
                MoveModule = null;
            }
            
            if (EvModule != null)
            { 
                EvModule.Dispose();
                EvModule = null;
            }
            
            if (TrModule != null)
            { 
                TrModule.Dispose();
                TrModule = null;
            }
        }
    }

    [System.Serializable]
    public class AnimationModule : IDisposable
    {
        /// <summary>
        /// Animation identifier.
        /// </summary>
        public int AnimationId => _animId;
        /// <summary>
        /// Animation name.
        /// </summary>
        public string Animation => animation;
        /// <summary>
        /// Animation duration.
        /// </summary>
        public float Duration => duration;
        
        [SerializeField] private string animation;
        [SerializeField] private float duration;
        private int _animId;

        public AnimationModule(string animation, float duration) : this(animation, duration,
            Animator.StringToHash(animation)) { }
        
        public AnimationModule(AnimationModule other) : this(other.Animation, other.Duration, other.AnimationId) { }

        private AnimationModule(string anim, float length, int animId)
        {
            animation = anim;
            duration = length;
            _animId = animId;
        }

        /// <summary>
        /// Catches the animation id, used if the class is not created using the constructor.
        /// </summary>
        public void SetAnimId()
        {
            if (_animId != 0) return;
            _animId = Animator.StringToHash(animation);
        }

        /// <summary>
        /// Plays the animation.
        /// </summary>
        /// <param name="view"></param>
        public void DoAnimation(IView view)
        {
            view.UpdateMovementValues(0);
            view.CrossFade(AnimationId);
        }

        /// <summary>
        /// Removes references.
        /// </summary>
        public void Dispose()
        {
            
        }
    }

    [System.Serializable]
    public class MovementModule : IDisposable
    {
        /// <summary>
        /// Enables or disables the movement.
        /// </summary>
        public bool CanMove => canMove;
        /// <summary>
        /// Movement speed.
        /// </summary>
        public float Speed => speed;
        /// <summary>
        /// Curve modifier to the movement speed.
        /// </summary>
        public AnimationCurve SpeedCurve => speedCurve;

        [SerializeField] private bool canMove;
        [SerializeField] private float speed;
        [SerializeField] private AnimationCurve speedCurve;

        public MovementModule(bool canMove, float speed, AnimationCurve speedCurve)
        {
            this.canMove = canMove;
            this.speed = speed;
            this.speedCurve = speedCurve;
        }

        /// <summary>
        /// Moves if it is enabled.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="t"></param>
        public void DoMove(IModel model, float t)
        {
            if (!canMove) return;

            var s = Speed * (speedCurve?.Evaluate(t) ?? 1);
            model.Move(model.GetForward(), s);
        }

        /// <summary>
        /// Removes all references.
        /// </summary>
        public void Dispose()
        {
            speedCurve = null;
        }
    }

    [System.Serializable]
    public class EventModule : IDisposable
    {
        /// <summary>
        /// Offset to return true or false.
        /// </summary>
        public float Offset => curveOffset;
        /// <summary>
        /// Curve used to know if a event is triggered depending on the numeric value of the curve is grater than the offset.
        /// </summary>
        public AnimationCurve Curve => curveEvent; 
        
        [SerializeField] private float curveOffset;
        [SerializeField] private AnimationCurve curveEvent;

        public EventModule(float offset, AnimationCurve curve)
        {
            curveOffset = offset;
            curveEvent = curve;
        }

        public EventModule(EventModule other) : this(other.Offset, other.Curve)
        {
            
        }
        
        /// <summary>
        /// Returns true if a event is triggered.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Triggered(float t)
        {
            return curveEvent != null && curveEvent.Evaluate(t) > curveOffset;
        }

        /// <summary>
        /// Removes all references.
        /// </summary>
        public void Dispose()
        {
            curveEvent = null;
        }
    }

    public class TransitionModule : IDisposable
    {
        public bool CanTransition { get; private set; }
        public float TimeToTransition { get; private set; }
        public Attack AttackTransition { get; private set; }

        public TransitionModule(bool canTransition, float timeToTransition, AttackSO attackTransition)
        {
            CanTransition = canTransition;
            TimeToTransition = timeToTransition;
            if (attackTransition == null) return;
            var attack = attackTransition.GetAttack();
            if (attack != null) AttackTransition = attack;
        }

        public bool TryGetTransition(out Attack transition)
        {
            transition = AttackTransition;
            return CanTransition && AttackTransition != null;
        }
        
        public void Dispose()
        {
            if (AttackTransition != null)
            {
                AttackTransition.Dispose();
                AttackTransition = null;
            }
            
        }
    }
    
    [System.Serializable]
    public class TransitionModuleData
    {
        public bool CanTransition => canTransition;
        public float TimeToTransition => timeToTransition;
        public AttackSO AttackTransition => attackTransition;
        
        [SerializeField] private bool canTransition;
        [SerializeField] private float timeToTransition;
        [SerializeField] private AttackSO attackTransition;

        public TransitionModule GetTransitionModule() =>
            new (canTransition, timeToTransition, attackTransition);
    }
}