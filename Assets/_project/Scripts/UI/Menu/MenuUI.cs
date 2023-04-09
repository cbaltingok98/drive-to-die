using System.Collections.Generic;
using _project.Scripts.Enums;
using _project.Scripts.UI.Interface;
using UnityEngine;

namespace _project.Scripts.UI.Menu
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private CoinContainerUI coinContainerUI;
        [SerializeField] private BottomMenuUI bottomMenuUI;
        [SerializeField] private List<MenuUiElement> uiMenuElements;

        private Coroutine _myCoroutine;
        
        public CoinContainerUI CoinContainerUI() => coinContainerUI;
        
        private void OnEnable()
        {
            bottomMenuUI.gameObject.SetActive(true);
        }


        private void OnDisable()
        {
            bottomMenuUI.gameObject.SetActive(false);
        }


        public void SwitchMenu(BottomMenuType disable, BottomMenuType enable)
        {
            MenuUiElement uiToDisable = null;
            MenuUiElement uiToEnable = null;

            for (var i = 0; i < uiMenuElements.Count; i++)
            {
                if (uiMenuElements[i].GetMenuType() == disable)
                {
                    uiToDisable = uiMenuElements[i];
                }
                
                if (uiMenuElements[i].GetMenuType() == enable)
                {
                    uiToEnable = uiMenuElements[i];
                }
            }

            if ((int)disable != 0 && uiToDisable.gameObject.activeSelf)
            {
                if (!uiToEnable.gameObject.activeSelf)
                {
                    uiToDisable.Disable();
                    uiToEnable.Enable();
                }
                uiToEnable.UpdateInfo();
            }
            else if ((int)disable == 0 && !uiToEnable.gameObject.activeSelf)
            {
                uiToEnable.Enable();
            }
        }
    }
}