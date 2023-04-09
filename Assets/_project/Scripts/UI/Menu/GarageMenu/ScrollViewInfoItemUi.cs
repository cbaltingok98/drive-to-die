using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu.GarageMenu
{
    public class ScrollViewInfoItemUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private TextMeshProUGUI infoCurrentValueText;
        [SerializeField] private TextMeshProUGUI infoNextValueText;
        [SerializeField] private Image arrowImg;


        public void Init(InfoData newData)
        {
            infoText.text = newData.InfoText.ToUpper();
            infoCurrentValueText.text = newData.CurrentValue.ToString();
            arrowImg.gameObject.SetActive(!newData.IsMaxLevel);

            if (newData.IsMaxLevel)
            {
                infoNextValueText.text = "MAX";
            }
            else
            {
                infoNextValueText.text = newData.NextValue.ToString();
            }
        }
    }

    public class InfoData
    {
        public string InfoText;
        public int CurrentValue;
        public int NextValue;
        public bool IsMaxLevel;
        public int BackgroundIndex;
    }
}