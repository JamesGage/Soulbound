using System.Collections;
using RPG.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Coroutine _currentActiveFade = null;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1;
        }
        
        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }

        public Coroutine FadeIn(float time)
        {
            SceneMusicManager.Scene(SceneManager.GetActiveScene().buildIndex);
            return Fade(0, time);
        }

        public Coroutine Fade(float target, float time)
        {
            if (_currentActiveFade != null)
            {
                StopCoroutine(_currentActiveFade);
            }
            _currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            return _currentActiveFade;
        }
        
        public IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}