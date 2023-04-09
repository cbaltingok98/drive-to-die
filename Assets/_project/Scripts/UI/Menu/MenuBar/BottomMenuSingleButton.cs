using _project.Scripts.Enums;
using _project.Scripts.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace _project.Scripts.UI
{
    public class BottomMenuSingleButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private RectTransform iconTransform;
        [SerializeField] private RectTransform selfRectTransform;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private BottomMenuType bottomMenuType;

        private BottomMenuUI bottomMenuUI;
        
        private bool selected;

        private Vector3 defaultScale;
        private Vector3 iconSelectedScale;
        private Vector3 backgroundSelectedScale;
        private Vector3 backgroundDeSelectedScale;
        private Vector3 sideMove;

        private Vector3 iconSelectedPosition;
        private Vector3 iconDefaultPosition;

        private Tween myTween;

        private bool initialized;


        private void Init()
        {
            if (initialized) return;
            initialized = true;
            
            defaultScale = Vector3.one;
            iconSelectedScale = new Vector3(1f, 1.35f, 1);
            
            backgroundSelectedScale = new Vector3(1.35f, 1f, 1f);
            backgroundDeSelectedScale = new Vector3(0.8f, 1f, 1f);

            iconSelectedPosition = new Vector3(0f, 80f, 0f);
            iconDefaultPosition = new Vector3(0f, 38f, 0f);

            sideMove = new Vector3(20f, -11f, 0f);
        }


        public void AssignMenu(BottomMenuUI bottomMenuUI)
        {
            this.bottomMenuUI = bottomMenuUI;
            titleText.text = bottomMenuType.ToString();
            Init();
        }

        public BottomMenuType GetButtonType() => bottomMenuType;


        public void OnPointerClick(PointerEventData eventData)
        {
            CanvasManager.Instance.UiClick();
            if (!DataManager.Instance.IsPlayedBefore()) return;
            
            if (selected)
            {
                AlreadySelectedAnimation();
                return;
            }
            selected = true;

            bottomMenuUI.MenuButtonClicked(bottomMenuType);
        }


        private void AlreadySelectedAnimation()
        {
            iconTransform.DOScale(iconSelectedScale * 1.1f, 0.15f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                iconTransform.DOScale(iconSelectedScale, 0.15f);
            });
        }


        private void SelectAnimation(int addX)
        {
            iconTransform.DOScale(iconSelectedScale, 0.15f).SetEase(Ease.OutBack);
            iconTransform.DOLocalMove(iconSelectedPosition, 0.15f).SetEase(Ease.OutBack);

            var pos = Vector3.zero;
            pos.y = -11;
            pos.x += addX;
            selfRectTransform.DOLocalMove(pos, 0.15f);
            selfRectTransform.DOScale(backgroundSelectedScale, 0.15f).OnComplete(() =>
            {
                titleText.DOFade(0, 0);
                titleText.gameObject.SetActive(true);
                titleText.DOFade(1, 0.15f);
            });

            background.DOFade(0, 0);
            background.DOFade(1f, 0.15f);
        }


        public void Select(int dir)
        {
            Init();
            
            selected = true;
            var add = dir * 20;
            SelectAnimation(add);
        }
        

        public void DeSelect(int dir, float multiply)
        {
            Init();
            var pos = sideMove;
            pos.x *= dir * multiply;

            titleText.DOFade(0, 0.15f);
            selfRectTransform.DOScale(Vector3.one, 0.15f);
            iconTransform.DOLocalMove(iconDefaultPosition, 0.15f);
            iconTransform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InBack);
            selfRectTransform.DOLocalMove(pos, 0.15f);
            selfRectTransform.DOScale(Vector3.one, 0.15f);

            background.DOFade(0f, 0.15f);

            selected = false;
        }
    }
}