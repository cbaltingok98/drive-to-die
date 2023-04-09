using System.Collections.Generic;
using UnityEngine;

namespace _project.Scripts.Score
{
    [CreateAssetMenu(fileName = "New Score Info", menuName = "ScriptableObjects/Score Info", order = 3)]
    public class LevelScoreInfo : ScriptableObject
    {
        public float driftPointsPerFrame;
        public List<float> requiredPointsForStar;
        public float correctHit;
        public float wrongHit;
    }
}