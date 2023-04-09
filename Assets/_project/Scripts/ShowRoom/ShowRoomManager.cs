using System.Numerics;
using _project.Scripts.Berk;
using _project.Scripts.Car;
using _project.Scripts.Data;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Skills;
using _project.Scripts.Skills.Skill_Logic;
using UnityEngine;
using CharacterController = _project.Scripts.Character.CharacterController;
using Vector3 = UnityEngine.Vector3;

namespace _project.Scripts.ShowRoom
{
    public class ShowRoomManager : MonoBehaviour
    {
        public static ShowRoomManager Instance;
        public Light showRoomBackgroundLight;
        public Light showRoomCarLight;
        [SerializeField] private Transform carShowRoomTransform;
        [SerializeField] private Transform skillShowRoomTransform;
        [SerializeField] private Transform characterShowRoomTransform;
        [SerializeField] private Transform battlePageCameraRef;
        [SerializeField] private Transform garagePageCameraRef;
        [SerializeField] private Transform armoryPageCameraRef;

        private ShowRoomCamera showRoomCamera;

        private SkillSystemHelper skillSystemHelper;

        private CarProfile currentCarOnShowroom;
        private Skill currentSkillOnDisplay;

        private Transform currentCharacterOnShowroom;
        
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

        public void EnableShowRoom()
        {
            GameManager.Instance.mainLight.gameObject.SetActive(false);
            showRoomBackgroundLight.gameObject.SetActive(true);
            showRoomCarLight.gameObject.SetActive(true);
            
            if (showRoomCamera == null)
            {
                showRoomCamera = ShowRoomCamera.Instance;   
            }

            if (skillSystemHelper == null)
            {
                skillSystemHelper = SkillSystemHelper.Instance;
            }

            gameObject.SetActive(true);
            SpawnCurrentCar(DataManager.Instance.GetCurrentCarModel());
            SpawnCurrentCharacter(DataManager.Instance.GetCurrentCharacter());
        }
        
        public void DisableShowRoom()
        {
            showRoomBackgroundLight.gameObject.SetActive(false);
            showRoomCarLight.gameObject.SetActive(false);
            GameManager.Instance.mainLight.gameObject.SetActive(true);
            PutCharacterBack();
            gameObject.SetActive(false);
        }


        public CarModels GetCurrentCarModelOnShowRoom()
        {
            return currentCarOnShowroom.GetModel();
        }

        public void SwitchShowRoomCar(CarModels carModel)
        {
            SpawnCurrentCar(carModel);
        }


        #region Avatar
        
        
        private void SpawnCurrentCharacter(CharacterType characterType)
        {
            if (currentCharacterOnShowroom != null)
            {
                PutCharacterBack();
            }

            currentCharacterOnShowroom = CharacterController.Instance.GetCharacter(characterType);
            
            if (currentCharacterOnShowroom == null)
            {
                Printer.Print("Character is not found or not unlocked", DesiredColor.Red);
                return;
            }
            
            currentCharacterOnShowroom.SetPositionAndRotation(characterShowRoomTransform.position, characterShowRoomTransform.rotation);
            currentCharacterOnShowroom.gameObject.SetActive(true);
        }


        private void PutCharacterBack()
        {
            if (currentCharacterOnShowroom == null) return;
            
            currentCharacterOnShowroom.SetParent(CharacterController.Instance.transform);
            currentCharacterOnShowroom.gameObject.SetActive(false);
            currentCharacterOnShowroom = null;
        }
        
        #endregion


        #region Car

        private void SpawnCurrentCar(CarModels modelToSpawn)
        {
            // TODO check through dataManager
            //
            // if (_currentCarOnShowroom != null && modelToSpawn == _currentCarOnShowroom.GetModel())
            // {
            //     Printer.Print("Returing", DesiredColor.Orange);
            //     return;
            // }

            if (currentCarOnShowroom != null)
            {
                PutCarBackToGarage();
            }
            
            currentCarOnShowroom = CarGarage.Instance.GetModel(modelToSpawn);
            currentCarOnShowroom.ControlFakeShadow(true);
            
            if (currentCarOnShowroom == null)
            {
                Printer.Print("Car is not found or not unlocked", DesiredColor.Red);
                return;
            }
            Transform carToShowTransform = currentCarOnShowroom.transform;

            carToShowTransform.SetPositionAndRotation(carShowRoomTransform.position, carShowRoomTransform.rotation);
            // carToShowTransform.position = carShowRoomTransform.position;
            // carToShowTransform.rotation = carShowRoomTransform.rotation;
            
            carToShowTransform.gameObject.SetActive(true);
        }


