using System;
using System.Collections.Generic;
using _project.Scripts.Managers;
using _project.Scripts.UI.InChapter;
using _project.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _project.Scripts.UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] private Transform scoreSection;
        [SerializeField] private Image scoreRingImage;
        [SerializeField] private Image fuelFillBar;
        [SerializeField] private TextMeshProUGUI startCountText;
        [SerializeField] private TextMeshProUGUI liveScoreText;
        [SerializeField] private RectTransform liveScoreGameObject;
        [SerializeField] private TextMeshProUGUI killCount;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private SkillPromptUI skillPromptUI;
        [SerializeField] private WarningUi warningUi;
        [SerializeField] private PauseMenuUi pauseMenuUi;
        [SerializeField] private GameObject pauseBtn;

        private Vector3 liveScoreTargetScale;


        private void Awake()
        {
            gameObject.SetActive(false);
        }


        private void Start()
        {
            liveScoreTargetScale = Vector3.one;    
            liveScoreGameObject.localScale = Vector3.zero;

            pauseMenuUi.Init();
        }


        private void OnEnable()
        {
            ControlPauseButton(false);
            pauseMenuUi.ResetItems();
        }


        public void ShowPauseMenu()
        {
            ControlPauseButton(true);
        }


        public void ShowWarningMessage(WarningType warningType)
        {
            warningUi.ShowWarning(warningType);
        }


        public void ControlPauseButton(bool set)
        {
            pauseBtn.SetActive(set);
        }


        public void ClosePauseMenuUI()
        {
            pauseMenuUi.gameObject.SetActive(false);
        }


        #region ScoreSection

        public void EnableScoreSection()
        {
            scoreRingImage.fillAmount = 0f;
            startCountText.text = "0X";
            liveScoreText.text = "0";
            
            scoreSection.gameObject.SetActive(true);
        }


        public void ControlScoreSection(bool set)
        {
            scoreSection.gameObject.SetActive(set);
        }


        public void EnableLiveScoreText()
        {
            liveScoreGameObject.gameObject.transform.DOScale(liveScoreTargetScale, 0.4f).SetEase(Ease.OutBack);
        }


        public void DisableLiveScoreText()
        {
            liveScoreGameObject.gameObject.transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack);
        }


        public void UpdateLiveScoreText(float value)
        {
            liveScoreText.text = value.ToString("0");
        }


        public void UpdateStarScore(int val)
        {
            startCountText.text = val.ToString("0");
        }


        public void UpdateKillCount(int val)
        {
            killCount.text = val.ToString();
        }


        public void SetScoreRingFill(float value)
        {
            scoreRingImage.fillAmount = value;
        }


        public void UpdateTimeText(float seconds)
        {
            timeText.text = Utilities.GetTimeFromSecondsToFormattedText((int)seconds);
        }


        public void ResetScoreUI()
        {
            scoreRingImage.fillAmount = 0f;
            killCount.text = "0";
            startCountText.text = "0";
            timeText.text = "00:00";
            liveScoreGameObject.transform.localScale = Vector3.zero;
        }

        #endregion


        #region FuelSection

        public void UpdateFuelBar(float val)
        {
            fuelFillBar.fillAmount = val;
        }


        public void ResetFuelBar()
        {
            fuelFillBar.fillAmount = 1f;
        }

        #endregion


        #region SkillPrompt

        public void ShowSkillPromptUI(List<SkillPromptInfo> skillPromptInfos, UnityAction action)
        {
            skillPromptUI.ShowPrompt(skillPromptInfos, action);
        }


        public void HideSkillPromptUI(UnityAction action)
        {
            skillPromptUI.HidePrompt(action);
        }


        public void CloseSkillPromptUi()
        {
            skillPromptUI.ResetPrompt();
        }

        public SkillPromptUI GetSkillPromptUi() => skillPromptUI;

        #endregion
    }
}