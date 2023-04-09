using System.Collections;
using System.Collections.Generic;
using _project.Scripts.AI;
using _project.Scripts.Berk;
using _project.Scripts.Car;
using _project.Scripts.Car.DrivingMechanic;
using _project.Scripts.Collectables;
using _project.Scripts.Enemy;
using _project.Scripts.Enums;
using _project.Scripts.LevelInfo;
using _project.Scripts.Player;
using _project.Scripts.ShowRoom;
using _project.Scripts.Skills;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        private CanvasManager canvasManager;
        private CameraManager cameraManager;

        private GameManager gameManager;
        private GameObject createdLevel;

        private MyCustomLevel myCustomLevel;
        private PlayerController playerController;
        private ShowRoomCamera showRoomCamera;
        private ShowRoomManager showRoomManager;

        private GameObject activePlayerObject;

        private List<CollectableSpawner> collectableSpawners;

        public ChapterType ChapterType { get; private set; }

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

        private void Start()
        {
            gameManager = GameManager.Instance;
            canvasManager = CanvasManager.Instance;
            cameraManager = CameraManager.Instance;
            showRoomCamera = ShowRoomCamera.Instance;
            showRoomManager = ShowRoomManager.Instance;
            
            StopPlayTimeCounter();
        }
        
        private void Action()
        {
            // _gameManager.InitializeLevelSystems();
            InitializeLevelSystems();
        }


        private void InitializeLevelSystems()
        {
            RenderSettings.fog = false;
            canvasManager.HideMenuUI();
            canvasManager.ShowInGameUI();

            showRoomManager.DisableShowRoom();
            showRoomCamera.ControlCamera(false);
            cameraManager.ControlInLevelCamera(true);

            gameManager.SetScoreInfo();

            //LoadAiSystemAndCars();
            LoadCarSystem();
        }

        private void LevelLoadedAndVisible()
        {
            canvasManager.GetCountDownUI().CountDownFrom3(CounterFinishedStartRacing);
        }

        private void CounterFinishedStartRacing()
        {
            if (!DataManager.Instance.IsPlayedBefore())
            {
                DataManager.Instance.PlayedBefore();
            }
            
            canvasManager.GetInGameUI().ShowPauseMenu();
            playerController.Activate();
            EnemySpawnSystem.Instance.Activate();
            StartPlayTimeCounter();
        }


        private void StartPlayTimeCounter()
        {
            PlayTimeCounterManager.Instance.ControlCounter(true);
        }


        private void StopPlayTimeCounter()
        {
            PlayTimeCounterManager.Instance.ControlCounter(false);
        }

        public void LoadLevel(Level levelObject, bool wait)
        {
            Printer.Print("level id " + levelObject.id);
            
            collectableSpawners = new List<CollectableSpawner>();
            
            ChapterType = levelObject.id;
            
            if (wait)
            {
                StartCoroutine(LateSpawnLevel(levelObject));
            }
            else
            {
                SpawnLevel(levelObject);
            }
        }
        
        private IEnumerator LateSpawnLevel(Level newLevel)
        {
            if (createdLevel != null)
            {
                Destroy(createdLevel);
            }
            
            yield return new WaitForSeconds(1f);
            

            createdLevel = Instantiate(newLevel.levelPrefab, Vector3.zero, Quaternion.identity);

            SendCustomLevel(newLevel);
        }

        private void SpawnLevel(Level newLevel)
        {
            if (createdLevel != null)
            {
                Destroy(createdLevel);
            }

            createdLevel = Instantiate(newLevel.levelPrefab, Vector3.zero, Quaternion.identity);

            SendCustomLevel(newLevel);
        }


        public void LoadShowroom()
        {
            showRoomManager.EnableShowRoom();
        }

        private void SendCustomLevel(Level level)
        {
            myCustomLevel = createdLevel.GetComponent<MyCustomLevel>();
            myCustomLevel.SetChapterTarget(level.chapterTarget);
            
            gameManager.SetCustomLevel(myCustomLevel);
            
            canvasManager.FadeOutAndFadeIn(Action, LevelLoadedAndVisible, myCustomLevel.GetLevelTheme());
        }
        
        public void LoadCarSystem()
        {
            if (activePlayerObject)
            {
                activePlayerObject.transform.position = myCustomLevel.GetCarSpawnTransform().position;
                activePlayerObject.transform.rotation = myCustomLevel.GetCarSpawnTransform().rotation;
            }
            else
            {
                activePlayerObject = Instantiate(gameManager.GetCarSystem(), myCustomLevel.GetCarSpawnTransform().position, myCustomLevel.GetCarSpawnTransform().rotation);// TODO uncomment when above code is commented    
            }
            
            gameManager.SetSkillSystem();
            
            CarMarriage carMarriage = activePlayerObject.GetComponent<CarMarriage>();
            CarModels currentCarModel = DataManager.Instance.PlayerData.currentCarModel;

            carMarriage.MarryMe(currentCarModel);

            CarProfile carProfile = CarGarage.Instance.GetModel(currentCarModel);
            DrivingProfileType drivingProfileType = carProfile.GetDrivingProfileType();
            DrivingProfile drivingProfile = DrivingProfileManager.GetDrivingProfile(drivingProfileType);
            
            carProfile.ControlFakeShadow(false);
            
            playerController = activePlayerObject.GetComponent<PlayerController>();
            playerController.SetDrivingProfile(drivingProfile);
            playerController.SetMimicSystem(gameManager.saveDrivingInput); // TODO remove
            playerController.Init();
            
            cameraManager.SetPlayer(activePlayerObject.transform); // TODO do better
            
            // _gameManager.myDebugUI.Init(_activePlayerObject);  // TODO remove
            // _gameManager.directionIndicatorToggleBtn.onClick.AddListener(_activePlayerObject.GetComponent<CarController>().ControlDirectionIndicator); // TODO remove
            
            gameManager.CarSystemLoaded();
        }
        
        // public void LoadCarSystem()
        // {
        //     // TODO COMMENT THIS SECTION
        //     
        //     // if (_gameManager.saveDrivingInput)
        //     // {
        //     //     _activePlayerObject = Instantiate(_gameManager.GetCarSystem(), _myCustomLevel.GetAiInformation()[_gameManager.savePosIndex].gridPos.position, _myCustomLevel.GetAiInformation()[_gameManager.savePosIndex].gridPos.rotation);
        //     // }
        //     // else
        //     // {
        //     //     if (_activePlayerObject)
        //     //     {
        //     //         _activePlayerObject.transform.position = _myCustomLevel.GetCarSpawnTransform().position;
        //     //         _activePlayerObject.transform.rotation = _myCustomLevel.GetCarSpawnTransform().rotation;
        //     //         Printer.Print("Player exists");
        //     //     }
        //     //     else
        //     //     {
        //     //         _activePlayerObject = Instantiate(_gameManager.GetCarSystem(), _myCustomLevel.GetCarSpawnTransform().position, _myCustomLevel.GetCarSpawnTransform().rotation);
        //     //         Printer.Print("New Player Created " + _activePlayerObject);
        //     //     }
        //     // }
        //     ////////// until here
        //     if (_activePlayerObject)
        //     {
        //         _activePlayerObject.transform.position = _myCustomLevel.GetCarSpawnTransform().position;
        //         _activePlayerObject.transform.rotation = _myCustomLevel.GetCarSpawnTransform().rotation;
        //     }
        //     else
        //     {
        //         _activePlayerObject = Instantiate(_gameManager.GetCarSystem(), _myCustomLevel.GetCarSpawnTransform().position, _myCustomLevel.GetCarSpawnTransform().rotation);// TODO uncomment when above code is commented    
        //     }
        //     
        //     CarModels currentCarModel = DataManager.Instance.PlayerData.currentCarModel;
        //
        //     SkillSystemHelper skillSystemHelper = _activePlayerObject.GetComponent<SkillSystemHelper>();
        //     CarMarriage carMarriage = _activePlayerObject.GetComponent<CarMarriage>();
        //     
        //     _gameManager.SetSkillSystem(skillSystemHelper);
        //     carMarriage.MarryMe(currentCarModel, skillSystemHelper);
        //     
        //     _playerController = _activePlayerObject.GetComponent<PlayerController>();
        //
        //     // if (_gameManager.saveDrivingInput)
        //     // {
        //     //     InLevelAiInfo aiInfo = _myCustomLevel.GetAiInformation()[_gameManager.savePosIndex];
        //     //     _playerController.SetDrivingProfile(DrivingProfileManager.GetDrivingProfile(aiInfo.drivingProfile));   
        //     // }
        //     // else
        //     // {
        //     //     _playerController.SetDrivingProfile(DrivingProfileManager.GetDrivingProfile(CarGarage.Instance.GetModel(currentCarModel).GetDrivingProfile()));   
        //     // }
        //     
        //     _playerController.SetDrivingProfile(DrivingProfileManager.GetDrivingProfile(CarGarage.Instance.GetModel(currentCarModel).GetDrivingProfile()));
        //     _playerController.SetMimicSystem(_gameManager.saveDrivingInput); // TODO remove
        //     _playerController.Init();
        //
        //     Camera.main.GetComponent<CameraManager>().SetPlayer(_activePlayerObject.transform); // TODO do better
        //     
        //     _gameManager.myDebugUI.Init(_activePlayerObject);  // TODO remove
        //     _gameManager.directionIndicatorToggleBtn.onClick.AddListener(_activePlayerObject.GetComponent<CarController>().ControlDirectionIndicator); // TODO remove
        //     
        //     _gameManager.CarSystemLoaded();
        // }
        
        public void DisableCarSystem()
        {
            playerController.ResetPlayer();
            playerController.DisableHealthBar();
            activePlayerObject.transform.SetParent(null);
            activePlayerObject.GetComponent<CarMarriage>().Divorce();
        }


        public void LevelCompleted(bool isWin)
        {
            DataManager.Instance.PlayedBefore();
            StopPlayTimeCounter();
            gameManager.GameOver(isWin, ChapterType);
            StartCoroutine(WaitBeforeLevelEndUI(isWin));
        }


        public void ReturnHomeFromPauseMenu()
        {
            DataManager.Instance.PlayedBefore();
            StopPlayTimeCounter();
            gameManager.GameOver(false, ChapterType);
            canvasManager.ReturnHomeFromPauseMenu();
        }


        private IEnumerator WaitBeforeLevelEndUI(bool isWin)
        {
            yield return new WaitForSeconds(2f);
            if (isWin)
            {
                canvasManager.LevelCompleted();
            }
            else
            {
                canvasManager.LevelFailed();
            }
        }


        public void ReturnToMainMenu()
        {
            gameManager.ScreenIsBlack();
            
            RenderSettings.fog = true;
            canvasManager.HideInGameUI(); // TODO move to level manager
            canvasManager.ShowMenuUI(); // TODO move to level manager
            CanvasManager.Instance.GetMenuUi().SwitchMenu(BottomMenuType.Home, BottomMenuType.Home);

            cameraManager.ControlInLevelCamera(false);
            showRoomManager.EnableShowRoom();
            showRoomCamera.ControlCamera(true);
            
            myCustomLevel.gameObject.SetActive(false);
            Destroy(myCustomLevel);
        }


        public LevelThemes GetLevelTheme() => myCustomLevel.GetLevelTheme();


        public void AddCollectableSpawner(CollectableSpawner newSpawner)
        {
            collectableSpawners.Add(newSpawner);
        }

        public void ResetCollectableSpawners()
        {
            for (var i = 0; i < collectableSpawners.Count; i++)
            {
                collectableSpawners[i].Reset();
            }
            collectableSpawners.Clear();
        }

        public void LoadAiSystemAndCars() // not in use
        {
            if (!gameManager.deployAI) return;
           // _gameManager.GetAiSystemManager().InitializeAiPlayers(_myCustomLevel.GetAiInformation());
        }

        public Transform GetPlayerTransform() => activePlayerObject.transform;
        public PlayerController GetPlayerController() => playerController;
    }
}