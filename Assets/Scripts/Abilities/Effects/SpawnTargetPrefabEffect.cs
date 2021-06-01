using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Effect Spawn Prefab", menuName = "Abilities/Effects/Spawn Prefab")]
    public class SpawnTargetPrefabEffect : EffectStrategy
    {
        [SerializeField] private Transform targetPrefab;
        [SerializeField] private float destroyDelay = -1;
        
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            var instance = Instantiate(targetPrefab, data.GetTargetPoint(), Quaternion.identity);
            if (destroyDelay > 0)
            {
                yield return new WaitForSeconds(destroyDelay);
                Destroy(instance.gameObject);
            }

            finished();
        }
    }
}