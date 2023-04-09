using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Skills.Skill_Logic.BOMB;
using UnityEngine;

namespace _project.Scripts.Skills.SkillStats
{
    [CreateAssetMenu(fileName = "New Bomb Info", menuName = "ScriptableObjects/SkillInfos/Bomb Info", order = 1)]
    public class BombInfoScriptable : ScriptableObject
    {
        public int unlockPrice;
        public int level;
        public List<BombLevelInfo> bombLevels;
        public List<int> priceList;

        public string GetSkillInfos()
        {
            var curLevelInfo = bombLevels[DataManager.Instance.GetSkillLevelFor(SkillType.Bomb) - 1];

            return "<color=\"orange\">1 star</color> Drops :" + "<color=\"green\">" + curLevelInfo.bombLevelInfo[0].amount + "</color> bomb(s)<br>" +
                   "<color=\"orange\">5 star</color> Drops : " + "<color=\"green\">" + curLevelInfo.bombLevelInfo[curLevelInfo.bombLevelInfo.Count-1].amount + "</color> bomb(s)<br>" +
                   "<br>" +
                   "<color=\"orange\">1 star</color> Damage : " + "<color=\"green\">" + curLevelInfo.bombLevelInfo[0].damage + "</color><br>" +
                   "<color=\"orange\">5 star</color> Damage : " + "<color=\"green\">" + curLevelInfo.bombLevelInfo[curLevelInfo.bombLevelInfo.Count-1].damage + "</color><br>" +
                   "<br>" +
                   "<color=\"orange\">1 star</color> Blast Radius : " + "<color=\"green\">" + curLevelInfo.bombLevelInfo[0].blastRadius + "</color>ft<br>" +
                   "<color=\"orange\">5 star</color> Blast Radius : " + "<color=\"green\">" + curLevelInfo.bombLevelInfo[curLevelInfo.bombLevelInfo.Count-1].blastRadius + "</color>ft<br>" +
                   "<br>" +
                   "<color=\"orange\">1 star</color> Wait " + "<color=\"green\">" + curLevelInfo.bombLevelInfo[0].passiveTime + "</color> seconds before another drop<br>" +
                   "<color=\"orange\">5 star</color> Wait " + "<color=\"green\">" + curLevelInfo.bombLevelInfo[curLevelInfo.bombLevelInfo.Count-1].passiveTime + "</color> seconds before another drop<br>";
        }
    }
}