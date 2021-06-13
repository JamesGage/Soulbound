using System;
using RPG.Combat;
using RPG.Stats;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Effect Spawn Projectile ", menuName = "Abilities/Effects/Spawn Projectile")]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] Projectile projectileToSpawn;
        [SerializeField] private float damage;
        [SerializeField] private bool useTargetPoint = true;
        [SerializeField] private bool isRightHand = true;

        public override void StartEffect(AbilityData data, Action finished)
        {
            Fighter fighter = data.GetUser().GetComponent<Fighter>();
            var spawnPosition = fighter.GetHandTransform(isRightHand).position;
            if (useTargetPoint)
            {
                SpawnProjectilesForTargetPoint(data, spawnPosition);
            }
            else
            {
                SpawnProjectilesForTargets(data, spawnPosition);
            }

            finished();
        }

        private void SpawnProjectilesForTargetPoint(AbilityData data, Vector3 spawnPosition)
        {
            var projectile = Instantiate(projectileToSpawn);
            projectile.transform.position = spawnPosition;
            projectile.SetTarget(data.GetTargetPoint(), data.GetUser(), damage);
        }

        private void SpawnProjectilesForTargets(AbilityData data, Vector3 spawnPosition)
        {
            foreach (var target in data.GetTargets())
            {
                if (target.GetComponent<Health>() != null && !target.GetComponent<Health>().IsDead())
                {
                    var projectile = Instantiate(projectileToSpawn);
                    projectile.transform.position = spawnPosition;
                    projectile.SetTarget(target.GetComponent<Health>(), data.GetUser(), damage);
                }
            }
        }
    }
}