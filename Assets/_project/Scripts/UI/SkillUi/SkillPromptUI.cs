using System.Collections.Generic;
using _project.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _project.Scripts.UI
{
    public class SkillPromptUI : MonoBehaviour
    {
        [SerializeField] private List<SingleSkillUi> singleSkillUis;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private RectTransform optionSection;

        private Tween myTween;

        public void ShowPrompt(List<SkillPromptInfo> skillPromptInfos, UnityAction action)
        {
            optionSection.localScale = Vector3.zero;
            
            gameObject.SetActive(true);
            
            backgroundImage.DOFade(0f, 0);
            backgroundImage.DOFade(0.6f, 0.3f);
            
            for (var i = 0; i < skillPromptInfos.Count; i++)
            {
                singleSkillUis[i].Initialize(skillPromptInfos[i]);
            }
            
            myTween = optionSection.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).SetDelay(0.15f).OnComplete(() =>
            {
                action?.Invoke();
            });
        }

        public void HidePrompt(UnityAction action)
        {
            action?.Invoke();

            optionSection.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).SetDelay(0.1f);
            backgroundImage.DOFade(0f, 0.2f).SetDelay(0.15f).OnComplete(() =>
            {
                for (var i = 0; i < singleSkillUis.Count; i++)
                {
                    singleSkillUis[i].gameObject.SetActive(false);
                }
                gameObject.SetActive(false);
            });
        }


        public void ResetPrompt()
        {
            foreach (var singleSkillUi in singleSkillUis)
            {
                singleSkillUi.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
        }


        public void KillTween()
        {
            myTween?.Kill();
        }
    }
}