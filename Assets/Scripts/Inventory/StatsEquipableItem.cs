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
                public Stats.StatTypes statTypes;
                public float value;
            }

            public IEnumerable<int> GetAddativeModifiers(Stats.StatTypes statTypes)
            {
                foreach (var modifier in addativeModifiers)
                {
                    if (modifier.statTypes == statTypes)
                    {
                        yield return (int)modifier.value;
                    }
                }
            }

            public IEnumerable<float> GetPercentageModifiers(Stats.StatTypes statTypes)
            {
                foreach (var modifier in percentageModifiers)
                {
                    if (modifier.statTypes == statTypes)
                    {
                        yield return (int)modifier.value;
                    }
                }
            }
    }
}