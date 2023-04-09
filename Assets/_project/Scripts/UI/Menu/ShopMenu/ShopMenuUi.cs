using _project.Scripts.Enums;
using _project.Scripts.UI.Interface;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.UI.Menu.ShopMenu
{
    
    
    public class ShopMenuUi : MenuUiElement
    {
        [SerializeField] private ShopCarSectionUi shopCarSectionUi;
        [SerializeField] private ShopSkillSection shopSkillSection;
        [SerializeField] private Transform background;
        [SerializeField] private CanvasGroup carSection;
        [SerializeField] private CanvasGroup avatarSection;
        [SerializeField] private CanvasGroup skillSection;
        
        
        public override BottomMenuType GetMenuType() => menuType;

        public override void Enable()
        {
            gameObject.SetActive(true);
            
            shopCarSectionUi.Init();
            //shopSkillSection.Init();
            
            StartAnimation();
        }
        
        public override void Disable()
        {
            gameObject.SetActive(false);
        }

        public override void UpdateInfo()
        {
            shopCarSectionUi.UpdateInfo();
            //shopSkillSection.UpdateInfo();
        }


        private void StartAnimation()
        {
            background.localScale = new Vector3(1f, 0f, 1f);
            carSection.alpha = 0;
            avatarSection.alpha = 0;
            skillSection.alpha = 0;

            background.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                float newAlpha = 0;
                DOTween.To(() => newAlpha, x => newAlpha = x, 1, 0.3f).OnUpdate(() =>
                {
                    carSection.alpha = newAlpha;
                });
                
                float newAlpha2 = 0;
                DOTween.To(() => newAlpha2, x => newAlpha2 = x, 1, 0.3f).SetDelay(0.15f).OnUpdate(() =>
                {
                    avatarSection.alpha = newAlpha2;
                });
                
                float newAlpha3 = 0;
                DOTween.To(() => newAlpha3, x => newAlpha3 = x, 1, 0.3f).SetDelay(0.3f).OnUpdate(() =>
                {
                    skillSection.alpha = newAlpha3;
                });
            });
        }
    }
}