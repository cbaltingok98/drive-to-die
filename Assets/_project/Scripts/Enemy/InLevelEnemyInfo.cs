using System;
using System.Collections.Generic;
using _project.Scripts.Enemy.Enemies;
using UnityEngine;

namespace _project.Scripts.Enemy
{
    [Serializable]
    public class InLevelEnemyInfo
    {
        public EnemyBase enemyBase;
        [Range(0f, 100f)] public float spawnChance;
        [Range(1, 10)] public int spawnLevel;
        public List<int> levelsToUpgradeEnemy;
    }
}