using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings_Menu
{
    public class AudioSliderUI : MonoBehaviour
    {
        [SerializeField] string _busPath;
        [SerializeField] Slider _slider;

        private Bus _bus;

        private void Awake()
        {
            _bus = FMODUnity.RuntimeManager.GetBus(_busPath);
        }

        public void SetBusVolume(float volume)
        {
            _bus.setVolume(DecibelToLinear(volume));
        }
        
        private float DecibelToLinear(float dB)
        {
            return Mathf.Pow(10.0f, dB / 20f);
        }
    }
}