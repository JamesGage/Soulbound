using System;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Select Directional", menuName = "Abilities/Targeting/Select Targeting")]
    public class SelectTargeting : TargetingStrategy
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] float areaAffectRadius;
        [SerializeField] float range;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            RaycastHit raycastHit;
            var ray = PlayerController.GetMouseRay();
            if (Physics.Raycast(ray, out raycastHit, 1000, layerMask))
            {
                data.SetTargetedPoint(raycastHit.point);
                data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
            }
            finished();
        }
        
        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            var hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.forward, range, layerMask);
            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }
    }
}