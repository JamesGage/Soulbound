using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<int> GetAddativeModifiers(StatTypes statTypes);
        IEnumerable<float> GetPercentageModifiers(StatTypes statTypes);
    }
}