using Project.Locomotion;
using UnityEngine;

namespace Project.Characters.Player
{
    [CreateAssetMenu(fileName = "New PlayerSettings", menuName = "Project/Characters/Player")]
    public class PlayerSettings : ScriptableObject
    {
        //[Header("Life")]
        
        [Header("Movement")] 
        [SerializeField] private MovementData moveSettings;
        [SerializeField] private MovementData runSettings;
        [SerializeField] private MovementData crouchMoveSettings;
        
    }
}