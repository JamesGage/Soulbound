using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings_Menu
{
    public class AudioSliderUI : MonoBehaviour
    {
        [SerializeField] string _busPath;
        
        private Slider _slider;

        private Bus _bus;

        private void Awake()
        {
            _bus = FMODUnity.RuntimeManager.GetBus(_busPath);
            _slider = GetComponent<Slider>();
            _slider.minValue = 0.0001f;
            _slider.maxValue = 1f;
        }

        private void Start()
        {
            _slider.value = PlayerPrefs.GetFloat(_busPath, 0.75f);
        }

        public void SetBusVolume(float volume)
        {
            PlayerPrefs.SetFloat(_busPath, volume);

            if (volume < 0.05f)
            {
                _bus.setVolume(0);
                return;
            }

            _bus.setVolume(Mathf.Log10(volume * 20));
        }
    }
}