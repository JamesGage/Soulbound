using RPG.Stats;
using UnityEngine;

namespace RPG.Control
{
    public class StopMovement : MonoBehaviour
    {
        [SerializeField] private bool lookAtCamera;
        [SerializeField] private GameObject _cameraPosition;
        private GameObject _player;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
        }

        private void OnEnable()
        {
            if (_player.GetComponent<Health>().IsDead()) return;
            
            _player.GetComponent<PlayerController>().SetStopMove(true);
            if (lookAtCamera)
            {
                _player.transform.LookAt(_cameraPosition.transform);
            }
        }

        private void OnDisable()
        {
            if (_player.GetComponent<Health>().IsDead()) return;
            
            _player.GetComponent<PlayerController>().SetStopMove(false);
        }
    }
}