using System;
using RPG.Questing;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class QuestInfoUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _title;
        [SerializeField] TextMeshProUGUI _objectiveTitleText;
        [SerializeField] Transform _objectiveContainer;
        [SerializeField] GameObject _objectivePrefab;
        [SerializeField] GameObject _objectiveIncompletePrefab;
        [SerializeField] TextMeshProUGUI _rewardTitleText;
        [SerializeField] TextMeshProUGUI _rewardText;
        
        private QuestStatus _status;

        private void OnEnable()
        {
            InfoActiveState(false);
        }

        public void SetInfo(QuestStatus status)
        {
            InfoActiveState(true);
            _status = status;
            Quest quest = status.GetQuest();
            _title.text = quest.GetTitle();
            foreach (Transform item in _objectiveContainer)
            {
                Destroy(item.gameObject);
            }
            
            foreach (var objective in quest.GetObjectives())
            {
                var prefab = _objectiveIncompletePrefab;
                if (status.IsObjectiveComplete(objective.reference))
                {
                    prefab = _objectivePrefab;
                }
                var objectiveInstance = Instantiate(prefab, _objectiveContainer);
                var objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective.description;
            }

            _rewardText.text = GetRewardText(quest);
        }
        
        private string GetRewardText(Quest quest)
        {
            string rewardText = "";
            foreach (var reward in quest.GetRewards())
            {
                if (rewardText != "")
                {
                    rewardText += ", ";
                }

                if (reward.number > 1)
                {
                    rewardText += reward.number + " ";
                }

                rewardText += reward.item.GetDisplayName();
            }

            if (rewardText == "")
            {
                rewardText = "No Reward";
            }

            rewardText += ".";
            return rewardText;
        }
        
        private void InfoActiveState(bool state)
        {
            _title.enabled = state;
            _objectiveTitleText.enabled = state;
            _objectiveContainer.gameObject.SetActive(state);
            _rewardTitleText.enabled = state;
            _rewardText.enabled = state;
        }
    }
}