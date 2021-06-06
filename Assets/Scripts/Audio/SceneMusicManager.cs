using UnityEngine;

namespace RPG.Audio
{
    public class SceneMusicManager : MonoBehaviour
    {
        [SerializeField] private string eventPath = "event:/Music/Scenes/Level Music";
        
        private static FMOD.Studio.EventInstance _music;

        private void Awake()
        {
            _music = FMODUnity.RuntimeManager.CreateInstance(eventPath);
            _music.start();
            _music.release();
        }

        public static void Scene(float currentScene)
        {
            _music.setParameterByName("Scene", currentScene);
        }

        public static void SetThreat(float threatLevel)
        {
            _music.setParameterByName("Threat Level", threatLevel);
        }

        private void OnDestroy()
        {
            _music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}