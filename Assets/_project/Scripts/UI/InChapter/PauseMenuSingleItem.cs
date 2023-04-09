using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.InChapter
{
    public class PauseMenuSingleItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI levelText;


        public void Init(Sprite sprite, int level)
        {
            icon.sprite = sprite;
            icon.DOFade(1, 0f);
            levelText.text = level.ToString();
        }


        public void ResetItem()
        {
            icon.sprite = null;
            icon.DOFade(0, 0);
            levelText.text = "";
        }
    }
}