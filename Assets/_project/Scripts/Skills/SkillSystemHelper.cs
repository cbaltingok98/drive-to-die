using System;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Enums;
using _project.Scripts.Skills.Skill_Logic;
using UnityEngine;

namespace _project.Scripts.Skills
{
    public class SkillSystemHelper : MonoBehaviour
    {
        public static SkillSystemHelper Instance;
        
        [SerializeField] private List<Skill> skillTypes;

        private Dictionary<SkillType, Skill> skillDictionary;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        private void Initialize()
        {
            if (skillDictionary != null) return;
            
            ParseInfoToDictionary();
        }

        private void ParseInfoToDictionary()
        {
            skillDictionary = new Dictionary<SkillType, Skill>();

            for (var i = 0; i < skillTypes.Count; i++)
            {
                if (skillDictionary.ContainsKey(skillTypes[i].GetSkillType())) continue;

                skillDictionary.Add(skillTypes[i].GetSkillType(), skillTypes[i]);
            }
        }

        public void SetSkillTransform(SkillPlacementInfo placementInfo)
        {
            Initialize();
            if (!skillDictionary.ContainsKey(placementInfo.skillType)) return;

            var skill = skillDictionary[placementInfo.skillType].transform; 
            skill.SetParent(placementInfo.transform);
            skill.localPosition = Vector3.zero;
            skill.localRotation = Quaternion.identity;
            skill.gameObject.SetActive(false);
        }


        public Skill GetSkill(SkillType skillType)
        {
            Initialize();
            if (!skillDictionary.ContainsKey(skillType)) return null;

            return skillDictionary[skillType];
        }

        public Dictionary<SkillType, Skill> GetSkillDictionary()
        {
            Initialize();
            return skillDictionary;
        }


        public SkillType GetFirstAvailableSkill()
        {
            Initialize();
            var key = skillDictionary.FirstOrDefault();
            return key.Key;
        }

        public void Reset()
        {
            for (var i = 0; i < skillTypes.Count; i++)
            {
                skillTypes[i].transform.SetParent(transform);
                skillTypes[i].Disable();
            }
        }
    }
}