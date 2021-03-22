using RPG.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Health Potion"))]
    public class ActionItemHealthPotion : ActionItem
    {
        [SerializeField] int _modifier;
        
        public override void Use(GameObject user)
        {
            user.GetComponent<Health>().Heal(_modifier);
            FMODUnity.RuntimeManager.PlayOneShot(comsumeSFX);
        }
    }
}