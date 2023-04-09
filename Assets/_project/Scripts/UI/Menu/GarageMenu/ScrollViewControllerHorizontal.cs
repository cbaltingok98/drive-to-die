using System;
using System.Collections.Generic;
using _project.Scripts.Car;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace _project.Scripts.UI.Menu.GarageMenu
{
    public class ScrollViewControllerHorizontal : MonoBehaviour
    {
        [SerializeField] private Transform contentParent;

        private GarageMenuUi garageMenuUi;

        private List<ScrollViewItemUi> scrollViewItems;

        private bool initialized;

        private ScrollViewItemUi clickedItem;

        public void Init(GarageMenuUi garageMenuUi)
        {
            var pos = contentParent.localPosition;
            pos.x = 0f;
            contentParent.localPosition = pos;

            if (initialized)
            {
                ResetSelection();
                UpdateInfo();
                return;
            }
            
            initialized = true;

            scrollViewItems = new List<ScrollViewItemUi>();

            this.garageMenuUi = garageMenuUi;

            var carModelCount = Enum.GetValues(typeof(CarModels)).Length;

            for (var i = 0; i < carModelCount; i++)
            {
                var newScrollViewItem = CanvasElementPooler.Instance.SpawnScrollViewItemFromPool(UiElementType.ScrollViewItemUi, contentParent);
                // newScrollViewItem.SetSprite();
                var model = (CarModels)i;
                
                newScrollViewItem.SetSprite(CarGarage.Instance.GetModel(model).GetGarageSprite());
                
                newScrollViewItem.SetText(model.ToString());// todo remove
                
                newScrollViewItem.AddListenerToButton(SwitchShowRoomCar, ButtonClicked, model);

                scrollViewItems.Add(newScrollViewItem);

                if ((CarModels)i == DataManager.Instance.GetCurrentCarModel())
                {
                    clickedItem = newScrollViewItem;
                    newScrollViewItem.ClickAnimation();
                }
            }
        }


        private void ResetSelection()
        {
            for (var i = 0; i < scrollViewItems.Count; i++)
            {
                if ((CarModels)i == DataManager.Instance.GetCurrentCarModel())
                {
                    clickedItem = scrollViewItems[i];
                    scrollViewItems[i].ClickAnimation();
                }
                else
                {
                    scrollViewItems[i].DeSelect();
                }
            }
        }

        public void UpdateInfo()
        {
            for (var i = 0; i < scrollViewItems.Count; i++)
            {
                scrollViewItems[i].UpdateInfo();
            }
        }


        public void ButtonClicked(ScrollViewItemUi clickedItem)
        {
            if (clickedItem == this.clickedItem) return;
            
            this.clickedItem.DeSelect();
            clickedItem.ClickAnimation();

            this.clickedItem = clickedItem;
        }

        public void SwitchShowRoomCar(CarModels carModel)
        {
            garageMenuUi.SwitchCar(carModel);
        }


        public void ResetItems()
        {
            for (var i = 0; i < scrollViewItems.Count; i++)
            {
                if ((CarModels)i == DataManager.Instance.GetCurrentCarModel())
                {
                    clickedItem = scrollViewItems[i];
                    scrollViewItems[i].ClickAnimation();
                }
                else
                {
                    scrollViewItems[i].DeSelect();   
                }
            }
        }
    }
}