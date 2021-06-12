using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Effect Spawn Prefab", menuName = "Abilities/Effects/Spawn Prefab")]
    public class SpawnPrefabEffect : EffectStrategy
    {
        [SerializeField] private Transform effectPrefab;
        [SerializeField] private Vector3 spawnOffset;
        [SerializeField] private float effectScale = 1;
        [SerializeField] private float destroyDelay = -1;
        
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            var instance = Instantiate(effectPrefab, data.GetUser().transform.position + spawnOffset, data.GetUser().transform.rotation);
            instance.transform.localScale *= effectScale;
            
            if (destroyDelay > 0)
            {
                yield return new WaitForSeconds(destroyDelay);
                Destroy(instance.gameObject);
            }

            finished();
        }
    }
}