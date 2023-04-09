using System;
using System.Collections.Generic;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.UI.Menu.ShopMenu
{
    public class ShopCarSectionUi : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        
        private bool initialized;

        private List<ShopSingleItemUi> items;

        public void Init()
        {
            if (initialized) return;
            initialized = true;

            items = new List<ShopSingleItemUi>();

            var modelCount = Enum.GetValues(typeof(CarModels)).Length;

            for (var i = 0; i < modelCount; i++)
            {
                var uiItem = CanvasElementPooler.Instance.SpawnShopSingleItem(UiElementType.ShopSingleItem, parent);
                
                uiItem.Init((CarModels)i);
                
                items.Add(uiItem);
            }
        }


        public void UpdateInfo()
        {
            for (var i = 0; i < items.Count; i++)
            {
                items[i].UpdateInfo();
            }
        }
    }
}