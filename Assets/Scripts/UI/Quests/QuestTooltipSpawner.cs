using RPG.Questing;
using UI.Tooltips;
using UnityEngine;

namespace UI.Quests
{
    public class QuestTooltipSpawner : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            QuestStatus status = GetComponent<QuestItemUI>().GetQuestStatus();
            tooltip.GetComponent<QuestTooltipUI>().Setup(status);
        }

        public override bool CanCreateTooltip()
        {
            return true;
        }
    }
}