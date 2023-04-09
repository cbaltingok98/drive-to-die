using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _project.Scripts.UI
{
    public class DamageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private RectTransform rectTransform;
        private Transform parent;

        private Vector3 startScale;
        

        public void Awake()
        {
            rectTransform = text.rectTransform;
            startScale = rectTransform.localScale;
        }
        
        private void GoBackToParent()
        {
            transform.SetParent(parent);
        }
        private void OnEnable()
        {
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = startScale;

            var pos = Vector3.zero;
            pos.y += 3.1f;

            text.alpha = 1f;
            // text.DOFade(1, 0);
            text.DOFade(0.4f, 0.6f).OnComplete(() =>
            {
               //GoBackToParent();
               gameObject.SetActive(false);
            });

            rectTransform.DOLocalMove(pos, 0.6f);
            // _rectTransform.DOScale(_startScale * 0.65f, 0.8f);
        }


        public void Reset()
        {
            gameObject.SetActive(false);
        }
    }
}