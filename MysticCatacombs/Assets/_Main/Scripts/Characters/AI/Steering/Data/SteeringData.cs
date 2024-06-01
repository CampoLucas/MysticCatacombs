using System.Collections.Generic;
using Game.Enemies;
using Game.Entities.Flocking;
using Game.Entities.Steering;
using Game.Interfaces;
using Game.Managers;
using Game.Pathfinding;
using UnityEngine;

namespace Game.Data
{
    [System.Serializable]
    public class SteeringData
    {
        public float Strength => strength;
        
        [Header("General")]
        [SerializeField] protected float strength;

    }

    [System.Serializable]
    public class SeekData : SteeringData
    {
        public ISteering Get(Transform self)
        {
            return new Seek(self, this);
        }

        public ISteeringDecorator GetDecorator(Transform self)
        {
            return new SeekDecorator(self, this);
        }

        public ISteering Get(Transform self, Pathfinder pathfinder)
        {
            return new SeekPathfinder(self, Strength, pathfinder);
        }
    }

    [System.Serializable]
    public class PursuitData : SteeringData
    {
        public float PursuitTime => pursuitTime;
        
        [Header("Pursuit")]
        [SerializeField] private float pursuitTime;
        
        public ISteering Get(Transform self)
        {
            return new Pursuit(self, this);
        }

        public ISteeringDecorator GetDecorator(Transform self)
        {
            return new PursuitDecorator(self, this);
        }
    }

    [System.Serializable]
    public class ObstacleAvoidanceData : SteeringData
    {
        public float Range => range;
        public float Angle => angle;
        public int MaxObs => maxObs;
        public LayerMask Mask => mask;
        
        [Header("Obstacle Avoidance")]
        [SerializeField] private float range = 0.05f;
        [SerializeField] private float angle = 240f;
        [SerializeField] private int maxObs = 10;
        [SerializeField] private LayerMask mask;
        
        public ISteering Get(Transform self)
        {
            return new ObstacleAvoidance(self, this);
        }

        public ISteeringDecorator GetDecorator(Transform self)
        {
            return new ObstacleAvoidanceDecorator(self, this);
        }
        
    }

    [System.Serializable]
    public class FlockingHandlerData : SteeringData
    {
        [Header("Detection")] 
        [SerializeField] private float range;
        [SerializeField] private LayerMask flockingMask;
        [SerializeField] private int maxBoids;
        
        [Header("Flockings")]
        [Header("Alignment")]
        [SerializeField] private bool useAlignment;
        [SerializeField] private float alignmentMultiplier;
        
        [Header("Avoidance")]
        [SerializeField] private bool useAvoidance;
        [SerializeField] private float avoidanceMultiplier;
        [SerializeField] private float avoidanceRange;
        
        [Header("Cohesion")]
        [SerializeField] private bool useCohesion;
        [SerializeField] private float cohesionMultiplier;

        public ISteering Get(EnemyModel self)
        {
            var s = new List<IFlocking>();
            
            if (useAlignment) s.Add(new Alignment(alignmentMultiplier));
            if (useAvoidance) s.Add(new Avoidance(avoidanceMultiplier, avoidanceRange));
            if (useCohesion) s.Add(new Cohesion(cohesionMultiplier));

            return new FlockingHandler(self.gameObject, s, self, flockingMask, range, Strength, maxBoids);
        }

        public ISteeringDecorator GetDecorator(EnemyModel self)
        {
            var s = new List<IFlocking>();
            
            if (useAlignment) s.Add(new Alignment(alignmentMultiplier));
            if (useAvoidance) s.Add(new Avoidance(avoidanceMultiplier, avoidanceRange));
            if (useCohesion) s.Add(new Cohesion(cohesionMultiplier));
            
            return new FlockingDecorator(self.gameObject, s, self, flockingMask, range, Strength, maxBoids);
        }
    }
}