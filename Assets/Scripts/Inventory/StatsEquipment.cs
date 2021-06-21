using System.Collections.Generic;
using RPG.Stats;

namespace RPG.Inventories
{
    public class StatsEquipment : Equipment, IModifierProvider
    {
        public IEnumerable<float> GetAddativeModifiers(Stat stat)
        {
            if(GetEquippedWeapon() == null) yield break;
            
            foreach (var modifier in GetEquippedWeapon().GetAddativeModifiers(stat))
            {
                yield return modifier;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(GetEquippedWeapon() == null) yield break;
            
            foreach (var modifier in GetEquippedWeapon().GetPercentageModifiers(stat))
            {
                yield return modifier;
            }
        }
    }
}