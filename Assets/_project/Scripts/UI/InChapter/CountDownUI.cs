using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _project.Scripts.UI
{
    public class CountDownUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup countTextCanvasGroup;
        [SerializeField] private TextMeshProUGUI countText;
        [Header("References")]
        [SerializeField] private Transform animateCommingFromTr;
        [SerializeField] private Transform animateTargetTr;
        
        private float durationMultiplier = 0.65f; //1 iken 1sn suruyor

        private void Start()
        {
            ResetUi();
        }

        private void ResetUi()
        {
            countText.text = "";
            countText.transform.position = animateTargetTr.position;
            countTextCanvasGroup.alpha = 0f;
        }

        private void ControlActive(bool set)
        {
            gameObject.SetActive(set);
        }

        public void CountDownFrom3(Action completedAction)
        {
            ControlActive(true);
            countText.transform.position = animateCommingFromTr.position;
            CountNumber("3", () =>
            {
                CountNumber("2", () =>
                {
                    CountNumber("1", () =>
                    {
                        GoGoGo("SURVIVE!", completedAction);
                    });
                });
            });
        }

        private void CountNumber(string text, Action completedAction)
        {
            countTextCanvasGroup.alpha = 0f;

            countText.text = text;

            countText.transform.position = animateCommingFromTr.position;

            countTextCanvasGroup.DOFade(1f, 0.25f * durationMultiplier);

            countText.transform.DOMove(animateTargetTr.position, 0.5f * durationMultiplier).OnComplete(() =>
             {
                 countTextCanvasGroup.DOFade(0f, 0.1f).SetDelay(0.4f * durationMultiplier).OnComplete(() =>
                 {
                     completedAction?.Invoke();
                 });
             }).SetEase(Ease.OutQuad);
        }


        private void GoGoGo(string text, Action completedAction)
        {
            countTextCanvasGroup.alpha = 0f;

            countText.text = text;

            countText.transform.position = animateTargetTr.position;

            countText.transform.localScale = Vector3.one * 3f;

            countTextCanvasGroup.DOFade(1f, 0.1f);
            countText.transform.DOScale(1f, 0.2f).OnComplete(() =>
            {
                completedAction?.Invoke();
                countTextCanvasGroup.DOFade(0f, 0.1f).SetDelay(0.2f).OnComplete(() =>
                {
                    ResetUi();
                    ControlActive(false);
                });
            });
        }

    }
}