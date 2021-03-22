using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<int> GetAddativeModifiers(Stats stats);
        IEnumerable<float> GetPercentageModifiers(Stats stats);
    }
}