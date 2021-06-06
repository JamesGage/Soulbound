using System.Collections;
using Cinemachine;
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
        [SerializeField] private float enemyHealthRegenPercentage = 50f;
        
        private Health _health;
        private NavMeshAgent _navAgent;
        private void Awake()
        {
            _health = GetComponent<Health>();
            _navAgent = GetComponent<NavMeshAgent>();
            _health.OnDieEvent.AddListener(Respawn);
        }

        private void Start()
        {
            if (_health.IsDead())
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            var savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            yield return new WaitForSeconds(respawnDelay);
            var fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);
            RespawnPlayer();
            ResetEnemies();
            savingWrapper.Save();
            yield return fader.FadeIn(fadeTime);
        }

        private void ResetEnemies()
        {
            foreach (var enemyControllers in FindObjectsOfType<AIController>())
            {
                enemyControllers.Reset();
                var health = enemyControllers.GetComponent<Health>();
                if (health && !health.IsDead())
                {
                    health.Heal(health.GetMaxHealth() * (enemyHealthRegenPercentage / 100));
                }
            }
        }

        private void RespawnPlayer()
        {
            Vector3 positionDelta = respawnLocation.position - transform.position;
            _navAgent.Warp(respawnLocation.position);
            _health.Heal(_health.GetMaxHealth() * (healthRegenPercentage / 100));
            var activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if (activeVirtualCamera.Follow == transform)
            {
                activeVirtualCamera.OnTargetObjectWarped(transform, positionDelta);
            }
        }
    }
}