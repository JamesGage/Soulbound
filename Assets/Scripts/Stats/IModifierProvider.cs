using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<int> GetAddativeModifiers(Stat stat);
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}