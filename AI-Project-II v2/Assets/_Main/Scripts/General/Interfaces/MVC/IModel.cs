using System;
using Game.Entities;
using Game.Items.Weapons;
using Game.SO;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IModel : IDisposable
    {
        IDamageable Damageable { get; }
        Transform Transform { get; }

        T GetData<T>() where T : StatSO;
        StatSO GetData();

        #region Movement

        void Move(Vector3 dir);
        void Move(Vector3 dir, float speed);
        void Rotate(Vector3 dir);
        Vector3 GetVelocity();
        Vector3 GetForward();

        #endregion

        #region Health

        bool IsInvulnerable();
        bool HasTakenDamage();
        bool IsAlive();

        #endregion

        #region Attacks

        Weapon CurrentWeapon();
        public void LightAttack();
        public void CancelLightAttack();
        public void HeavyAttack();
        public void CancelHeavyAttack();

        #endregion

        #region Levels

        void IncreaseLevel();
        int GetCurrentLevel();

        bool HasReachedMaxLevel();
        Levelable GetLevelable();

        #endregion
        
        #region Timer Methods

        bool GetTimerComplete();
        float GetRandomTime(float maxTime);
        void RunTimer();
        void SetTimer(float time);

        #endregion
        
        #region Strategy

        IMovement GetWalkingMovement();
        IMovement GetRunningMovement();
        void SetMovement(IMovement movement);
        void SetRotation(IRotation rotation);
        void SetLightAttack(IAttack attack);
        void SetHeavyAttack(IAttack attack);

        #endregion
    }
}