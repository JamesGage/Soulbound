using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAddativeModifiers(Stat stat);
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}