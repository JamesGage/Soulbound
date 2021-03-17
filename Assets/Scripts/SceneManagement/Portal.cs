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

            yield return fader.FadeOut(_fadeTime.x);
            
            saveWrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            
            saveWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            yield return new WaitForSeconds(1f);
            
            //saveWrapper.DeleteFile();

            yield return fader.FadeIn(_fadeTime.y);
            saveWrapper.Save();
            
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