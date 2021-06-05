using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier { A, B, C, D }

        [SerializeField] string _sceneToLoad;
        [SerializeField] Transform _spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] private float _fadeOutTime = 0.2f;
        [SerializeField] float _fadeInTime = 0.2f;
        [SerializeField] float _fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper saveWrapper = FindObjectOfType<SavingWrapper>();

            yield return fader.FadeOut(_fadeOutTime);
            
            saveWrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            
            saveWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            saveWrapper.Save();
            
            yield return new WaitForSeconds(_fadeWaitTime);
            fader.FadeIn(_fadeInTime);

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) continue;
                if(portal.destination != destination) continue;
                return portal;
            }

            Debug.Log("No portal in scene");
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
            player.transform.rotation = otherPortal._spawnPoint.rotation;
        }
    }
}