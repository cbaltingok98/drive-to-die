using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.LevelInfo
{
    [CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/Level", order = 2)]
    public class LevelObject : ScriptableObject
    {
        public Sprite sprite;
        public ChapterType id;
        public GameObject levelPrefab;
        public string chapterName;
        public int chapterTarget;
        public bool comingSoon;
    }
}