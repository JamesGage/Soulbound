using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [System.Serializable]
    public class StatVariables
    {
        [SerializeField] private Stat[] _stats;
        
        public Dictionary<Stats, float> statsLookup = new Dictionary<Stats, float>();

        public void Initialize()
        {
            statsLookup.Clear();

            foreach (var stat in _stats)
            {
                if (stat.GetStatType() == Stats.MoveSpeed || stat.GetStatType() == Stats.AttackSpeed)
                {
                    stat.SetStat((int)statsLookup[Stats.Speed]);
                }
                statsLookup.Add(stat.GetStatType(), stat.GetStat());
            }
        }

        public void SetStat(Stats statType, int statValue)
        {
            foreach (var stat in _stats)
            {
                if(stat.GetStatType() != statType) continue;
                
                stat.SetStat(statValue);
                break;
            }
            
            Initialize();
        }
    }
}