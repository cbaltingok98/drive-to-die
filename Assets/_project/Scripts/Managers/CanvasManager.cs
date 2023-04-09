using System;
using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.LevelInfo;
using _project.Scripts.UI;
using _project.Scripts.UI.Interface;
using _project.Scripts.UI.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace _project.Scripts.Managers
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance;

        [SerializeField] private MenuUI menuUI;
        [SerializeField] private InGameUI inGameUI;
        [SerializeField] private TransitionUI transitionUI;
        [SerializeField] private CountDownUI countDownUI;
        [SerializeField] private LevelFinishUI levelFinishUI;
        [SerializeField] private Joystick joystick;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        #region Menu

        public void ShowMenuUI()
        {
            menuUI.gameObject.SetActive(true);
        }


        public void HideMenuUI()
        {
            menuUI.gameObject.SetActive(false);
        }


        public MenuUI GetMenuUi() => menuUI;


        public void HideMoneyUI()
        {
            menuUI.CoinContainerUI().gameObject.SetActive(false);
        }


        public void ShowMoneyUI()
        {
            menuUI.CoinContainerUI().gameObject.SetActive(true);
        }

        #endregion


        #region InGame

        public void ShowInGameUI()
        {
            Printer.Print("Enabling in game ui");
            inGameUI.gameObject.SetActive(true);
        }


        public void HideInGameUI()
        {
            inGameUI.gameObject.SetActive(false);
        }


        public void ShowSkillPrompt(List<SkillPromptInfo> skillPromptInfos, UnityAction action)
        {
            inGameUI.ShowSkillPromptUI(skillPromptInfos, action);
        }


        public void HideSkillPrompt(UnityAction action)
        {
            inGameUI.HideSkillPromptUI(action);
        }


        public InGameUI GetInGameUI() => inGameUI;
        public CountDownUI GetCountDownUI() => countDownUI;

        #endregion


        #region LevelFail
        
        public void LevelFailed()
        {
            HideInGameUI();
            levelFinishUI.LevelFailed(GameManager.Instance.GetLevelFinishData());
        }


        public void LevelCompleted()
        {
            HideInGameUI();
            levelFinishUI.LevelCompleted(GameManager.Instance.GetLevelFinishData());
        }


        public void HideLevelFailUi()
        {
            ReturnToMainMenu(levelFinishUI.HideLevelFailUi);
        }


        public void HideLevelCompleteUi()
        {
            ReturnToMainMenu(levelFinishUI.HideLevelCompleteUi);
        }


        public void ReturnHomeFromPauseMenu()
        {
            inGameUI.ClosePauseMenuUI();
            ReturnToMainMenu(null);
        }
        #endregion


        #region Transition

        private void ReturnToMainMenu(UnityAction action)
        {
            var actionList = new List<UnityAction>();
            actionList.Add(LevelManager.Instance.DisableCarSystem);
            actionList.Add(LevelManager.Instance.ReturnToMainMenu);
            
            transitionUI.FadeOutAndFadeInToMenu(action, actionList, LevelManager.Instance.GetLevelTheme());
        }


        public void FadeOutAndFadeIn(UnityAction unityAction, UnityAction unityAction2, LevelThemes theme)
        {
            transitionUI.FadeOutAndFadeIn(unityAction, unityAction2, theme);
        }


        public void FadeIn()
        {
            transitionUI.FadeIn();
        }


        public void FadeOutAndFadeIn(LevelThemes theme)
        {
            transitionUI.FadeOutAndFadeIn(theme);
        }
        
        #endregion


        public void UiClick()
        {
            VibrationManager.UiClick();
        }
    }
}