using _project.Scripts.Managers;
using _project.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _project.Scripts.UI
{
    public class CoinContainerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;


        private void OnEnable()
        {
            UpdateCoinTextFromData(DataManager.Instance.PlayerData.gemCount);
        }


        public void UpdateCoinTextFromData(int from)
        {
            var to = DataManager.Instance.PlayerData.gemCount;

            DOTween.To(() => from, x => from = x, to, 0.3f).OnUpdate(() =>
            {
                coinText.text = Utilities.FormatPriceToString(from);
            });
        }
    }
}