        public void PutCarBackToGarage()
        {
            if (currentCarOnShowroom == null) return;
            
            currentCarOnShowroom.gameObject.SetActive(false);
            var targetTransform = CarGarage.Instance.transform;
            currentCarOnShowroom.transform.SetParent(targetTransform);
            currentCarOnShowroom.transform.localPosition = Vector3.zero;
            currentCarOnShowroom.transform.localEulerAngles = Vector3.zero;
            currentCarOnShowroom.ControlFakeShadow(false);

            currentCarOnShowroom = null;
        }
        
        #endregion


        #region Skill

        
        public void SwitchShowroomSkill(SkillType skillType)
        {
            SpawnSkill(skillType);
        }

        private void SpawnSkill(SkillType skillType)
        {
            if (currentSkillOnDisplay != null)
            {
                PutSkillBack();
            }

            currentSkillOnDisplay = SkillSystemHelper.Instance.GetSkill(skillType);
            if (currentSkillOnDisplay == null)
            {
                Printer.Print("Skill is not found or unlocked", DesiredColor.Red);
            }

            Transform skillToShowTransform = currentSkillOnDisplay.transform;

            skillToShowTransform.SetPositionAndRotation(skillShowRoomTransform.position, skillShowRoomTransform.rotation);
            // skillToShowTransform.position = skillShowRoomTransform.position;
            // skillToShowTransform.rotation = skillShowRoomTransform.rotation;

            currentSkillOnDisplay.ShowArmoryModel();
        }


        private void PutSkillBack()
        {
            if (currentSkillOnDisplay == null) return;
            
            currentSkillOnDisplay.HideArmoryModel();
            
            var targetTransform = SkillSystemHelper.Instance.transform;
            currentSkillOnDisplay.transform.SetParent(targetTransform);
            currentSkillOnDisplay.transform.localPosition = Vector3.zero;
            currentSkillOnDisplay.transform.localEulerAngles = Vector3.zero;

            currentSkillOnDisplay = null;
        }
        
        #endregion


        #region ArmoryEnableDisable
        public void ArmoryMenuEnabled()
        {
            GoToArmoryCameraAngle();
            PutCarBackToGarage();
            SpawnSkill(SkillSystemHelper.Instance.GetFirstAvailableSkill());
        }


        public void ArmoryMenuDisabled()
        {
            PutSkillBack();
        }
        #endregion
        
        
        #region BaseEnableDisable

        public void BaseMenuEnabled()
        {
            GoToBattleCameraAngle();
            SpawnCurrentCar(DataManager.Instance.GetCurrentCarModel());
        }


        public void BaseMenuDisabled()
        {
            
        }
        

        #endregion
        
        
        #region GarageEnableDisable

        public void GarageMenuEnabled()
        {
            GoToGarageCameraAngle();
            SpawnCurrentCar(DataManager.Instance.GetCurrentCarModel());
        }


        public void GarageMenuDisabled()
        {
            var currentModel = currentCarOnShowroom.GetModel();
            if (currentModel != DataManager.Instance.PlayerData.currentCarModel && 
                DataManager.Instance.IsCarUnlocked(currentModel))
            {
                DataManager.Instance.PlayerData.currentCarModel = currentModel;
                SaveLoadSystem.Save(DataManager.Instance.PlayerData);
            }
        }

        #endregion


        public void GoToBattleCameraAngle()
        {
            showRoomCamera.MoveToPosition(battlePageCameraRef, 60);
        }


        public void GoToGarageCameraAngle()
        {
            showRoomCamera.MoveToPosition(garagePageCameraRef, 65);
        }


        public void GoToArmoryCameraAngle()
        {
            showRoomCamera.MoveToPosition(armoryPageCameraRef, 65);
        }

    }
}