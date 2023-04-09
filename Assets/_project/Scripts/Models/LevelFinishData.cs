using System;
using _project.Scripts.Enums;

namespace _project.Scripts.Models
{
    [Serializable]
    public class LevelFinishData
    {
        public ChapterType chapterType;
        public int playTime;
        public int skillScore;
        public int killCount;
        public int coinToEarn;


        public LevelFinishData(ChapterType chapterType, int playTime, int skillScore, int killCount, int coinToEarn)
        {
            this.chapterType = chapterType;
            this.playTime = playTime;
            this.skillScore = skillScore;
            this.killCount = killCount;
            this.coinToEarn = coinToEarn;
        }
    }
}