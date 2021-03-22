using UnityEngine;

namespace RPG.Stats
{
    [System.Serializable]
    public abstract class Stat : MonoBehaviour
    { 
        public virtual float GetStat()
        {
            return 0;
        }

        public virtual void SetStat(int value)
        {
            
        }

        public virtual Stats GetStatType()
        {
            return Stats.Vitality;
        }
    }
}