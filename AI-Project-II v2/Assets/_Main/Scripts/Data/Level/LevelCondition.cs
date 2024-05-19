using System;
using Game.DesignPatterns.Predicate;
using UnityEngine;

namespace Game.Managers.Level
{
    public class LevelCondition : ScriptableObject, IDisposable, IPredicate
    {
        public string Title => title;
        
        [Header("UI")] 
        [SerializeField] private string title;
        
        public virtual bool Evaluate()
        {
            return false;
        }
        
        public virtual void Dispose()
        {
            
        }

        protected virtual void OnInit(){ }
        
        protected virtual LevelCondition OnCloned()
        {
            return Instantiate(this);
        }

        public LevelCondition Clone()
        {
            var objective = OnCloned();
            OnInit();
            return objective;
        }
    }
}