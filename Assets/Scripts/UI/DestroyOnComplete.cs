using UnityEngine;

namespace UI
{
    public class DestroyOnComplete : MonoBehaviour
    {
        [SerializeField] float _timeToDestroy;

        private void OnEnable()
        {
            Destroy(gameObject, _timeToDestroy);
        }
    }
}