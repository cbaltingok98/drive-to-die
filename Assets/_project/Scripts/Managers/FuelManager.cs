using System;
using System.Runtime.InteropServices.WindowsRuntime;
using _project.Scripts.Berk;
using _project.Scripts.Car.DrivingMechanic;
using _project.Scripts.UI;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class FuelManager : MonoBehaviour
    {
        public static FuelManager Instance;
        
        private Car.DrivingMechanic.Car car;
        private InGameUI inGameUI;

        private Tween myTween;
        
        private float maxFuel;
        private float currentFuelAmount;
        private float lowFuelPercentage = 30f;

        private bool consume;
        private bool block;
        private bool showLowFuelWarning;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public void Init(Car.DrivingMechanic.Car car)
        {
            showLowFuelWarning = false;
            block = false;
            SetConsumption(false);
            inGameUI = CanvasManager.Instance.GetInGameUI();
            
            this.car = car;
            UpdateMaxFuel(this.car.fuel);
            currentFuelAmount = maxFuel;
            UpdateFuelBar();
        }


        public float GetMaxFuel() => maxFuel;
        
        
        public void UpdateMaxFuel(float maxFuel)
        {
            this.maxFuel = maxFuel;
        }

        public void SetConsumption(bool set)
        {
            consume = set;
        }


        public void AddFuel(float amount)
        {
            block = true;
            var newFuel = currentFuelAmount + amount;
            newFuel = newFuel > maxFuel ? maxFuel : newFuel;

            myTween = DOTween.To(() => currentFuelAmount, x => currentFuelAmount = x, newFuel, 0.4f).OnUpdate(() =>
            {
                UpdateFuelBar();
            }).OnComplete(() =>
            {
                block = false;
            });

            if (currentFuelAmount / maxFuel * 100f > lowFuelPercentage)
            {
                showLowFuelWarning = false;
            }
        }


        private void Update()
        {
            if (!consume) return;
            if (block) return;

            currentFuelAmount -= car.fuelConsumption;
            currentFuelAmount = currentFuelAmount < 0 ? 0f : currentFuelAmount;
            UpdateFuelBar();
            CheckForGameOver();
        }


        private void FixedUpdate()
        {
            if (!consume) return;
            if (block) return;
            
            if (showLowFuelWarning) return;
            if (currentFuelAmount/maxFuel*100f > lowFuelPercentage) return; // when fuel level is below %30, show warning

            showLowFuelWarning = true;
            CanvasManager.Instance.GetInGameUI().ShowWarningMessage(WarningType.LowFuel);
        }


        private void CheckForGameOver()
        {
            if (currentFuelAmount > 0) return;
            
            LevelManager.Instance.LevelCompleted(false);
            CanvasManager.Instance.GetInGameUI().ShowWarningMessage(WarningType.Fuel);
        }

        private void UpdateFuelBar()
        {
            var val = currentFuelAmount / maxFuel;
            
            inGameUI.UpdateFuelBar(val);
        }


        public void Reset()
        {
            myTween?.Kill();
            SetConsumption(false);
            block = false;
        }
    }
}