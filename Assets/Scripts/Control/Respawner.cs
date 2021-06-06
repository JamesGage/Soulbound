using System.Collections;
using RPG.SceneManagement;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField] Transform respawnLocation;
        [SerializeField] private float respawnDelay = 2f;
        [SerializeField] private float fadeTime = 0.2f;
        [SerializeField] private float healthRegenPercentage = 50f;
        
        private Health _health;
        private NavMeshAgent _navAgent;
        private void Awake()
        {
            _health = GetComponent<Health>();
            _navAgent = GetComponent<NavMeshAgent>();
            _health.OnDieEvent.AddListener(Respawn);
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            yield return new WaitForSeconds(respawnDelay);
            var fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);
            _navAgent.Warp(respawnLocation.position);
            _health.Heal(_health.GetMaxHealth() * (healthRegenPercentage/100));
            yield return fader.FadeIn(fadeTime);
        }
    }
}