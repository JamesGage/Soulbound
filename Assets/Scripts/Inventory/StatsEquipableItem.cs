using System.Collections.Generic;
using RPG.Inventories;
using RPG.Stats;
using UnityEngine;


[CreateAssetMenu(menuName = ("RPG/Inventory/Equipable Item"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [SerializeField] Modifier[] addativeModifiers;
        [SerializeField] Modifier[] percentageModifiers;
            
            [System.Serializable]
            struct Modifier
            {
                public StatTypes statTypes;
                public float value;
            }

            public IEnumerable<int> GetAddativeModifiers(StatTypes statTypes)
            {
                foreach (var modifier in addativeModifiers)
                {
                    if (modifier.statTypes == statTypes)
                    {
                        yield return (int)modifier.value;
                    }
                }
            }

            public IEnumerable<float> GetPercentageModifiers(StatTypes statTypes)
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