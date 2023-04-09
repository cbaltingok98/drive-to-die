using System;
using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using UnityEngine;

namespace _project.Scripts.UI
{
    public class BottomMenuUI : MonoBehaviour
    {
        [SerializeField] private List<BottomMenuSingleButton> singleButtons;

        private BottomMenuType currentOpenMenu;

        private CanvasManager canvasManager;

        private bool initialized;
        
        private void Init()
        {
            if (initialized) return;
            initialized = true;
            
            canvasManager = CanvasManager.Instance;
            foreach (var menuSingleButton in singleButtons)
            {
                menuSingleButton.AssignMenu(this);
            }
        }


        private void OnEnable()
        {
            ResetBottomMenuAndEnable(BottomMenuType.Home);
        }


        private void ResetBottomMenuAndEnable(BottomMenuType menuType)
        {
            Init();
            
            canvasManager.GetMenuUi().SwitchMenu(currentOpenMenu, menuType);
            currentOpenMenu = menuType;
            
            var found = false;
            for (var i = 0; i < singleButtons.Count; i++)
            {
                if (singleButtons[i].GetButtonType() == menuType)
                {
                    var dir = 1;
                    if (i == 0)
                    {
                        dir = 1;
                    }
                    else if (i == singleButtons.Count - 1)
                    {
                        dir = -1;
                    }
                    else
                    {
                        dir = 0;
                    }
                    singleButtons[i].Select(dir);
                    found = true;
                }
                else
                {
                    var multiply = 1f;
                    if (i == 0 || i == singleButtons.Count - 1)
                    {
                        multiply = 0.5f;
                    }
                    singleButtons[i].DeSelect(found ? 1 : -1, multiply);
                }
            }
        }


        public void MenuButtonClicked(BottomMenuType menuType)
        {
            ResetBottomMenuAndEnable(menuType);
            SoundManager.Instance.Play(SoundType.UiClick);
        }
    }
}