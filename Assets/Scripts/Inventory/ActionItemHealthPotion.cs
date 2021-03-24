using RPG.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Health Potion"))]
    public class ActionItemHealthPotion : ActionItem
    {
        [Range(0, 100)]
        [Tooltip("Gain back a percentage of characters MaxHealth between 0-100%")]
        [SerializeField] int _modifier = 20;
        
        public override void Use(GameObject user)
        {
            user.GetComponent<Health>().Heal(_modifier);
            FMODUnity.RuntimeManager.PlayOneShot(comsumeSFX);
        }
    }
}