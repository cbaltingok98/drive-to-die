using System;
using System.Collections.Generic;
using _project.Scripts.Car;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _project.Scripts.UI.Menu.GarageMenu
{
    public class DrivingSpecsInfoController : MonoBehaviour
    {
        [SerializeField] private Transform contentParent;
        [SerializeField] private TextMeshProUGUI title;

        [SerializeField] private List<SpecItem> specItems;
        

        public void Init(CarModels carModel)
        {
            //Reset();
            //_specItems = new List<SpecItem>();
            
            DrivingProfileType drivingProfileType = CarGarage.Instance.GetModel(carModel).GetDrivingProfileType();
            DrivingProfile drivingProfile = DrivingProfileManager.GetDrivingProfile(drivingProfileType);

            var level = drivingProfile.level-1;
            var currentStats = drivingProfile.drivingStatsList[level];
            DrivingStats nextStats = null;

            if ((level + 1) < drivingProfile.drivingStatsList.Count)
            {
                nextStats = drivingProfile.drivingStatsList[level + 1];
            }

            title.text = drivingProfile.displayName;
            ParseDrivingInfo(currentStats, nextStats);
        }


        private void ParseDrivingInfo(DrivingStats drivingStats, DrivingStats nextLevelData)
        {
            //var scrollViewInfo = GetNewScrollItem();
            var specItem = specItems[0];
            
            var newData = new InfoData();
            newData.BackgroundIndex = 0;
            newData.InfoText = "Health";
            newData.CurrentValue = GetBarCountForHealth(drivingStats.health);
            newData.IsMaxLevel = nextLevelData == null;

            if (nextLevelData != null)
            {
                newData.NextValue = GetBarCountForHealth(nextLevelData.health);
            }
            
            specItem.Init(newData);

            specItem = specItems[1];
            
            newData = new InfoData();
            newData.BackgroundIndex = 1;
            newData.InfoText = "Acceleration";
            newData.CurrentValue = GetBarCountForAcceleration(drivingStats.acceleration);
            newData.IsMaxLevel = nextLevelData == null;

            if (nextLevelData != null)
            {
                newData.NextValue = GetBarCountForAcceleration(nextLevelData.acceleration);
            }
            
            specItem.Init(newData);
            
            specItem = specItems[2];
           
            newData = new InfoData();
            newData.BackgroundIndex = 2;
            newData.InfoText = "Max Speed";
            newData.CurrentValue = GetBarCountForMaxSpeed(drivingStats.maxSpeed);
            newData.IsMaxLevel = nextLevelData == null;

            if (nextLevelData != null)
            {
                newData.NextValue = GetBarCountForMaxSpeed(nextLevelData.maxSpeed);
            }
            
            specItem.Init(newData);
        }


        private int GetBarCountForHealth(float value)
        {
            if (value <= 100)
            {
                return 0; // 1 bar
            }
            else if (value <= 140)
            {
                return 1;
            }
            else if (value <= 180)
            {
                return 2;
            }
            else if (value <= 200)
            {
                return 3;
            }
            else
            {
                return 4; // 5 bars
            }
        }
        
        private int GetBarCountForMaxSpeed(float value)
        {
            if (value <= 34f)
            {
                return 0;
            }
            else if (value <= 34.5f)
            {
                return 1;
            }
            else if (value <= 35f)
            {
                return 2;
            }
            else if (value <= 35.5f)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        
        private int GetBarCountForAcceleration(float value)
        {
            if (value <= 34.5f)
            {
                return 0;
            }
            else if (value <= 35f)
            {
                return 1;
            }
            else if (value <= 35.5f)
            {
                return 2;
            }
            else if (value <= 36f)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }


        private SpecItem GetNewSpecItem()
        {
            var newView = CanvasElementPooler.Instance.SpawnScrollViewInfoItemFromPool(UiElementType.SpecItem,
                contentParent);
            specItems.Add(newView);
            return newView;
        }


        public void Reset()
        {
            if (specItems == null) return;
            
            for (var i = 0; i < specItems.Count; i++)
            {
                specItems[i].gameObject.SetActive(false);
                specItems[i].transform.SetParent(CanvasElementPooler.Instance.transform);
            }
        }
    }
}