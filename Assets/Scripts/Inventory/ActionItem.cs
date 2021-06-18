using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Action Item"))]
    public class ActionItem : InventoryItem
    {
        [FMODUnity.EventRef] public string comsumeSFX;
        public virtual void Use(GameObject user)
        {
            Debug.Log("Using action: " + this);
            FMODUnity.RuntimeManager.PlayOneShot(comsumeSFX);
        }
    }
}