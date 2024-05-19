using System;
using Game.Entities;
using Game.Items.Weapons;
using Game.SO;
using Game.WeaponSystem;
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
        void Rotate(Vector3 dir, float speed);
        Vector3 GetVelocity();
        Vector3 GetForward();

        #endregion

        #region Health

        bool IsInvulnerable();
        bool HasTakenDamage();
        bool IsAlive();

        #endregion

        #region Attacks

        BaseWeapon CurrentWeapon();

        #endregion
        
        #region Timer Methods

        bool GetTimerComplete();
        float GetRandomTime(float maxTime);
        void RunTimer();
        void SetTimer(float time);

        #endregion
        
        #region Strategy

        void SetMovement(IMovement movement, bool dispose = false);
        void SetRotation(IRotation rotation);

        #endregion
    }
}