using UnityEngine;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer = null;
        [SerializeField] private bool hideMenus;
        [SerializeField] private GameObject[] menusToHide;
        
        [FMODUnity.EventRef] public string UIOpenSFX = "event:/SFX/UI/Bag/Bag_Open";
        [FMODUnity.EventRef] public string UICloseSFX = "event:/SFX/UI/Bag/Bag_Closed";
        
        void Start()
        {
            if (toggleKey == KeyCode.None) return;
            uiContainer.SetActive(false);
        }
        
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

            if (hideMenus)
            {
                foreach (var menu in menusToHide)
                {
                    menu.SetActive(!menu.activeSelf);
                }   
            }

            if(!uiContainer.activeSelf)
                FMODUnity.RuntimeManager.PlayOneShot(UIOpenSFX);
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot(UICloseSFX);
            }
        }
    }
}