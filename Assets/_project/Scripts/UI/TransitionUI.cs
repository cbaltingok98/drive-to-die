using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Themes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _project.Scripts.UI
{
    public class TransitionUI : MonoBehaviour
    {
        private const float FadeOutDuration = 1F;
        private const float FadeInDuration = 1F;
        private const float DelayDuration = 0.5F;
        private const float DelayDurationAfterFadeOut = 2.5F;
        
        public Image fadeImage;
        // public Button fadeButton;


        private void Awake()
        {
            fadeImage.color = Color.black;
            fadeImage.DOFade(1F, 0F);
            fadeImage.enabled = true;
        }


        public void FadeIn(UnityAction action, UnityAction action2)
        {
            action?.Invoke();
            fadeImage.DOFade(1F, 0F);
            fadeImage.enabled = true;
            fadeImage.color = Color.white;

            fadeImage.DOFade(0F, FadeInDuration)
                .SetDelay(DelayDurationAfterFadeOut)
                .SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    fadeImage.enabled = false;
                    action2?.Invoke();
                });
        }
        
        public void FadeIn(UnityAction action)
        {
            action?.Invoke();
            fadeImage.DOFade(1F, 0F);
            fadeImage.enabled = true;
            fadeImage.color = Color.white;

            fadeImage.DOFade(0F, FadeInDuration)
                .SetDelay(DelayDuration)
                .SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    fadeImage.enabled = false;
                });
        }
        
        public void FadeIn(List<UnityAction> action)
        {
            foreach (var unityAction in action)
            {
                unityAction?.Invoke();
            }
            
            fadeImage.DOFade(1F, 0F);
            fadeImage.enabled = true;
            fadeImage.color = Color.white;

            fadeImage.DOFade(0F, FadeInDuration)
                .SetDelay(DelayDuration)
                .SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    fadeImage.enabled = false;
                });
        }
        
        public void FadeIn()
        {
            fadeImage.DOFade(1F, 0F);
            fadeImage.enabled = true;
            fadeImage.color = Color.black;

            fadeImage.DOFade(0F, FadeInDuration)
                .SetDelay(DelayDuration)
                .SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    fadeImage.enabled = false;
                });
        }


        public void FadeOutAndFadeIn(UnityAction action,UnityAction action2, LevelThemes theme)
        {
            fadeImage.DOFade(0F, 0F);
            fadeImage.sprite = ThemeImageManager.GetThemeImage(theme);
            fadeImage.color = Color.white;
            fadeImage.enabled = true;

            fadeImage.DOFade(1F, FadeOutDuration)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                { 
                    FadeIn(action, action2);
                });
        }
        
        public void FadeOutAndFadeInToMenu(UnityAction action, UnityAction action2, LevelThemes theme)
        {
            fadeImage.DOFade(0F, 0F);
            fadeImage.sprite = ThemeImageManager.GetThemeImage(theme);
            fadeImage.color = Color.white;
            fadeImage.enabled = true;

            fadeImage.DOFade(1F, FadeOutDuration)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    action?.Invoke();
                    FadeIn(action2);
                });
        }
        
        public void FadeOutAndFadeInToMenu(UnityAction action, List<UnityAction> action2, LevelThemes theme)
        {
            fadeImage.DOFade(0F, 0F);
            fadeImage.sprite = ThemeImageManager.GetThemeImage(theme);
            fadeImage.color = Color.white;
            fadeImage.enabled = true;

            fadeImage.DOFade(1F, FadeOutDuration)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    action?.Invoke();
                    FadeIn(action2);
                });
        }
        
        // public void FadeInAndFadeOut(UnityAction action, LevelThemes theme)
        // {
        //     fadeImage.sprite = ThemeImageManager.GetThemeImage(theme);
        //     
        //     fadeImage.DOFade(0F, 0F);
        //     //fadeImage.color = Color.black;
        //     fadeImage.enabled = true;
        //     //fadeButton.interactable = true;
        //
        //     fadeImage.DOFade(1F, FadeOutDuration)
        //         .SetEase(Ease.OutSine)
        //         .OnComplete(() =>
        //         {
        //             FadeIn(action);
        //         });
        // }

        public void FadeOutAndFadeIn(LevelThemes theme)
        {
            fadeImage.sprite = ThemeImageManager.GetThemeImage(theme);
            
            fadeImage.DOFade(0F, 0F);
            //fadeImage.color = Color.black;
            fadeImage.enabled = true;
            //fadeButton.interactable = true;

            fadeImage.DOFade(1F, FadeOutDuration)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    
                });
        }
    }
}