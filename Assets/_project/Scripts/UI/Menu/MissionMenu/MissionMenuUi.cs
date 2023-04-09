using System;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.UI.Interface;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu
{
    public class MissionMenuUi : MenuUiElement
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private CanvasGroup menuTitle;
        
        public override BottomMenuType GetMenuType() => menuType;
        
        
        public override void Enable()
        {
            CanvasManager.Instance.HideMoneyUI();
            gameObject.SetActive(true);
            StartAnimations();
        }


        public override void Disable()
        {
            gameObject.SetActive(false);
            CanvasManager.Instance.ShowMoneyUI();
        }


        public override void UpdateInfo()
        {
            
        }


        private void StartAnimations()
        {
            menuTitle.alpha = 0;
            backgroundImage.transform.localScale = new Vector3(1f, 0f, 1f);
            
            backgroundImage.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                float newAlpha = 0;
                DOTween.To(() => newAlpha, x => newAlpha = x, 1, 0.3f).OnUpdate(() =>
                {
                    menuTitle.alpha = newAlpha;
                });
            });
        }
    }
}