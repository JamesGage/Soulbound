using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Targeting Surrounding", menuName = "Abilities/Targeting/Surrounding Targeting")]
    public class SurroundingTargeting : TargetingStrategy
    {
        [SerializeField] private float _radius = 2f;
        [Range(0f, 360f)]
        [SerializeField] private float _viewAngle = 90f;
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private LayerMask _obstacleMask;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            var userTransform = data.GetUser().transform;
            data.SetTargets(GetGameObjectsInView(userTransform));

            finished();
        }

        private IEnumerable<GameObject> GetGameObjectsInView(Transform userTransform)
        {
            Collider[] targetsInViewRadius =
                Physics.OverlapSphere(userTransform.position, _radius, _targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - userTransform.position).normalized;
                if (Vector3.Angle(userTransform.forward, dirToTarget) < _viewAngle / 2)
                {
                    float distToTarget = Vector3.Distance(userTransform.position, target.position);
                    if (!Physics.Raycast(userTransform.position, dirToTarget, distToTarget, _obstacleMask))
                    {
                        yield return target.gameObject;
                    }
                }
            }
        }
    }
}