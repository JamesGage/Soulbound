using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Equipable Item"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [SerializeField] Modifier[] addativeModifiers;
        [SerializeField] Modifier[] percentageModifiers;
            
            [System.Serializable]
            struct Modifier
            {
                public Stats.Stats stats;
                public float value;
            }

            public IEnumerable<int> GetAddativeModifiers(Stats.Stats stats)
            {
                foreach (var modifier in addativeModifiers)
                {
                    if (modifier.stats == stats)
                    {
                        yield return (int)modifier.value;
                    }
                }
            }

            public IEnumerable<float> GetPercentageModifiers(Stats.Stats stats)
            {
                foreach (var modifier in percentageModifiers)
                {
                    if (modifier.stats == stats)
                    {
                        yield return (int)modifier.value;
                    }
                }
            }
    }
}