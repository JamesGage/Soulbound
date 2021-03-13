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
                public Stat stat;
                public float value;
            }

            public IEnumerable<int> GetAddativeModifiers(Stat stat)
            {
                foreach (var modifier in addativeModifiers)
                {
                    if (modifier.stat == stat)
                    {
                        yield return (int)modifier.value;
                    }
                }
            }

            public IEnumerable<float> GetPercentageModifiers(Stat stat)
            {
                foreach (var modifier in percentageModifiers)
                {
                    if (modifier.stat == stat)
                    {
                        yield return (int)modifier.value;
                    }
                }
            }
    }
}