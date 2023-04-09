using System.Collections.Generic;
using _project.Scripts.Berk;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu.GarageMenu
{
    public class SpecItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image background;
        [SerializeField] private List<SingleSpecBar> bars;
        [SerializeField] private List<Sprite> backgrounds;
        
        
        public void Init(InfoData newData)
        {
            titleText.text = newData.InfoText;
            background.sprite = backgrounds[newData.BackgroundIndex];

            for (var i = 0; i < bars.Count; i++)
            {
                if (newData.CurrentValue >= i)
                {
                    bars[i].LightUp();
                }
                else if (!newData.IsMaxLevel && newData.NextValue >= i)
                {
                    bars[i].ControlAnimator(true);
                }
                else
                {
                    bars[i].TurnOff();
                }
            }
        }
    }
}