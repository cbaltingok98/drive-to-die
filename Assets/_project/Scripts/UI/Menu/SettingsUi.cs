using System;
using _project.Scripts.Berk;
using _project.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu
{
    public class SettingsUi : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private CanvasGroup child;
        [SerializeField] private Image hapticBtn;
        [SerializeField] private Image soundBtn;
        [SerializeField] private Image musicBtn;
        [SerializeField] private Transform icon;
        [SerializeField] private Button hapticButton;
        [SerializeField] private Button soundButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private bool animate;


        private void Start()
        {
            button.onClick.AddListener(EnableUi);
            hapticButton.onClick.AddListener(HapticButtonClicked);
            soundButton.onClick.AddListener(SoundButtonClicked);
            musicButton.onClick.AddListener(MusicButtonClicked);
            
            UpdateInfo();
        }


        private void EnableUi()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(DisableUi);

            UpdateInfo();
            
            CanvasManager.Instance.UiClick();

            if (!animate)
            {
                child.gameObject.SetActive(true);
                child.transform.localScale = Vector3.one;
                return;
            }

            icon.transform.DORotate(new Vector3(0, 0, -170), 0.4f);

            var targetScale = Vector3.one;
            child.transform.localScale = new Vector3(targetScale.x, 0f, targetScale.z);

            child.gameObject.SetActive(true);
            child.transform.DOScale(targetScale, 0.4f);
            
            float newAlpha = 0;
            DOTween.To(() => newAlpha, x => newAlpha = x, 1, 0.3f).OnUpdate(() =>
            {
                child.alpha = newAlpha;
            });
        }


        private void UpdateInfo()
        {
            hapticBtn.gameObject.SetActive(!Constants.CanVibrate);
            soundBtn.gameObject.SetActive(!Constants.CanPlaySound);
            musicBtn.gameObject.SetActive(!Constants.CanPlayMusic);
        }


        private void HapticButtonClicked()
        {
            if (Constants.CanVibrate)
            {
                DataManager.Instance.ControlVibration(false);
                hapticBtn.gameObject.SetActive(true);
            }
            else
            {
                DataManager.Instance.ControlVibration (true);
                hapticBtn.gameObject.SetActive(false);
            }
        }


        private void SoundButtonClicked()
        {
            if (Constants.CanPlaySound)
            {
                DataManager.Instance.ControlSound(false);
                soundBtn.gameObject.SetActive(true);
            }
            else
            {
                DataManager.Instance.ControlSound(true);
                soundBtn.gameObject.SetActive(false);
            }
        }

        
        private void MusicButtonClicked()
        {
            if (Constants.CanPlayMusic)
            {
                DataManager.Instance.ControlMusic(false);
                musicBtn.gameObject.SetActive(true);
            }
            else
            {
                DataManager.Instance.ControlMusic(true);
                musicBtn.gameObject.SetActive(false);
            }
        }

        private void DisableUi()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(EnableUi);

            CanvasManager.Instance.UiClick();
            
            if (!animate)
            {
                child.gameObject.SetActive(false);
                child.transform.localScale = new Vector3(1, 0, 1);
                return;
            }
            
            icon.transform.DORotate(new Vector3(0, 0, 0), 0.4f);

            child.transform.DOScale(new Vector3(1, 0, 1), 0.4f).OnComplete(() =>
            {
                child.gameObject.SetActive(false);
            });
            
            float newAlpha = 1;
            DOTween.To(() => newAlpha, x => newAlpha = x, 0, 0.3f).OnUpdate(() =>
            {
                child.alpha = newAlpha;
            });
        }


        public void ResetPrefs() // TODO remove before release
        {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }
    }
}