using _project.Scripts.Car;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu
{
    public class ScrollViewItemUi : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Image levelImage;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Animator upgradeIcon;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;

        private CarModels myModel;

        private bool interactable;
        
        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }


        public void SetText(string txt)
        {
            text.text = txt;
        }


        public void UpdateInfo()
        {
            var drivingProfile = CarGarage.Instance.GetModel(myModel).GetDrivingProfileType();
            var isMaxedOut = DrivingProfileManager.IsCarMaxedOut(drivingProfile);
            var isUnlocked = DataManager.Instance.IsCarUnlocked(myModel);
            
            if (isUnlocked)
            {
                upgradeIcon.gameObject.SetActive(!isMaxedOut);
            }
            else
            {
                upgradeIcon.gameObject.SetActive(false);
            }
            
            levelText.text = DrivingProfileManager.GetDrivingProfile(drivingProfile).level.ToString();
        }


        public void AddListenerToButton(UnityAction<CarModels> action,UnityAction<ScrollViewItemUi> clickAction, CarModels carModel)
        {
            transform.localScale = Vector3.one;
            levelImage.transform.localScale = Vector3.zero;

            myModel = carModel;
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { action(myModel); });
            button.onClick.AddListener(delegate { clickAction(this); });
            button.onClick.AddListener(PlaySound);
            
            UpdateInfo();
        }

        // animate and show car level and arrow up if not max leve


        private void PlaySound()
        {
            SoundManager.Instance.Play(SoundType.UiClick);
        }


        public void ClickAnimation()
        {
            interactable = false;
            transform.DOScale(Vector3.one * 1.1f, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                interactable = true;
            });

            levelImage.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
        }


        public void DeSelect()
        {
            interactable = false;
            transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                interactable = true;
            });

            levelImage.transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.OutBack);
        }
    }
}