using System.Collections.Generic;
using _project.Scripts.Enemy.Enemies;
using _project.Scripts.Enums;
using _project.Scripts.LevelInfo;
using _project.Scripts.Managers;
using _project.Scripts.Models;
using _project.Scripts.Player;
using _project.Scripts.Score;
using _project.Scripts.UI;
using _project.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _project.Scripts.Enemy
{

    public struct SpawnInformation
    {
        public float SpawnRate;
        public Queue<EnemyBase> EnemyBaseQueue;
        public int SpawnLevel;
    }

    public struct SpawnInfo
    {
        public EnemyType type;
        public float weight;

        public SpawnInfo(EnemyType a, float w)
        {
            type = a;
            weight = w;
        }
    }
    
    public class EnemySpawnSystem : MonoBehaviour
    {
        public static EnemySpawnSystem Instance;

        private ParticlePooler particlePooler;
        private ScoreManager scoreManager;

        private Dictionary<EnemyType, SpawnInformation> enemyPoolDictionary;

        private List<int> levelsToUpgradeEnemyLevel;

        private List<SpawnRates> chapterSpawnRates;

        private EnemyType currentEnemyTypeToSpawn;

        private ChapterInfo chapterInfo;

        private Transform playerTransform;
        private Transform myParent;

        private float accumulatedWeights;
        private System.Random rand;

        private float minDistance = 25f;
        private float maxDistance = 35f;

        private float baseSpawnRate;
        private float spawnCounter;
        private float spawnRateCurrentLimit;
        private float spawnChangeRate = 5f;
        private float spawnChangeCounter;

        private int spawnAmount = 150;
        private int maxAliveEnemyCount = 150;
        private int currentAliveEnemyCount;

        private bool isActive;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                myParent = transform.parent;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Init(ChapterInfo chapterInformation, Transform playerTransform, Transform tempParent)
        {
            chapterInfo = chapterInformation;
            chapterSpawnRates = chapterInformation.spawnRates;
            
            transform.SetParent(tempParent);
            playerTransform.SetParent(transform); // TODO why here ????

            var skillManager = SkillManager.Instance;
            var canvasElementPooler = CanvasElementPooler.Instance;
            var playerController = PlayerController.Instance;
            
            particlePooler = ParticlePooler.Instance;
            scoreManager = ScoreManager.Instance;
            
            this.playerTransform = playerTransform;
            baseSpawnRate = chapterSpawnRates[0].rate;

            if (enemyPoolDictionary != null)
            {
                for (var i = 0; i < chapterInformation.enemyInformation.Count; i++)
                {
                    var inLevelEnemyInfo = chapterInfo.enemyInformation[i];
                
                    EnemyType enemyType = inLevelEnemyInfo.enemyBase.GetEnemyType();

                    if (!enemyPoolDictionary.ContainsKey(enemyType))
                        continue;

                    var info = enemyPoolDictionary[enemyType];
                    info.SpawnLevel = inLevelEnemyInfo.spawnLevel;
                    enemyPoolDictionary[enemyType] = info;

                }
                return;
            }

            rand = new System.Random();
            
            enemyPoolDictionary = new Dictionary<EnemyType, SpawnInformation>();

            for (var i = 0; i < chapterInformation.enemyInformation.Count; i++)
            {
                var inLevelEnemyInfo = chapterInfo.enemyInformation[i];
                
                EnemyType enemyType = inLevelEnemyInfo.enemyBase.GetEnemyType();

                if (enemyPoolDictionary.ContainsKey(enemyType))
                    continue;
                
                SpawnInformation spawnInformation = new SpawnInformation();
                Queue<EnemyBase> enemyBaseQueue = new Queue<EnemyBase>();

                for (var j = 0; j < spawnAmount; j++)
                {
                    var enemy = Instantiate(inLevelEnemyInfo.enemyBase, Vector3.zero, Quaternion.identity, transform);
                    enemy.gameObject.SetActive(false);
                    enemy.Init(new EnemyInitializeInfo(inLevelEnemyInfo.spawnLevel, playerTransform, skillManager, canvasElementPooler, EnemyDead, DeSpawnEnemy, playerController));
                    
                    enemyBaseQueue.Enqueue(enemy);
                }

                spawnInformation.SpawnLevel = inLevelEnemyInfo.spawnLevel;
                spawnInformation.SpawnRate = inLevelEnemyInfo.spawnChance;
                spawnInformation.EnemyBaseQueue = enemyBaseQueue;
                
                enemyPoolDictionary.Add(enemyType, spawnInformation);
            }
            
            CalculateSpawnWeights();
        }

        public void Activate()
        {
            isActive = true;
        }

        public void DeActivate()
        {
            isActive = false;
        }


        public void ShouldUpgradeEnemyLevel(int scoreLevel)
        {
            var tempList = new List<EnemyType>();
            for (var i = 0; i < chapterInfo.enemyInformation.Count; i++)
            {
                var updateLevels = chapterInfo.enemyInformation[i].levelsToUpgradeEnemy;
                if (updateLevels.Contains(scoreLevel))
                {
                    tempList.Add(chapterInfo.enemyInformation[i].enemyBase.GetEnemyType());
                }
            }

            for (var i = 0; i < tempList.Count; i++)
            {
                if (!enemyPoolDictionary.ContainsKey(tempList[i])) continue;
                
                var hold = enemyPoolDictionary[tempList[i]];
                hold.SpawnLevel += 1;
                enemyPoolDictionary[tempList[i]] = hold;
            }
            
            //UpdateSpawnRateForLevel(scoreLevel);
        }


        private void UpdateSpawnRateForLevel(int scoreLevel)
        {
            for (var i = 0; i < chapterSpawnRates.Count; i++)
            {
                if (chapterSpawnRates[i].level == scoreLevel)
                {
                    baseSpawnRate = chapterSpawnRates[i].rate;
                    break;
                }
            }
        }


        public void UpdateSpawnRate(float rate)
        {
            baseSpawnRate = rate;
        }

        private void Update()
        {
            if (!isActive) return;

            spawnChangeCounter += Time.deltaTime;
            if (spawnChangeCounter >= spawnChangeRate)
            {
                spawnChangeCounter = 0f;
                PickEnemyToSpawn();
            }

            if (ManipulateSpawnRate()) return;

            spawnCounter += Time.deltaTime;
            if (spawnCounter < baseSpawnRate) return;
            spawnCounter = 0f;
            
            SpawnEnemy();
        }

        private bool ManipulateSpawnRate()
        {
            var difference = maxAliveEnemyCount - currentAliveEnemyCount;
            // var percentage = difference * 0.005f;
            // var addUp = _baseSpawnRate - percentage;
            // _spawnRateCurrentLimit = addUp;
            //_spawnRateCurrentLimit = 0.05f + _currentAliveEnemyCount * 0.005f;
            
            return difference <= 0;
        }

        private void PickEnemyToSpawn()
        {
            currentEnemyTypeToSpawn = GetRandomEnemyType();
        }

        private void SpawnEnemy()
        {
            var queue = enemyPoolDictionary[currentEnemyTypeToSpawn].EnemyBaseQueue;
            var enemyBase = queue.Dequeue();
            
            var counter = 0;
            var limit = queue.Count;
            
            while (counter < limit && enemyBase.gameObject.activeSelf)
            {
                queue.Enqueue(enemyBase);
                enemyBase = queue.Dequeue();
                counter += 1;
            }
            
            queue.Enqueue(enemyBase);

            if (counter >= limit && enemyBase.gameObject.activeSelf) return;

            enemyBase.transform.position = GetRandomPosition();
            enemyBase.gameObject.SetActive(true);
            enemyBase.Activate(enemyPoolDictionary[currentEnemyTypeToSpawn].SpawnLevel);

            currentAliveEnemyCount += 1;
        }

        private void EnemyDead(float killScore, Vector3 enemyPosition)
        {
            currentAliveEnemyCount -= 1;
            enemyPosition.y = 1f;
            scoreManager.AddExtraPoint(killScore);
            particlePooler.SpawnFromPool(SpawnType.Death, enemyPosition).Play();
        }
        
        private void DeSpawnEnemy()
        {
            currentAliveEnemyCount -= 1;
        }

        private Vector3 GetRandomPosition()
        {
            var rand = Random.Range(0f, 360f);
            var randDirection = Quaternion.Euler(0f, rand, 0f);
            var randDist = minDistance + (rand / 36f);
            var point = randDirection * transform.forward * randDist + playerTransform.position;

            if (point.x > 150f || point.x < -150f)
            {
                var add = point.x > 0 ? -1 : 1;
                point.x += randDist * add;
            }
            
            if (point.z > 150f || point.z < -150f)
            {
                var add = point.z > 0 ? -1 : 1;
                point.z += randDist * add;
            }
            // var point = randDirection * transform.forward * Random.Range(_minDistance, _maxDistance) + _playerTransform.position;

            return point;
        }
        
        private EnemyType GetRandomEnemyType () {
            double r = rand.NextDouble() * accumulatedWeights ;
            
            foreach (var spawnInformation in enemyPoolDictionary)
            {
                if (enemyPoolDictionary[spawnInformation.Key].SpawnRate > r)
                {
                    return spawnInformation.Key;
                }
            }
            return EnemyType.Minion;
        }
        
        private void CalculateSpawnWeights () {
            accumulatedWeights = 0f ;
            var tempList = new List<SpawnInfo>();
            
            foreach (var spawnInformation in enemyPoolDictionary.Keys)
            {
                var info = new SpawnInfo(spawnInformation, enemyPoolDictionary[spawnInformation].SpawnRate);
                accumulatedWeights += info.weight;
                info.weight = accumulatedWeights;
                tempList.Add(info);
            }

            for (var i = 0; i < tempList.Count; i++)
            {
                var info = enemyPoolDictionary[tempList[i].type];
                info.SpawnRate = tempList[i].weight;
                enemyPoolDictionary[tempList[i].type] = info;
            }
        }

        public void GameOver()
        {
            isActive = false;
            
            spawnCounter = 0f;
            spawnRateCurrentLimit = 0f;
            spawnChangeCounter = 0f;
            currentAliveEnemyCount = 0;

            foreach (var spawnInformation in enemyPoolDictionary)
            {
                var queue = enemyPoolDictionary[spawnInformation.Key].EnemyBaseQueue;

                foreach (var enemyBase in queue)
                {
                    enemyBase.GameOver();
                }
            }
            
            transform.SetParent(null);
        }


        public void ResetEnemies()
        {
            foreach (var spawnInformation in enemyPoolDictionary)
            {
                var queue = enemyPoolDictionary[spawnInformation.Key].EnemyBaseQueue;

                foreach (var enemyBase in queue)
                {
                    enemyBase.ResetSelf();
                }
            }
        }
    }
}