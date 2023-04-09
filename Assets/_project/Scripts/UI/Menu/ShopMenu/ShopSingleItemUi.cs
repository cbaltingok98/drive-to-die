using _project.Scripts.Car;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Skills;
using _project.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu.ShopMenu
{
    public class ShopSingleItemUi : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject buttonSection;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI priceText;

        private CarModels myModel;
        private SkillType mySkillType;

        private int price;


        #region Car

        public void Init(CarModels carModel)
        {
            myModel = carModel;
            
            icon.sprite = CarGarage.Instance.GetModel(myModel).GetShopSprite();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(UnlockButtonPressedForCar);
            
            UpdateInfoForCarType();

            transform.localScale = Vector3.one;
        }
        
        public void UpdateInfoForCarType()
        {
            if (DataManager.Instance.IsCarUnlocked(myModel))
            {
                buttonSection.SetActive(false);
                return;
            }
            
            buttonSection.gameObject.SetActive(true);
            price = CarGarage.Instance.GetModel(myModel).GetUnlockPrice();
            priceText.text = Utilities.FormatPriceToString(price);
        }
        
        private void UnlockButtonPressedForCar()
        {
            if (DataManager.Instance.SpendCoin(price))
            {
                DataManager.Instance.UnlockCar(myModel);
                UpdateInfoForCarType();
                CanvasManager.Instance.UiClick();
                SoundManager.Instance.Play(SoundType.UiUnlock);
            }
            else
            {
                SoundManager.Instance.Play(SoundType.UiNotEnoughMaterial);
            }

            button.transform.DOScale(Vector3.one * 0.9f, 0.1f).OnComplete(() =>
            {
                button.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
            });
        }

        #endregion
        
        #region Skill

        public void Init(SkillType skillType)
        {
            mySkillType = skillType;

            icon.sprite = SkillSystemHelper.Instance.GetSkill(mySkillType).GetSkillSprite();
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(UnlockButtonPressedForSkill);
            
            UpdateInfoForSkillType();

            transform.localScale = Vector3.one;
        }
        
        public void UpdateInfoForSkillType()
        {
            if (DataManager.Instance.IsSkillUnlocked(mySkillType))
            {
                buttonSection.SetActive(false);
                return;
            }
            
            buttonSection.gameObject.SetActive(true);
            price = SkillSystemHelper.Instance.GetSkill(mySkillType).GetUnlockPrice();
            priceText.text = Utilities.FormatPriceToString(price);
        }
        
        private void UnlockButtonPressedForSkill()
        {
            if (DataManager.Instance.SpendCoin(price))
            {
                DataManager.Instance.UnlockSkill(mySkillType);
                UpdateInfoForSkillType();
                CanvasManager.Instance.UiClick();
            }
        }

        #endregion
        
        public void UpdateInfo()
        {
            
        }
    }
}