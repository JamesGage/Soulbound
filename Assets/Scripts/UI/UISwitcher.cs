using UnityEngine;

namespace RPG.UI
{
    public class UISwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject entryPoint;

        private void OnEnable()
        {
            SwitchTo(entryPoint);
        }

        public void SwitchTo(GameObject toDisplay)
        {
            if(toDisplay.transform.parent != transform) return;
            
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(child.gameObject == toDisplay);
            }
        }
    }
}