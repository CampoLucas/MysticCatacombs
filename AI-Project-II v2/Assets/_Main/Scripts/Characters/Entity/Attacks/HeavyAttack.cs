using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;

namespace Game.Entities
{
    public class HeavyAttack : LightAttack
    {
        protected override float GetStartTime() => Entity.CurrentWeapon().Stats.HeavyAttackTriggerStarts;
        protected override float GetEndTime() => Entity.CurrentWeapon().Stats.HeavyAttackTriggerEnds;
    }
}
