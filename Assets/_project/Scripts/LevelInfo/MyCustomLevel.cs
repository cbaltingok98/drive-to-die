using System;
using System.Collections.Generic;
using _project.Scripts.AI;
using _project.Scripts.Enemy;
using _project.Scripts.Enums;
using _project.Scripts.Platform;
using _project.Scripts.Pools;
using UnityEngine;

namespace _project.Scripts.LevelInfo
{
    [Serializable]
    public struct ChapterInfo
    {
        public List<SpawnRates> spawnRates;
        public List<InLevelEnemyInfo> enemyInformation;
    }

    [Serializable]
    public struct SpawnRates
    {
        public int level;
        public float rate;
    }
    public class MyCustomLevel : MonoBehaviour
    {
        [SerializeField] private LevelThemes levelTheme;
        [SerializeField] private int targetStar;
        [SerializeField] private Transform carSpawnTransform;
        [SerializeField] private ChapterInfo chapterInformation;
        [SerializeField] private PlatformSpawner platformSpawner;
        

        public LevelThemes GetLevelTheme() => levelTheme;
        public Transform GetCarSpawnTransform() => carSpawnTransform;
        public ChapterInfo GetChapterInformation() => chapterInformation;

        public int GetTargetStar() => targetStar;


        public void SetChapterTarget(int target)
        {
            targetStar = target;
        }

        public void Init(Transform parent)
        {
            // platformSpawner.ApplySettings(new PlatformSettings());
            // platformSpawner.Init();
        }

    }
}