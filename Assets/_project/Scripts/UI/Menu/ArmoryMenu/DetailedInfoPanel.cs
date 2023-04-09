using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Skills.Skill_Logic;
using _project.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu
{
    public class DetailedInfoPanel : MonoBehaviour
    {
        [SerializeField] private Transform mainBody;
        [SerializeField] private Transform content;
        [SerializeField] private Image skillIcon;
        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private TextMeshProUGUI skillLevel;
        [SerializeField] private TextMeshProUGUI skillInfo;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundBtn;
        [SerializeField] private ParticleSystem particle;

        private SkillType skillType;
        private Skill skill;
        private int price;


        public void OpenInfoPanel(Skill skill, UnityAction upgradeAction)
        {
            var pos = content.localPosition;
            pos.y = 0;
            content.localPosition = pos;
            
            skillType = skill.GetSkillType();
            this.skill = skill;
            
            SetIcon(this.skill.GetSkillSprite());
            SetName(this.skill.GetName());
            
            UpdateInfo();
            
            gameObject.SetActive(true);
            
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseButtonClicked);
            closeButton.onClick.AddListener(upgradeAction);
            
            backgroundBtn.onClick.RemoveAllListeners();
            backgroundBtn.onClick.AddListener(CloseButtonClicked);
            backgroundBtn.onClick.AddListener(upgradeAction);
        }


        public void CloseButtonClicked()
        {
            gameObject.SetActive(false);
        }


        private void UpdateInfo()
        {
            var curLevel = DataManager.Instance.GetSkillLevelFor(skillType);
            var maxLevel = skill.GetMaxLevel();
            
            SetLevelText(curLevel, maxLevel);
            SetInfoText(skill.GetSkillInfoForArmory());

            upgradeButton.onClick.RemoveAllListeners();
            
            if (curLevel < maxLevel)
            {
                upgradeButton.gameObject.SetActive(true);
                priceText.transform.parent.gameObject.SetActive(true);
                SetPrice();
                
                upgradeButton.onClick.AddListener(UpgradeButtonClicked);
            }
            else
            {
                upgradeButton.gameObject.SetActive(false);
                priceText.transform.parent.gameObject.SetActive(false);
            }
        }


        private void UpgradeButtonClicked()
        {
            upgradeButton.transform.DOScale(Vector3.one * 0.9f, 0.1f).OnComplete(() =>
            {
                upgradeButton.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
            });

            if (!DataManager.Instance.SpendCoin(price))
            {
                SoundManager.Instance.Play(SoundType.UiNotEnoughMaterial);
                return;
            }

            DataManager.Instance.UpdateSkillLevel(skillType);
            UpdateInfo();
            CanvasManager.Instance.UiClick();
            
            SoundManager.Instance.Play(SoundType.UiUnlock);
            particle.Play();
        }


        private void SetPrice()
        {
            var previous = price;
            price = skill.GetUpgradePrice();
            
            DOTween.To(() => previous, x => previous = x, price, 0.3f).OnUpdate(() =>
            {
                priceText.text = Utilities.FormatPriceToString(previous);
            });
        }


        private void SetIcon(Sprite sprite)
        {
            skillIcon.sprite = sprite;
        }


        private void SetName(string skillName)
        {
            this.skillName.text = skillName;
        }


        private void SetLevelText(int level, int maxLevel)
        {
            this.skillLevel.text = "Level: " + level + "/" + maxLevel;
        }


        private void SetInfoText(string info)
        {
            skillInfo.text = info;
        }
    }
}