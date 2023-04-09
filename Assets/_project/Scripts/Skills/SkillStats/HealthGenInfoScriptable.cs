using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Skills.Skill_Logic.HEALTH_GEN;
using UnityEngine;

namespace _project.Scripts.Skills.SkillStats
{
    [CreateAssetMenu(fileName = "New Health Generator Info", menuName = "ScriptableObjects/SkillInfos/Health Gen Info", order = 1)]
    public class HealthGenInfoScriptable : ScriptableObject
    {
        public int unlockPrice;
        public int level;
        public List<HealthGeneratorLevelInfo> healthGeneratorLevels;
        public List<int> priceList;
        
        public string GetSkillInfos()
        {
            var curLevelInfo = healthGeneratorLevels[DataManager.Instance.GetSkillLevelFor(SkillType.HealthGenerator) - 1];

            return "<color=\"orange\">1 star</color> Heal : <color=\"green\">" +
                   curLevelInfo.healthGeneratorLevelInfo[0].percent + "%</color> for every <color=\"green\">" +
                   curLevelInfo.healthGeneratorLevelInfo[0].forEveryKill + " kill</color><br>" +
                   "<color=\"orange\">5 star</color> Heal : <color=\"green\">" +
                   curLevelInfo.healthGeneratorLevelInfo[curLevelInfo.healthGeneratorLevelInfo.Count - 1].percent +
                   "%</color> for every <color=\"green\">" +
                   curLevelInfo.healthGeneratorLevelInfo[curLevelInfo.healthGeneratorLevelInfo.Count - 1].forEveryKill +
                   " kill</color><br>";
        }
    }
}