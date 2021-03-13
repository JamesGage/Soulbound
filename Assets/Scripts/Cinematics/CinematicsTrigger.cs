using System;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicsTrigger : MonoBehaviour, ISaveable
    {
        private bool _alreadyTriggered;
        private void OnTriggerEnter(Collider other)
        {
            if (!_alreadyTriggered && other.CompareTag("Player"))
            {
                GetComponent<PlayableDirector>().Play();
                _alreadyTriggered = true;
            }
        }

        public object CaptureState()
        {
            return _alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            _alreadyTriggered = (bool) state;
        }
    }
}