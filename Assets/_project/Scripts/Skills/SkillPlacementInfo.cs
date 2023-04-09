using System;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.Skills
{
    [Serializable]
    public class SkillPlacementInfo
    {
        public SkillType skillType;
        public Transform transform;
    }
}