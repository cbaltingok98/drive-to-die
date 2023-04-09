using System;
using System.Collections.Generic;
using _project.Scripts.Enums;
using _project.Scripts.Skills;
using UnityEngine;

namespace _project.Scripts.UI.Menu
{
    public class ScrollViewControllerHorizontalArmory : MonoBehaviour
    {
        [SerializeField] private List<Transform> contentParents;

        private ArmoryMenuUi armoryMenuUi;

        private List<ScrollViewItemArmoryUi> scrollViewItems;

        private ScrollViewItemArmoryUi clickedItem;

        private bool initialized;

        public void Init(ArmoryMenuUi armoryMenuUi, int rowSize)
        {
            if (initialized) return;
            initialized = true;

            scrollViewItems = new List<ScrollViewItemArmoryUi>();

            this.armoryMenuUi = armoryMenuUi;

            var skills = SkillSystemHelper.Instance.GetSkillDictionary();

            var parentIndex = -1;
            var counter = rowSize;

            foreach (var skill in skills.Keys)
            {
                if (counter >= rowSize)
                {
                    counter = 0;
                    parentIndex += 1;
                    contentParents[parentIndex].gameObject.SetActive(true);
                }
                
                var newScrollViewItem = CanvasElementPooler.Instance.SpawnScrollViewArmoryInfoItemFromPool(UiElementType.ScrollViewInfoItemArmory, contentParents[parentIndex]);

                newScrollViewItem.AddListenerToButton(SwitchShowRoomSkill, ButtonClicked, skill);
                newScrollViewItem.AddListenerToInfoButton(this.armoryMenuUi.OpenDetailedInfoPanel);

                scrollViewItems.Add(newScrollViewItem);

                if (skill == SkillSystemHelper.Instance.GetFirstAvailableSkill())
                {
                    clickedItem = newScrollViewItem;
                    clickedItem.ClickAnimation();
                }

                counter += 1;
            }
        }


        public void SwitchShowRoomSkill(SkillType skillType, string skillName)
        {
            armoryMenuUi.SwitchSkill(skillType, skillName);
        }
        
        public void ButtonClicked(ScrollViewItemArmoryUi clickedItem)
        {
            if (clickedItem == this.clickedItem) return;
            
            this.clickedItem.DeSelect();
            clickedItem.ClickAnimation();
            
            this.clickedItem = clickedItem;
        }


        public void UpdateInfo()
        {
            for (var i = 0; i < scrollViewItems.Count; i++)
            {
                scrollViewItems[i].UpdateInfo();
            }
        }


        public void ResetItems()
        {
            for (var i = 0; i < scrollViewItems.Count; i++)
            {
                if (i == 0)
                {
                    clickedItem = scrollViewItems[i];
                    clickedItem.ClickAnimation();
                }
                else
                {
                    scrollViewItems[i].DeSelect();
                }
            }
        }
    }
}