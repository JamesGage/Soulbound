using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

namespace UI.Settings_Menu
{
    public class SettingsMenuUI : MonoBehaviour
    {
        [SerializeField] Slider masterSlider;
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider sfxSlider;
        [SerializeField] TMP_Dropdown _resolutionDropdown;

        private Bus masterBus;
        private Bus musicBus;
        private Bus sfxBus;
        private Resolution[] _resolutions;

        private void Start()
        {
            FindResolutions();
            FindBus();
        }

        public void SetResolution(int resolutionIndex)
        {
            var resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
        
        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetFullScreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        public void SetMasterVolume(float volume)
        {
            masterBus.setVolume(DecibelToLinear(volume));
        }
        
        public void SetMusicVolume(float volume)
        {
            musicBus.setVolume(DecibelToLinear(volume));
        }
        
        public void SetSFXVolume(float volume)
        {
            sfxBus.setVolume(DecibelToLinear(volume));
        }
        
        private void FindResolutions()
        {
            _resolutions = Screen.resolutions;
            _resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            var currentResolutionIndex = 0;

            for (int i = 0; i < _resolutions.Length; i++)
            {
                var option = _resolutions[i].width + " x " + _resolutions[i].height;
                options.Add(option);

                if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }

            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.value = currentResolutionIndex;
            _resolutionDropdown.RefreshShownValue();
        }

        private void FindBus()
        {
            masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
            musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
            sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        }

        private float DecibelToLinear(float dB)
        {
           return Mathf.Pow(10.0f, dB / 20f);
        }
    }
}