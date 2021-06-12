using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Effect Spawn Target Prefab", menuName = "Abilities/Effects/Spawn Target Prefab")]
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
            var instances = new List<Transform>();
            
            foreach (var target in data.GetTargets())
            {
                instances.Add(Instantiate(targetPrefab, target.transform.position, Quaternion.identity));
            }
            
            if (destroyDelay > 0)
            {
                foreach (var instance in instances)
                {
                    yield return new WaitForSeconds(destroyDelay);
                    Destroy(instance.gameObject);
                }
            }

            finished();
        }
    }
}