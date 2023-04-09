using _project.Scripts.Berk;
using _project.Scripts.Car;
using _project.Scripts.Data;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.ShowRoom;
using _project.Scripts.UI.Interface;
using _project.Scripts.UI.Menu.GarageMenu;
using _project.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu
{
    public class GarageMenuUi : MenuUiElement
    {
        [SerializeField] private Image background;
        [SerializeField] private CanvasGroup horizontalScrollCanvasGroup;
        [SerializeField] private ScrollViewControllerHorizontal scrollViewControllerHorizontal;
        [SerializeField] private DrivingSpecsInfoController drivingSpecsInfoController;
        [SerializeField] private GameObject upgradeSection;
        [SerializeField] private Transform warningMessage;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TextMeshProUGUI upgradeButtonText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Image priceImage;

        private int price;

        public override BottomMenuType GetMenuType() => menuType;

        public override void Enable()
        {
            gameObject.SetActive(true);
            ShowRoomManager.Instance.GarageMenuEnabled();
            scrollViewControllerHorizontal.Init(this);
            drivingSpecsInfoController.Init(DataManager.Instance.PlayerData.currentCarModel);
            CheckCarInfoForAvailabilityAndMaxLevel(DataManager.Instance.PlayerData.currentCarModel);
            
            StartAnimation();
        }


        public override void Disable()
        {
            gameObject.SetActive(false);
            scrollViewControllerHorizontal.ResetItems();
            //drivingSpecsInfoController.Reset();

            ShowRoomManager.Instance.GarageMenuDisabled();
        }


        public override void UpdateInfo()
        {
            
        }


        public void SwitchCar(CarModels carModel)
        {
            ShowRoomManager.Instance.SwitchShowRoomCar(carModel);
            drivingSpecsInfoController.Init(carModel);
            CheckCarInfoForAvailabilityAndMaxLevel(carModel);
            CanvasManager.Instance.UiClick();
        }


        private void CheckCarInfoForAvailabilityAndMaxLevel(CarModels carModel)
        {
            upgradeButton.onClick.RemoveAllListeners();
            var isUnlocked = DataManager.Instance.IsCarUnlocked(carModel);
            
            if (!isUnlocked)
            {
                upgradeSection.SetActive(false);
                warningMessage.gameObject.SetActive(true);
                return;
            }
            
            warningMessage.gameObject.SetActive(false);
            upgradeSection.SetActive(true);
            
            var drivingProfile = CarGarage.Instance.GetModel(carModel).GetDrivingProfileType();
            var isMaxedOut = DrivingProfileManager.IsCarMaxedOut(drivingProfile);

            if (isMaxedOut)
            {
                priceImage.gameObject.SetActive(false);
                upgradeButtonText.text = "MAX LEVEL";
                return;
            }

            var previous = price;
            price = DrivingProfileManager.GetUpgradePriceFor(drivingProfile);

            upgradeButtonText.text = "UPGRADE";
            priceImage.gameObject.SetActive(true);
            
            DOTween.To(() => previous, x => previous = x, price, 0.3f).OnUpdate(() =>
            {
                priceText.text = Utilities.FormatPriceToString(previous);
            });

            upgradeButton.onClick.AddListener(delegate { UpgradeButtonClicked(drivingProfile, carModel, price); });
        }


        private void UpgradeButtonClicked(DrivingProfileType drivingProfileType, CarModels model, int price)
        {
            if (!DataManager.Instance.SpendCoin(price))
            {
                SoundManager.Instance.Play(SoundType.UiNotEnoughMaterial);
                return;
            }
            
            DataManager.Instance.UpdateCarLevel(drivingProfileType);
            CheckCarInfoForAvailabilityAndMaxLevel(model);
            drivingSpecsInfoController.Init(model);
            scrollViewControllerHorizontal.UpdateInfo();

            upgradeButton.transform.DOScale(Vector3.one * 0.9f, 0.1f).OnComplete(() =>
            {
                upgradeButton.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
            });
            
            SoundManager.Instance.Play(SoundType.UiUnlock);
        }


        private void StartAnimation()
        {
            horizontalScrollCanvasGroup.alpha = 0;
            background.transform.localScale = new Vector3(1f, 0f, 1f);
            
            background.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                float newAlpha = 0;
                DOTween.To(() => newAlpha, x => newAlpha = x, 1, 0.3f).OnUpdate(() =>
                {
                    horizontalScrollCanvasGroup.alpha = newAlpha;
                });
            });
        }
    }
}