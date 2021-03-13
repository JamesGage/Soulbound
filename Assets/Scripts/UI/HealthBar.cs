using System.Collections;
using RPG.Attributes;
using UnityEngine;

namespace RPG.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health _healthComponent = null;
        [SerializeField] RectTransform _healthBarFill;
        [SerializeField] Canvas _rootCanvas;

        private float _oldHealth = 1f;

        private void Start()
        {
            _rootCanvas.enabled = false;
        }

        private void OnEnable()
        {
            _healthComponent.onHealthChanged += UpdateHealthBar;
        }
        
        private void OnDisable()
        {
            _healthComponent.onHealthChanged += UpdateHealthBar;
        }

        private void UpdateHealthBar()
        {
            if (Mathf.Approximately(_healthComponent.GetFraction(), 0)
                || Mathf.Approximately(_healthComponent.GetFraction(), 1))
            {
                _rootCanvas.enabled = false;
                return;
            }
            
            _rootCanvas.enabled = true;
            StartCoroutine(LerpHealth());
        }
        

        private IEnumerator LerpHealth()
        {
            var elapsedTime = 0f;
            while (elapsedTime < 0.5f)
            {
                _oldHealth = Mathf.Lerp(_oldHealth, _healthComponent.GetFraction(), Time.deltaTime * 5f);
                _healthBarFill.localScale = new Vector3(_oldHealth, 1f, 1f);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}