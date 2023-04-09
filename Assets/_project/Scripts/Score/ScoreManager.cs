using System.Collections;
using _project.Scripts.Berk;
using _project.Scripts.Enemy;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.UI;
using _project.Scripts.Utils;
using UnityEngine;

namespace _project.Scripts.Score
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;

        private GameManager gameManager;
        private LevelScoreInfo levelScoreInfo;
        private InGameUI inGameUI;

        private CanvasManager canvasManager;

        private Coroutine myCoroutine;
        private WaitForSeconds waitFor;

        private float currentPoints;
        private float displaySessionPoints;
        private float driftAddPoint;
        private float frameWait;
        
        private ChapterType currentChapter;
        private int killCount;
        private int currentStarAmount;
        private int maxStarAmount;
        private int targetStar;

        private bool isActive;
        private bool drifting;
        private bool uiActive;
        private bool maxReached;

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
            SetActive(false);
        }

        public void SetActive(bool set)
        {
            gameObject.SetActive(set);

            if (!set) return;
            isActive = true;
            canvasManager = CanvasManager.Instance; // TODO do better
            inGameUI = canvasManager.GetInGameUI();
            inGameUI.EnableScoreSection();
            killCount = 0;
            UpdateStarRingScore();
            
            if(gameManager == null)
                gameManager = GameManager.Instance;
        }

        private void Update()
        {
            if(!isActive) return;
            if (!drifting) return;

            frameWait += Time.deltaTime;
            if (frameWait < 0.05f) return;
            frameWait = 0f;
            
            currentPoints += driftAddPoint;
            displaySessionPoints += driftAddPoint;
            
            UpdateScore();
        }
        
        //test
        public int GetStarAmount()
        {
            return maxReached ? maxStarAmount : currentStarAmount;
        }


        public int GetKillCount() => killCount;
        //


        public void UpdateScore()
        {
            CheckForNextStar();
            UpdateRingFill();
            UpdateLiveScoreUI();
        }

        private void CheckForNextStar()
        {
            if (maxReached) return;
            if (currentPoints < levelScoreInfo.requiredPointsForStar[currentStarAmount]) return;
            NextStar();
        }


        private void NextStar() // TODO make private
        {
            if (currentStarAmount + 1 >= maxStarAmount)
            {
                UpdateStarRingScore(1);
                maxReached = true;
                return;
            }
            currentStarAmount += 1;
            currentPoints = 0f;
            UpdateStarRingScore();
            LetEnemySpawnerKnow();
            
            NextStarReached();
        }


        public void DebugTest()
        {
            NextStar();
            UpdateRingFill();
        }


        private void NextStarReached()
        {
            DataManager.Instance.AddStarLevel();

            if (currentStarAmount >= targetStar)
            {
                LevelManager.Instance.LevelCompleted(true);
            }
            else
            {
                gameManager.ShowUnlockNewSkillPrompt();
            }
        }


        private void LetEnemySpawnerKnow()
        {
            EnemySpawnSystem.Instance.ShouldUpgradeEnemyLevel(maxReached ? maxStarAmount : currentStarAmount);
        }

        private void UpdateLiveScoreUI()
        {
            inGameUI.UpdateLiveScoreText(displaySessionPoints);
        }

        private void UpdateRingFill()
        {
            var val = currentPoints / levelScoreInfo.requiredPointsForStar[currentStarAmount];
            inGameUI.SetScoreRingFill(val);
        }

        private void UpdateStarRingScore(int add = 0)
        {
            inGameUI.UpdateStarScore(currentStarAmount + add);
        }

        private void LoadLevelScoreInfo()
        {
            levelScoreInfo = ScoreBank.GetScoreInfoForLevel(currentChapter);
        }

        public void AddExtraPoint(float scoreToAdd) // TODO use this
        {
            // _currentPoints += _levelScoreInfo.correctHit;
            currentPoints += scoreToAdd;
            killCount += 1;
            inGameUI.UpdateKillCount(killCount);

            currentPoints += scoreToAdd;
            displaySessionPoints += scoreToAdd;

            if (killCount % 50 == 0)
            {
                SkillManager.Instance.ActivatePassiveSkills();  
            }
            
            if(!drifting)
                UpdateScore();
        }

        public void RemovePoints() // TODO use this
        {
            currentPoints -= levelScoreInfo.wrongHit;
        }

        public void GetReadyForLevel(ChapterType chapterType, int targetStarCount)
        {
            waitFor = new WaitForSeconds(1f);
            currentChapter = chapterType;
            targetStar = targetStarCount;
            currentPoints = 0f;
            currentStarAmount = 0;
            maxReached = false;
            
            CloseUI();
            LoadLevelScoreInfo();

            driftAddPoint = levelScoreInfo.driftPointsPerFrame;
            maxStarAmount = levelScoreInfo.requiredPointsForStar.Count;
        }
        
        public void SetDrifting(bool set)
        {
            if (set)
            {
                if (myCoroutine != null)
                {
                    StopCoroutine(myCoroutine);
                }
                OpenUI();
                drifting = true;
            }
            else
            {
                myCoroutine = StartCoroutine(LateClose());
                drifting = false;
            }
        }
        
        private void OpenUI()
        {
            if (uiActive) return;

            uiActive = true;
            inGameUI.EnableLiveScoreText();
        }

        private void CloseUI()
        {
            if (!uiActive) return;
            
            uiActive = false;
            inGameUI.DisableLiveScoreText();
        }
        
        private IEnumerator LateClose()
        {
            yield return waitFor;
            CloseUI();
            ResetData();
        }

        private void ResetData()
        {
            displaySessionPoints = 0f;
        }

        public void GameOver()
        {
            isActive = false;
            drifting = false;
            maxReached = false;

            displaySessionPoints = 0;
            currentPoints = 0;
            currentStarAmount = 0;
            killCount = 0;
            
            if (myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
            }

            uiActive = false;
        }


        public void ResetScores()
        {
            inGameUI.ResetScoreUI();
        }
    }
}