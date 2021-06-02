using System;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Targeting Directional", menuName = "Abilities/Targeting/Directional Targeting")]
    public class DirectionalTargeting : TargetingStrategy
    {
        [SerializeField] private LayerMask layerMask;
        
        public override void StartTargeting(AbilityData data, Action finished)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask))
            {
                data.SetTargetedPoint(raycastHit.point);
            }
            finished();
        }
    }
}