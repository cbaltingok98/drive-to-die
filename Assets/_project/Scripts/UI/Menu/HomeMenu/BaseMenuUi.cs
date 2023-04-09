using System;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.ShowRoom;
using _project.Scripts.UI.Interface;
using _project.Scripts.UI.Menu.ChapterMenu;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu
{
    public class BaseMenuUi : MenuUiElement
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Transform starLevel;
        [SerializeField] private TextMeshProUGUI starLevelText;
        [SerializeField] private ChapterMenuUi chapterMenuUi;
        [SerializeField] private Transform tutorialHand;

        public override BottomMenuType GetMenuType() => menuType;

        private bool btnTapped;


        private void Start()
        {
            startButton.onClick.AddListener(EnableChapterUi);
        }


        private void EnableChapterUi()
        {
            if (btnTapped) return;
            btnTapped = true;
            
            startButton.transform.DOScale(Vector3.one * 0.9f, 0.1f).OnComplete(() =>
            {
                startButton.transform.localScale = Vector3.zero;
            });
            
            chapterMenuUi.EnableUi();
            CanvasManager.Instance.UiClick();
            SoundManager.Instance.Play(SoundType.UiClick);
        }

        public override void Enable()
        {
            btnTapped = false;
            gameObject.SetActive(true);
            ShowRoomManager.Instance.BaseMenuEnabled();
            EnableAnimation();
        }


        public override void Disable()
        {
            gameObject.SetActive(false);
            chapterMenuUi.DisableUi();
        }


        public override void UpdateInfo()
        {
            btnTapped = false;
            chapterMenuUi.DisableUi();
            EnableAnimation();
        }


        private void EnableAnimation()
        {
            startButton.transform.localScale = Vector3.zero;
            startButton.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

            starLevelText.text = DataManager.Instance.PlayerData.starLevel.ToString();
            
            tutorialHand.gameObject.SetActive(!DataManager.Instance.IsPlayedBefore());
        }
    }
}