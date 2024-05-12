using UnityEngine;
using Game.Entities.Weapons;
using Game.Interfaces;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "SO/Items/Weapons", order = 1)]

    public class WeaponSO : ScriptableObject
    {
        public float Damage => damage;
        public float Range => range;
        public int WeaponIndex => weaponIndex;
        public AttackSO LightAttack => lightAttack;
        public AttackSO HeavyAttack => heavyAttack;
        
        [Header("Settings")] 
        [SerializeField] private float damage;
        [SerializeField] private float range;

        [Header("Animation")] 
        [Range(0, 3)] [SerializeField] private int weaponIndex;
        
        [Header("Attacks")]
        [SerializeField] private AttackSO lightAttack;
        [SerializeField] private AttackSO heavyAttack;
    }
    
    
}


