using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Skills.Skill_Logic;
using _project.Scripts.Skills.Skill_Logic.ROCKETLAUNCHER;
using UnityEngine;

namespace _project.Scripts.Skills.SkillStats
{
    [CreateAssetMenu(fileName = "New Rocket Launcher Info", menuName = "ScriptableObjects/SkillInfos/Rocket Launcher Info", order = 1)]
    public class RocketLauncherInfoScriptable : ScriptableObject
    {
        public int unlockPrice;
        public int level;
        public List<RocketLauncherLevelInfo> rocketLauncherLevels;
        public List<int> priceList;
        
        public string GetSkillInfos()
        {
            var curLevelInfo = rocketLauncherLevels[DataManager.Instance.GetSkillLevelFor(SkillType.RocketLauncher) - 1];

            return "<color=\"orange\">1 star</color> Damage Per Rocket : " + "<color=\"green\">" + curLevelInfo.rocketLauncherLevelInfos[0].damage + "</color><br>" +
                   "<color=\"orange\">5 star</color> Damage Per Rocket : " + "<color=\"green\">" + curLevelInfo.rocketLauncherLevelInfos[curLevelInfo.rocketLauncherLevelInfos.Count-1].damage + "</color><br>" +
                   "<br>" +
                   "<color=\"orange\">1 star</color> Fired Rockets : " + "<color=\"green\">#" + curLevelInfo.rocketLauncherLevelInfos[0].numOfRockets + "</color><br>" +
                   "<color=\"orange\">5 star</color> Fired Rockets : " + "<color=\"green\">#" + curLevelInfo.rocketLauncherLevelInfos[curLevelInfo.rocketLauncherLevelInfos.Count-1].numOfRockets + "</color><br>" +
                   "<br>" +
                   "<color=\"orange\">1 star</color> Fire Rate : " + "<color=\"green\">" + curLevelInfo.rocketLauncherLevelInfos[0].fireRate + "s</color><br>" +
                   "<color=\"orange\">5 star</color> Fire Rate : " + "<color=\"green\">" + curLevelInfo.rocketLauncherLevelInfos[curLevelInfo.rocketLauncherLevelInfos.Count-1].fireRate + "s</color><br>" + 
                   "<br>" +
                   "<color=\"orange\">1 star</color> Blast Radius : " + "<color=\"green\">" + curLevelInfo.rocketLauncherLevelInfos[0].blastRadius + "ft</color><br>" +
                   "<color=\"orange\">5 star</color> Blast Radius : " + "<color=\"green\">" + curLevelInfo.rocketLauncherLevelInfos[curLevelInfo.rocketLauncherLevelInfos.Count-1].blastRadius + "ft</color><br>" +
                   "<br>" +
                   "<color=\"orange\">1 star</color> Rocket Speed : " + "<color=\"green\">" + curLevelInfo.rocketLauncherLevelInfos[0].rocketSpeed + "</color><br>" +
                   "<color=\"orange\">5 star</color> Rocket Speed : " + "<color=\"green\">" + curLevelInfo.rocketLauncherLevelInfos[curLevelInfo.rocketLauncherLevelInfos.Count-1].rocketSpeed + "</color><br>" + 
                   "<br>" +
                   "<color=\"orange\">1 star</color> Passive Time : " + "<color=\"green\">" + curLevelInfo.rocketLauncherLevelInfos[0].passiveTime + "s</color><br>" +
                   "<color=\"orange\">5 star</color> Passive Time : " + "<color=\"green\">" + curLevelInfo.rocketLauncherLevelInfos[curLevelInfo.rocketLauncherLevelInfos.Count-1].passiveTime + "s</color><br>";
        }
    }
}