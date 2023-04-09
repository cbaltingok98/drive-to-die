using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Skills.Skill_Logic.MINIGUN;
using UnityEngine;

namespace _project.Scripts.Skills.SkillStats
{
    [CreateAssetMenu(fileName = "New Mini Gun Info", menuName = "ScriptableObjects/SkillInfos/MiniGun Info", order = 1)]
    public class MiniGunInfoScriptable : ScriptableObject
    {
        public int unlockPrice;
        public int level;
        public List<MiniGunLevelInfo> miniGunLevels;
        public List<int> priceList;
        
        public string GetSkillInfos()
        {
            var curLevelInfo = miniGunLevels[DataManager.Instance.GetSkillLevelFor(SkillType.MiniGun) - 1];

            return "<color=\"orange\">1 star</color> Damage : " + "<color=\"green\">" + curLevelInfo.miniGunLevelInfo[0].damage + "</color><br>" +
                   "<color=\"orange\">5 star</color> Damage : " + "<color=\"green\">" + curLevelInfo.miniGunLevelInfo[curLevelInfo.miniGunLevelInfo.Count-1].damage + "</color><br>" +
                   "<br>" +
                   "<color=\"orange\">1 star</color> Fire Rate : " + "<color=\"green\">" + curLevelInfo.miniGunLevelInfo[0].fireRate + "s</color><br>" +
                   "<color=\"orange\">5 star</color> Fire Rate : " + "<color=\"green\">" + curLevelInfo.miniGunLevelInfo[curLevelInfo.miniGunLevelInfo.Count-1].fireRate + "s</color><br>" +
                   "<br>" +
                   "<color=\"orange\">1 star</color> Radius : " + "<color=\"green\">" + curLevelInfo.miniGunLevelInfo[0].radius + "ft</color><br>" +
                   "<color=\"orange\">5 star</color> Radius : " + "<color=\"green\">" + curLevelInfo.miniGunLevelInfo[curLevelInfo.miniGunLevelInfo.Count-1].radius + "ft</color><br>";
        }
    }
}