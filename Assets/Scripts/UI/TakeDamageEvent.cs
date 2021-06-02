using RPG.Combat;
using UnityEngine.Events;

namespace RPG.UI
{
    [System.Serializable]
    public class TakeDamageEvent : UnityEvent<float, DamageType>
    {
        
    }
}