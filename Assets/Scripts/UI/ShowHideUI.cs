using UnityEngine;

namespace GameDevTV.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer = null;
        
        [FMODUnity.EventRef] public string UIOpenSFX = "event:/SFX/UI/Bag/Bag_Open";
        [FMODUnity.EventRef] public string UICloseSFX = "event:/SFX/UI/Bag/Bag_Closed";

        // Start is called before the first frame update
        void Start()
        {
            if (toggleKey == KeyCode.None) return;
            uiContainer.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
            
            if(!uiContainer.activeSelf)
                FMODUnity.RuntimeManager.PlayOneShot(UIOpenSFX);
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot(UICloseSFX);
            }
        }
    }
}