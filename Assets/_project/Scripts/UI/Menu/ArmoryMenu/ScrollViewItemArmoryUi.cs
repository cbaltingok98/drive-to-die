using _project.Scripts.Car;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Skills;
using _project.Scripts.Skills.Skill_Logic;
using _project.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu
{
    public class ScrollViewItemArmoryUi : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Animator upgradeIcon;
        [SerializeField] private Button button;
        [SerializeField] private Button infoButton;
        [SerializeField] private GameObject deselectedLayer;
        [SerializeField] private GameObject lockedIcon;
        [SerializeField] private GameObject levelInfoBar;
        [SerializeField] private GameObject priceBar;
        [SerializeField] private TextMeshProUGUI priceText;

        private Skill mySkill;
        private SkillType mySkillType;

        private bool interactable;
        private bool isUnlocked;
        
        private void SetSprite(Sprite sprite)
        {
            icon.sprite = sprite;
        }


        public void UpdateInfo()
        {
            mySkill = SkillSystemHelper.Instance.GetSkill(mySkillType);
            
            SetSprite(mySkill.GetSkillSprite());

            levelText.text = "LvL " + DataManager.Instance.GetSkillLevelFor(mySkillType);

            isUnlocked = DataManager.Instance.IsSkillUnlocked(mySkillType);

            if (isUnlocked)
            {
                upgradeIcon.gameObject.SetActive(!mySkill.IsSkillLevelMaxedOut());   
                lockedIcon.SetActive(false);
                icon.gameObject.SetActive(true);
                levelInfoBar.SetActive(true);
                priceBar.SetActive(!mySkill.IsSkillLevelMaxedOut());
                priceText.text = Utilities.FormatPriceToString(mySkill.GetUpgradePrice());
            }
            else
            {
                upgradeIcon.gameObject.SetActive(false);
                lockedIcon.SetActive(true);
                icon.gameObject.SetActive(false);
                levelInfoBar.SetActive(false);
                priceBar.SetActive(false);
            }
        }
        
        
        public void AddListenerToButton(UnityAction<SkillType, string> action,UnityAction<ScrollViewItemArmoryUi> clickAction, SkillType skillType)
        {
            transform.localScale = Vector3.one;
            infoButton.gameObject.SetActive(false);
            
            mySkillType = skillType;
            UpdateInfo();

            if (!isUnlocked) return;
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { action(skillType, mySkill.GetName()); });
            button.onClick.AddListener(delegate { clickAction(this); });
            button.onClick.AddListener(PlaySound);
        }


        public void AddListenerToInfoButton(UnityAction<Skill> action)
        {
            infoButton.onClick.RemoveAllListeners();
            infoButton.onClick.AddListener(delegate { action(mySkill); });
            infoButton.onClick.AddListener(AnimateInfoBtn);
        }


        private void AnimateInfoBtn()
        {
            infoButton.transform.DOScale(Vector3.one * 0.9f, 0.1f).OnComplete(() =>
            {
                infoButton.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
            });
        }


        private void PlaySound()
        {
            SoundManager.Instance.Play(SoundType.UiClick2);
        }
        
        public void ClickAnimation()
        {
            interactable = false;
            transform.DOScale(Vector3.one * 1.1f, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                interactable = true;
            });
            
            infoButton.gameObject.SetActive(true);
            deselectedLayer.SetActive(false);
        }


        public void DeSelect()
        {
            interactable = false;
            transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                interactable = true;
            });
            
            infoButton.gameObject.SetActive(false);
            deselectedLayer.SetActive(true);
        }
    }
}