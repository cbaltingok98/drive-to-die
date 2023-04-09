using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _project.Scripts.UI
{
    public enum WarningType
    {
        Fuel,
        NotEnoughMaterial,
        Death,
        LowFuel,
        ShieldOn,
        ShieldOff
    }
    
    public class WarningUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI warningMessage;
        [SerializeField] private Transform warningTransform;

        private Queue<Action> warningQueue;

        private bool warningInPlace;
        

        public void ShowWarning(WarningType warningType)
        {
            if (warningQueue == null)
            {
                warningQueue = new Queue<Action>();    
            }
            
            switch (warningType)
            {
                case WarningType.Fuel:
                    // FuelWarning();
                    warningQueue.Enqueue(FuelWarning);
                    break;
                case WarningType.Death:
                    warningQueue.Enqueue(DeathWarning);
                    // DeathWarning();
                    break;
                case WarningType.NotEnoughMaterial:
                    warningQueue.Enqueue(NotEnoughMaterialWarning);
                    // NotEnoughMaterialWarning();
                    break;
                case WarningType.LowFuel:
                    warningQueue.Enqueue(LowFuelWarning);
                    // LowFuelWarning();
                    break;
                case WarningType.ShieldOn:
                    // ShieldTutorialOn();
                    warningQueue.Enqueue(ShieldTutorialOn);
                    break;
                case WarningType.ShieldOff:
                    // ShieldTutorialOff();
                    warningQueue.Enqueue(ShieldTutorialOff);
                    break;
                default:
                    break;
            }
            
            StartWarning();
        }


        private void StartWarning()
        {
            if (warningInPlace) return;
            if (warningQueue.Count == 0) return;
            
            var warning = warningQueue.Dequeue();
            warning?.Invoke();
            warningInPlace = true;
        }
        
        private void FuelWarning()
        {
            ShowWarning("Out of fuel");    
        }


        private void DeathWarning()
        {
            ShowWarning("You died");
        }


        private void NotEnoughMaterialWarning()
        {
            ShowWarning("Not enough material");
        }


        private void LowFuelWarning()
        {
            ShowWarning("Low Fuel");
        }


        private void ShieldTutorialOn()
        {
            ShowWarning("Shield ON");
        }
        
        private void ShieldTutorialOff()
        {
            ShowWarning("Shield OFF");
        }
        
        private void ShowWarning(string txt)
        {
            warningMessage.text = txt;
            
            warningTransform.localScale = Vector3.zero;
            warningTransform.gameObject.SetActive(true);

            warningTransform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                warningTransform.DOScale(Vector3.zero, 0.4f).SetDelay(2f).OnComplete(() =>
                {
                    warningTransform.gameObject.SetActive(false);
                    warningInPlace = false;
                    StartWarning();
                });
            });
        }
    }
}