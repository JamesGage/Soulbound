using System;
using System.Collections.Generic;
using RPG.Core;
using RPG.Inventories;
using RPG.Saving;
using UnityEngine;

namespace RPG.Questing
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        private List<QuestStatus> _statuses = new List<QuestStatus>();
        private Inventory _inventory;
        private ItemDropper _itemDropper;

        [FMODUnity.EventRef] public string addQuestSFX;
        [FMODUnity.EventRef] public string objectiveCompleteSFX;
        [FMODUnity.EventRef] public string questCompleteSFX;

        public event Action onUpdate;

        private void Awake()
        {
            _inventory = GetComponent<Inventory>();
            _itemDropper = GetComponent<ItemDropper>();
        }

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            var newStatus = new QuestStatus(quest);
            _statuses.Add(newStatus);
            
            FMODUnity.RuntimeManager.PlayOneShot(addQuestSFX);
            
            if(onUpdate != null)
                onUpdate();
        }

        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return _statuses;
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            var status = GetQuestStatus(quest);
            status.CompleteObjective(objective);
            FMODUnity.RuntimeManager.PlayOneShot(objectiveCompleteSFX);

            if (status.IsComplete())
            {
                GiveReward(quest);
                FMODUnity.RuntimeManager.PlayOneShot(questCompleteSFX);
            }
            
            if(onUpdate != null)
                onUpdate();
        }

        public int GetQuestCount()
        {
            return _statuses.Count;
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (var status in _statuses)
            {
                if (status.GetQuest() == quest)
                {
                    return status;
                }
            }

            return null;
        }
        
        private void GiveReward(Quest quest)
        {
            foreach (var reward in quest.GetRewards())
            {
                var success = _inventory.AddToFirstEmptySlot(reward.item, reward.number);
                if (!success)
                {
                    _itemDropper.DropItem(reward.item, reward.number);
                }
            }
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (var status in _statuses)
            {
                state.Add(status.CaptureState());
            }

            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if (stateList == null) return;

            _statuses.Clear();
            foreach (var objectState in stateList)
            {
                _statuses.Add(new QuestStatus(objectState));
            }
        }

        public bool? Evaluate(PredicateType predicate, string[] parameters)
        {
            switch (predicate)
            {
                case PredicateType.HasQuest : return HasQuest(Quest.GetByName(parameters[0]));
                case PredicateType.QuestCompleted : return GetQuestStatus(Quest.GetByName(parameters[0])).IsComplete();
                case PredicateType.CompletedObjective : return GetQuestStatus(Quest.GetByName(parameters[0])).IsObjectiveComplete(parameters[1]);
            }

            return null;
        }
    }
}