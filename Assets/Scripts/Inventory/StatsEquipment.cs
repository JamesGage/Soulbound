using System.Collections.Generic;
using RPG.Stats;

namespace RPG.Inventories
{
    public class StatsEquipment : Equipment, IModifierProvider
    {
        public IEnumerable<int> GetAddativeModifiers(Stats.StatTypes statTypes)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if(item == null) continue;

                foreach (var modifier in item.GetAddativeModifiers(statTypes))
                {
                    yield return modifier;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stats.StatTypes statTypes)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if(item == null) continue;

                foreach (var modifier in item.GetPercentageModifiers(statTypes))
                {
                    yield return modifier;
                }
            }
        }
    }
}