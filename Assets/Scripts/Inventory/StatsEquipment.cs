using System.Collections.Generic;
using RPG.Stats;

namespace RPG.Inventories
{
    public class StatsEquipment : Equipment, IModifierProvider
    {
        public IEnumerable<int> GetAddativeModifiers(Stats.Stats stats)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if(item == null) continue;

                foreach (var modifier in item.GetAddativeModifiers(stats))
                {
                    yield return modifier;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stats.Stats stats)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if(item == null) continue;

                foreach (var modifier in item.GetPercentageModifiers(stats))
                {
                    yield return modifier;
                }
            }
        }
    }
}