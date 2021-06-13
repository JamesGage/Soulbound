using RPG.UI.Inventories;
using UnityEngine;
using UI.Tooltips;

namespace RPG.UI.Ability_Menu
{
    [RequireComponent(typeof(IItemHolder))]
    public class AbilityTooltipSpawner : TooltipSpawner
    {
        public override bool CanCreateTooltip()
        {
            var ability = GetComponent<IAbilityHolder>().GetAbility();
            if (!ability) return false;

            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            var abilityTooltip = tooltip.GetComponent<AbilityTooltip>();
            if (!abilityTooltip) return;

            var ability = GetComponent<IAbilityHolder>().GetAbility();

            abilityTooltip.Setup(ability);
        }
    }
}