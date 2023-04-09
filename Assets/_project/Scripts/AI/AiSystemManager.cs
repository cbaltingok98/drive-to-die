using System;
using System.Collections.Generic;
using _project.Scripts.Car;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _project.Scripts.AI
{
    public class AiSystemManager : MonoBehaviour
    {
        public static AiSystemManager Instance;
        
        [SerializeField] private GameObject aiCarSystem;

        private CarGarage carGarage;

        private List<AiController> activeAi;

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

        private void Init()
        {
            if (carGarage != null) return;
            carGarage = CarGarage.Instance;
        }

        public void InitializeAiPlayers(List<InLevelAiInfo> aiInformation)
        {
            Init();
            SpawnCars(aiInformation);
        }
        
        private void SpawnCars(List<InLevelAiInfo> aiInformation)
        {
            activeAi = new List<AiController>();
            for (int i = 0; i < aiInformation.Count; i++)
            {
                if (!aiInformation[i].active) continue;
                
                GameObject newAiControllerGameObject =
                    Instantiate(aiCarSystem, aiInformation[i].gridPos.position, aiInformation[i].gridPos.rotation);
                
                CarProfile carProfile = carGarage.GetModel(aiInformation[i].carModel, true);
                
                GameObject newCarObject = Instantiate(carProfile.gameObject, aiInformation[i].gridPos.position, aiInformation[i].gridPos.rotation);
            
                newAiControllerGameObject.GetComponent<CarMarriage>().MarryMe(newCarObject.GetComponent<CarProfile>());
                
                AiController aiController = newAiControllerGameObject.GetComponent<AiController>();
                
                aiController.SetDrivingProfile(DrivingProfileManager.GetDrivingProfile(aiInformation[i].drivingProfile));
                aiController.SetMimicData(i); // TODO do better, pass difficulty setting with grid pos
                
                activeAi.Add(aiController);
            }
        }

        public void RaceStarted()
        {
            if (activeAi == null) return;
            
            foreach (var activeAi in activeAi) 
            {
                activeAi.Activate();
            }
        }

        private CarModels GetRandomCarModel()
        {
            var maxNumOfModels = Enum.GetValues(typeof(CarModels)).Length;
            var randPick = Random.Range(0, maxNumOfModels);

            return (CarModels)randPick;
        }
    }
}