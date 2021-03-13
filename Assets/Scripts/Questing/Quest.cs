using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Questing
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest/NewQuest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] List<Objective> _objectives = new List<Objective>();
        [SerializeField] private List<Reward> _rewards = new List<Reward>();

        [System.Serializable]
        public class Reward
        {
            [Min(1)]
            public int number;
            public InventoryItem item;
        }
        
        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;
        }

        public string GetTitle()
        {
            return name;
        }

        public int GetobjectiveCount()
        {
            return _objectives.Count;
        }

        public IEnumerable<Objective> GetObjectives()
        {
            return _objectives;
        }

        public IEnumerable<Reward> GetRewards()
        {
            return _rewards;
        }

        public bool HasObjective(string objectiveRef)
        {
            foreach (var objective in _objectives)
            {
                if (objective.reference == objectiveRef)
                {
                    return true;
                }
            }

            return false;
        }

        public static Quest GetByName(string questName)
        {
            foreach (var quest in Resources.LoadAll<Quest>(""))
            {
                if (quest.name == questName)
                {
                    return quest;
                }
            }
            return null;
        }
    }
}