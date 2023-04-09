using System;
using _project.Scripts.Berk;
using _project.Scripts.Data;
using _project.Scripts.Enums;
using _project.Scripts.LevelInfo;
using _project.Scripts.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu.ChapterMenu
{
    public class ChapterMenuUi : MonoBehaviour
    {
        [SerializeField] private Image chapterImage;
        [SerializeField] private Image lockedLayer;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI chapterInfo;
        [SerializeField] private Transform comingSoonText;
        [SerializeField] private Button arrowLeft;
        [SerializeField] private Button arrowRight;
        [SerializeField] private Button startButton;
        [SerializeField] private Transform background;
        [SerializeField] private CanvasGroup middleCanvas;
        [SerializeField] private Transform warning;

        private bool btnTapped;

        private ChapterType currentChapterType;

        private void Start()
        {
            startButton.onClick.AddListener(StartLevel);
            arrowRight.onClick.AddListener(NextChapter);
            arrowLeft.onClick.AddListener(PreviousChapter);
        }


        public void EnableUi()
        {
            btnTapped = false;
            gameObject.SetActive(true);
            ParseInfo(ChapterType.Chapter1);
            Animate();
        }
        
        public void StartLevel()
        {
            if (btnTapped) return;
            btnTapped = true;
            
            GameManager.Instance.StartLevel();
            CanvasManager.Instance.UiClick();
            SoundManager.Instance.Play(SoundType.UiClick);

            startButton.transform.DOScale(Vector3.one * 0.9f, 0.1f).OnComplete(() =>
            {
                startButton.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
            });
        }


        private void ParseInfo(ChapterType chapter)
        {
            currentChapterType = chapter;
            
            var levelObject = LevelObjectManager.GetLevelObjectOfType(currentChapterType);

            var sprite = levelObject.sprite;
            
            chapterImage.sprite = sprite;
            lockedLayer.sprite = sprite;

            title.text = levelObject.chapterName;
            chapterInfo.text = "Reach : <color=#8BFFD9>" + levelObject.chapterTarget + " Star Score</color>";
            
            title.gameObject.SetActive(!levelObject.comingSoon);

            comingSoonText.gameObject.SetActive(levelObject.comingSoon);

            var chapterUnlocked = DataManager.Instance.IsChapterUnlocked(currentChapterType);
            
            lockedLayer.gameObject.SetActive(!chapterUnlocked || levelObject.comingSoon);
            startButton.gameObject.SetActive(chapterUnlocked);
            
            if (levelObject.comingSoon)
            {
                warning.gameObject.SetActive(false);
                chapterInfo.gameObject.SetActive(false);
            }
            else
            {
                warning.gameObject.SetActive(!chapterUnlocked);
                chapterInfo.gameObject.SetActive(chapterUnlocked);
            }

            if (chapterUnlocked)
            {
                DataManager.Instance.SetChapter(currentChapterType);
            }
        }


        private void NextChapter()
        {
            CanvasManager.Instance.UiClick();
            SoundManager.Instance.Play(SoundType.UiClick);
            
            var chapterCount = Enum.GetValues(typeof(ChapterType)).Length;
            var curIndex = (int)currentChapterType;

            if (curIndex + 1 >= chapterCount) return;
            
            currentChapterType = (ChapterType)curIndex + 1;
            
            ParseInfo(currentChapterType);
        }


        private void PreviousChapter()
        {
            CanvasManager.Instance.UiClick();
            SoundManager.Instance.Play(SoundType.UiClick);
            
            var curIndex = (int)currentChapterType;

            if (curIndex - 1 < 0) return;

            currentChapterType = (ChapterType)curIndex - 1;
            
            ParseInfo(currentChapterType);
        }


        public void DisableUi()
        {
            gameObject.SetActive(false);
        }


        private void Animate()
        {
            background.localScale = new Vector3(1f, 0f, 1f);
            middleCanvas.alpha = 0;
            
            startButton.gameObject.SetActive(false);

            background.DOScale(Vector3.one, 0.4f).OnComplete(() =>
            {
                middleCanvas.DOFade(1f, 0.4f).OnComplete(() =>
                {
                    startButton.gameObject.SetActive(true);
                });
            });
        }
    }
}