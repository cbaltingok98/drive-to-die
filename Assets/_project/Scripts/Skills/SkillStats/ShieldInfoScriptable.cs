using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Skills.Skill_Logic;
using _project.Scripts.Skills.Skill_Logic.SHIELD;
using UnityEngine;

namespace _project.Scripts.Skills.SkillStats
{
    [CreateAssetMenu(fileName = "New Shield Info", menuName = "ScriptableObjects/SkillInfos/Shield Info", order = 1)]
    public class ShieldInfoScriptable : ScriptableObject
    {
        public int unlockPrice;
        public int level;
        public List<ShieldLevelInfo> shieldLevels;
        public List<int> priceList;
        
        public string GetSkillInfos()
        {
            var curLevelInfo = shieldLevels[DataManager.Instance.GetSkillLevelFor(SkillType.Shield) - 1];

            return "Nothing can hurt you when the shield is active.<br><br>" +
                   "<color=\"orange\">1 star</color> Active Duration : " + "<color=\"green\">" +
                   curLevelInfo.shieldLevelInfo[0].activeTime + "</color> seconds<br>" +
                   "<color=\"orange\">5 star</color> Active Duration : " + "<color=\"green\">" +
                   curLevelInfo.shieldLevelInfo[curLevelInfo.shieldLevelInfo.Count - 1].activeTime + "</color> seconds<br>" +
                   "<br>" +
                   "<color=\"orange\">1 star</color> Passive Duration : " + "<color=\"green\">" +
                   curLevelInfo.shieldLevelInfo[0].passiveTime + "</color> seconds<br>" +
                   "<color=\"orange\">5 star</color> Passive Duration : " + "<color=\"green\">" +
                   curLevelInfo.shieldLevelInfo[curLevelInfo.shieldLevelInfo.Count - 1].passiveTime + "</color> seconds<br>";
        }
    }
}