using System;
using _project.Scripts.LevelInfo;
using _project.Scripts.Managers;
using _project.Scripts.Models;
using _project.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI
{
    public class LevelFailedUI : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Image background;
        [SerializeField] private Image frontBackground;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI killText;
        [SerializeField] private TextMeshProUGUI skillScoreText;
        [SerializeField] private TextMeshProUGUI coinText;

        private bool ignoreLick;

        private int coinToEarn;

        public void Initialize(LevelFinishData levelFinishData)
        {
            timeText.text = Utilities.GetTimeFromSecondsToFormattedText(levelFinishData.playTime);
            skillScoreText.text = levelFinishData.skillScore.ToString();
            killText.text = Utilities.FormatPriceToString(levelFinishData.killCount);
            coinToEarn = levelFinishData.coinToEarn;

            coinText.text = "";
        }


        private void OnEnable()
        {
            StartAnimations();
        }


        private void StartAnimations()
        {
            var continueBtnScale = continueButton.transform.localScale;
            continueButton.transform.localScale = Vector3.zero;

            background.DOFade(0f, 0f);
            var frontBackgroundScale = frontBackground.transform.localScale;
            frontBackground.transform.localScale = Vector3.zero;

            background.DOFade(0.6f, 0.4f);
            frontBackground.transform.DOScale(frontBackgroundScale, 0.4f).SetDelay(0.2f).SetEase(Ease.OutBack).OnComplete(
                () =>
                {
                    AnimateEarnedCoin();
                    continueButton.transform.DOScale(continueBtnScale, 0.3f).SetDelay(1.5f).OnComplete(() =>
                    {
                        ignoreLick = false;
                    });
                });
        }


        private void AnimateEarnedCoin()
        {
            int coin = 0;
            DOTween.To(() => coin, x => coin = x, coinToEarn, 1.5f).OnUpdate(() =>
            {
                coinText.text = Utilities.FormatPriceToString(coin);
            });
        }


        public void ContinueButtonTapped()
        {
            if (ignoreLick)
            {
                return;
            }
            
            CanvasManager.Instance.HideLevelFailUi();
            CanvasManager.Instance.UiClick();
        }
    }
}