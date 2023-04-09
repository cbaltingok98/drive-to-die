using System;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using UnityEngine;

namespace _project.Scripts.Car
{
    public class CarGarage : MonoBehaviour
    {
        public static CarGarage Instance;
        
        [SerializeField] private List<CarProfile> carProfileList;

        #region Singleton
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
        #endregion

        public CarProfile GetModel(CarModels model, bool aiPass = false)
        {
            if (carProfileList.Count == 0)
            {
                Printer.Print(model + " : Not Available", DesiredColor.Orange);
                return null;
            }
            
            // TODO check if car is unlocked first
            //if (!aiPass && !DataManager.Instance.PlayerData.IsCarUnlocked(model)) return null;

            var car = carProfileList.First(car => car.GetModel() == model);
            return car;
        }
    }
}