using System.Collections.Generic;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.Car
{
    [CreateAssetMenu(fileName = "New Driving Profile", menuName = "ScriptableObjects/Driving Profile", order = 1)]
    public class DrivingProfile: ScriptableObject
    {
        public DrivingProfileType profileType;
        public string displayName;
        public int level;
        public List<DrivingStats> drivingStatsList;
        public List<int> upgradePrices;
    }
}