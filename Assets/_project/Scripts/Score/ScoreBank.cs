using System.IO;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.Score
{
    public static class ScoreBank
    {
        public static LevelScoreInfo GetScoreInfoForLevel(ChapterType chapterType)
        {
            var folderName = Path.Combine("Data", "ScoreInfo");
            var levelName = "ScoreFor_" + chapterType;
            
            return Resources.Load<LevelScoreInfo>(Path.Combine(folderName, levelName)); // TODO change name base to type base
        }
    }
}