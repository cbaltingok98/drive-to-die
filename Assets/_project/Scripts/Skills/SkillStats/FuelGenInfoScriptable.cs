using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Skills.Skill_Logic.FUEL_GEN;
using UnityEngine;

namespace _project.Scripts.Skills.SkillStats
{
    [CreateAssetMenu(fileName = "New Fuel Generator Info", menuName = "ScriptableObjects/SkillInfos/Fuel Gen Info", order = 1)]
    public class FuelGenInfoScriptable : ScriptableObject
    {
        public int unlockPrice;
        public int level;
        public List<FuelGeneratorLevelInfo> fuelGeneratorLevels;
        public List<int> priceList;
        
        public string GetSkillInfos()
        {
            var curLevelInfo = fuelGeneratorLevels[DataManager.Instance.GetSkillLevelFor(SkillType.FuelGenerator) - 1];

            return "<color=\"orange\">1 star</color> Fill : <color=\"green\">" + curLevelInfo.fuelGeneratorLevelInfo[0].percent + "%</color> for every " + "<color=\"green\">" +
                   curLevelInfo.fuelGeneratorLevelInfo[0].forEveryKill + " kill</color><br>" +
                   "<color=\"orange\">5 star</color> Fill : <color=\"green\">" +
                   curLevelInfo.fuelGeneratorLevelInfo[curLevelInfo.fuelGeneratorLevelInfo.Count - 1].percent +
                   "%</color> for every " + "<color=\"green\">" +
                   curLevelInfo.fuelGeneratorLevelInfo[curLevelInfo.fuelGeneratorLevelInfo.Count - 1].forEveryKill +
                   " kill</color><br>";
        }
    }
}