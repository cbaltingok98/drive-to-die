using System;
using _project.Scripts.AI;
using _project.Scripts.Berk;
using _project.Scripts.Enemy;
using _project.Scripts.Enums;
using _project.Scripts.Input;
using _project.Scripts.LevelInfo;
using _project.Scripts.Models;
using _project.Scripts.Pools;
using _project.Scripts.Score;
using _project.Scripts.UI;
using _project.Scripts.Utils;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public Light mainLight;
        
        [SerializeField] private GameObject playerCarSystem;
        [SerializeField] private JoystickController joystickController;

        [Header("DEBUG")] 
        [Space(10)] 
        public bool saveDrivingInput; // TODO remove
        public bool deployAI; // TODO remove
        [Range(0, 5)] public int savePosIndex; // TODO remove

        private DataManager dataManager;
        private LevelManager levelManager;
        private ScoreManager scoreManager;
        private CanvasManager canvasManager;
        private AiSystemManager aiSystemManager;
        private EnemySpawnSystem enemySpawnSystem;
        private SkillManager skillManager;
        private ObjectPooler objectPooler;
        private CanvasElementPooler canvasElementPooler;
        private ParticlePooler particlePooler;

        private MyCustomLevel myCustomLevel;

        private LevelFinishData levelFinishData;

        public GameState GameState { get; private set; }
        public GameState PreviousGameState { get; private set; }
        


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
            
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }


        private void Start()
        {
            DOTween.Init();

            dataManager = DataManager.Instance;
            levelManager = LevelManager.Instance;
            scoreManager = ScoreManager.Instance;
            canvasManager = CanvasManager.Instance;
            aiSystemManager = AiSystemManager.Instance;
            enemySpawnSystem = EnemySpawnSystem.Instance;
            skillManager = SkillManager.Instance;

            particlePooler = ParticlePooler.Instance;
            canvasElementPooler = CanvasElementPooler.Instance;
            objectPooler = ObjectPooler.Instance;

            particlePooler.Init();
            canvasElementPooler.Init();
            objectPooler.Init();
            SoundManager.Instance.Init();

            canvasManager.FadeIn();

            if (!dataManager.IsPlayedBefore())
            {
                Printer.Print("First Time Playing");
                
                // _dataManager.PlayedBefore();// do after tutorial completed and free car unlocked
                // StartLevel();
                // return;
            }

            levelManager.LoadShowroom();
            canvasManager.ShowMenuUI();
        }


        public void StartLevel()
        {
            LoadNextLevel();
        }

        public void CarSystemLoaded()
        {
            enemySpawnSystem.Init(myCustomLevel.GetChapterInformation(), levelManager.GetPlayerTransform(),
                myCustomLevel.transform);
            myCustomLevel.Init(enemySpawnSystem.transform);
        }


        public GameObject GetCarSystem() => playerCarSystem;
        public AiSystemManager GetAiSystemManager() => aiSystemManager;


        private void LoadNextLevel()
        {
            var level = dataManager.LoadLevel();
            levelManager.LoadLevel(level, false);
        }

        public void SetCustomLevel(MyCustomLevel myCustomLevel)
        {
            this.myCustomLevel = myCustomLevel;
        }


        public void SetSkillSystem()
        {
            skillManager.Init();
        }


        public void ShowUnlockNewSkillPrompt()
        {
            canvasManager.ShowSkillPrompt(skillManager.GetSkillsForPrompt(), FreezeTime);
        }


        public void CloseSkillUnlockPrompt()
        {
            canvasManager.HideSkillPrompt(ContinueTime);
        }


        private void FreezeTime()
        {
            Time.timeScale = 0;
            levelManager.GetPlayerController().DeActivate();
        }


        private void ContinueTime()
        {
            Time.timeScale = 1;
            levelManager.GetPlayerController().Activate();
        }


        private void ResetTime()
        {
            Time.timeScale = 1;
        }


        public void PauseGame()
        {
            FreezeTime();
        }


        public void ContinueGame()
        {
            ContinueTime();
        }


        public void SetScoreInfo()
        {
            scoreManager.GetReadyForLevel(levelManager.ChapterType, myCustomLevel.GetTargetStar());
        }


        public LevelFinishData GetLevelFinishData() => levelFinishData;

        private LevelFinishData SaveLevelFinishData()
        {
            LevelFinishData newData = new LevelFinishData(
                levelManager.ChapterType,
                (int)PlayTimeCounterManager.Instance.SecondsPlayed,
                scoreManager.GetStarAmount(),
                scoreManager.GetKillCount(),
                CalculateCoinEarn()
                );

            return newData;
        }


        private int CalculateCoinEarn()
        {
            var killCount = scoreManager.GetKillCount() * 0.1;
            var aliveSeconds = (int)PlayTimeCounterManager.Instance.SecondsPlayed;

            var coinToEarn = killCount * aliveSeconds;
            
            DataManager.Instance.AddCoin((int)coinToEarn);

            return (int)coinToEarn;
        }


        private void UnlockNextChapterAfter(ChapterType chapterType)
        {
            var maxChapterNum = Enum.GetValues(typeof(ChapterType)).Length;

            var curChapterIndex = (int)chapterType;

            if (curChapterIndex + 1 >= maxChapterNum) return;
            if (LevelObjectManager.IsComingSoon((ChapterType)curChapterIndex+1)) return;
            
            DataManager.Instance.UnlockChapter((ChapterType)curChapterIndex+1);
        }


        public void GameOver(bool isWin, ChapterType chapterType)
        {

            if (isWin)
            {
                UnlockNextChapterAfter(chapterType);    
            }
            
            levelFinishData = SaveLevelFinishData();
            
            joystickController.ResetJoystick();
            enemySpawnSystem.GameOver();
            
            levelManager.ResetCollectableSpawners();

            particlePooler.Reset();
            canvasElementPooler.Reset();
            objectPooler.Reset();
            
            canvasManager.GetInGameUI().GetSkillPromptUi().KillTween();
            canvasManager.GetInGameUI().CloseSkillPromptUi();
            canvasManager.GetInGameUI().ControlPauseButton(false);
            ResetTime();
            
            skillManager.GameOver();
            scoreManager.GameOver();
            
            FuelManager.Instance.Reset();

            if (isWin)
            {
                levelManager.GetPlayerController().BlockPlayerInput();
                particlePooler.SpawnFromPool(SpawnType.CollectableHealParticle, levelManager.GetPlayerTransform().position,
                    Quaternion.identity, false).Play();   
            }
            else
            {
                levelManager.DisableCarSystem();
                particlePooler.SpawnFromPool(SpawnType.RocketLauncherBlast, levelManager.GetPlayerTransform().position,
                    Quaternion.identity, false).Play();
            }
        }


        public void ScreenIsBlack()
        {
            enemySpawnSystem.ResetEnemies();
            scoreManager.ResetScores();   
        }


        public string GetLevelName() => myCustomLevel.name; // for mimic data
        public int GetGridIndex() => savePosIndex;
        
        public bool IsPlaying()
        {
            return GameState == GameState.Playing;
        }
        
        public bool IsGameStateAvailableForGdpr()
        {
            return !IsPlaying();
        }


        public bool IsDebug()
        {
            return Constants.IsDebugOn;
        }


        public void AdStartsPlaying()
        {
            
        }


        public void AdFailedToPlay()
        {
            
        }


        public void AdCompletedPlaying()
        {
            
        }


        public void UserGdprConsentChanged(bool result)
        {
            
        }
    }
}