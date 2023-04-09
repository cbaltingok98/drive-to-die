using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.ShowRoom;
using _project.Scripts.Skills;
using _project.Scripts.Skills.Skill_Logic;
using _project.Scripts.UI.Interface;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _project.Scripts.UI.Menu
{
    public class ArmoryMenuUi : MenuUiElement
    {
        [SerializeField] private int rowSize;
        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private ScrollViewControllerHorizontalArmory scrollViewControllerHorizontalArmory;
        [SerializeField] private Transform background;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private DetailedInfoPanel detailedInfoPanel;
        
        public override BottomMenuType GetMenuType() => menuType;

        private SkillSystemHelper skillSystemHelper;

        public override void Enable()
        {
            if (skillSystemHelper == null)
            {
                skillSystemHelper = SkillSystemHelper.Instance;
            }
            
            gameObject.SetActive(true);
            ShowRoomManager.Instance.ArmoryMenuEnabled();
            scrollViewControllerHorizontalArmory.Init(this, rowSize);
            skillName.text = SkillSystemHelper.Instance.GetSkill(SkillSystemHelper.Instance.GetFirstAvailableSkill())
                .GetName().ToUpper();
            StartAnimations();
        }

        public override void Disable()
        {
            gameObject.SetActive(false);
            ShowRoomManager.Instance.ArmoryMenuDisabled();
            scrollViewControllerHorizontalArmory.ResetItems();
        }


        public override void UpdateInfo()
        {
            scrollViewControllerHorizontalArmory.UpdateInfo();
        }


        public void SwitchSkill(SkillType skillType, string skillName)
        {
            ShowRoomManager.Instance.SwitchShowroomSkill(skillType);
            this.skillName.text = skillName.ToUpper();
            CanvasManager.Instance.UiClick();
        }


        public void OpenDetailedInfoPanel(Skill skill)
        {
            detailedInfoPanel.OpenInfoPanel(skill, UpdateInfo);
        }


        private void StartAnimations()
        {
            var targetScale = background.localScale;
            
            canvasGroup.alpha = 0;
            background.localScale = new Vector3(targetScale.x, 0, targetScale.z);

            background.DOScale(targetScale, 0.3f).OnComplete(() =>
            {
                float newAlpha = 0;
                DOTween.To(() => newAlpha, x => newAlpha = x, 1, 0.3f).OnUpdate(() =>
                {
                    canvasGroup.alpha = newAlpha;
                });
            });
        } 
    }
}