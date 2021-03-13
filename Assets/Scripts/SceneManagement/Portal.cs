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
        
        [Tooltip("X = Fade Out time. Y = Fade In time.")]
        [SerializeField] Vector2 _fadeTime = new Vector2(1f, 0.5f);

        private PlayerController _oldPlayerController;
        private PlayerController _newPlayerController;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _oldPlayerController = other.GetComponent<PlayerController>();
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper saveWrapper = FindObjectOfType<SavingWrapper>();
            _oldPlayerController.enabled = false;

            yield return fader.FadeOut(_fadeTime.x);
            
            saveWrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            
            saveWrapper.Load();
            
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            _newPlayerController.enabled = false;

            yield return fader.FadeIn(_fadeTime.y);
            saveWrapper.Save();

            _newPlayerController.enabled = true;
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
            _newPlayerController = player.GetComponent<PlayerController>();
            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
            player.transform.rotation = otherPortal._spawnPoint.rotation;
        }
    }
}