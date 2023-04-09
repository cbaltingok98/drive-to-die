using System;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.AI
{
    [Serializable]
    public class InLevelAiInfo
    {
        public bool active;
        public Transform gridPos;
        public DrivingProfileType drivingProfile;
        public CarModels carModel;
        //public Difficulty difficulty; // TODO not in use
    }
}