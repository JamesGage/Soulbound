using UnityEngine;

namespace RPG.UI
{
    public class DestroyObject : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void DestroyTimed(float time)
        {
            Destroy(gameObject, time);
        }
    }
}