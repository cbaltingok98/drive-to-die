using System;
using System.Collections.Generic;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.UI.Menu.ShopMenu
{
    public class ShopSkillSection : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        
        private bool initialized;

        private List<ShopSingleItemUi> items;

        public void Init()
        {
            if (initialized) return;
            initialized = true;

            items = new List<ShopSingleItemUi>();

            var skillCount = Enum.GetValues(typeof(SkillType)).Length;

            for (var i = 0; i < skillCount; i++)
            {
                var uiItem = CanvasElementPooler.Instance.SpawnShopSingleItem(UiElementType.ShopSingleItem, parent);
                
                uiItem.Init((SkillType)i);
                
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