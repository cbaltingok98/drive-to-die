using System;
using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enemy.Enemies;
using _project.Scripts.Enums;
using _project.Scripts.Score;
using _project.Scripts.Skills;
using _project.Scripts.Skills.Skill_Logic;
using _project.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _project.Scripts.Managers
{
    public struct SkillPromptInfo
    {
        public SkillType SkillType;
        public Sprite SkillSprite;
        public string SkillName;
        public string SkillDescription;
        public int SkillLevel;
        public UnityAction<SkillType, SingleSkillUi> Action;
    }

    public struct ClosestEnemyInfo
    {
        public EnemyBase EnemyBase;
        public float Distance;


        public ClosestEnemyInfo(EnemyBase enemyBase, float distance)
        {
            EnemyBase = enemyBase;
            Distance = distance;
        }
    }
    
    public class SkillManager : MonoBehaviour
    {
        public static SkillManager Instance;
        
        private SkillSystemHelper skillSystemHelper;

        private Dictionary<SkillType, Sprite> _skillImageBank;
        private Dictionary<SkillType, Skill> _skillDictionary;
        private Dictionary<SkillType, Skill> _activatedSkill;

        private ClosestEnemyInfo _closestEnemy;

        private List<Skill> _activePassiveSkills;

        private bool _canInteractWithPrompt;

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

        public void Init()
        {
            _activePassiveSkills = new List<Skill>();
            _activatedSkill = new Dictionary<SkillType, Skill>();
            skillSystemHelper = SkillSystemHelper.Instance;

            _skillDictionary = skillSystemHelper.GetSkillDictionary();
            ParseImageDataToDictionary();
        }

        public void UnlockSkill(SkillType skillType, SingleSkillUi singleSkillUi)
        {
            if (!_canInteractWithPrompt) return;
            var skill = _skillDictionary[skillType];
            if (skill.IsMaxedOutInChapterUpgrade()) return;

            _canInteractWithPrompt = false;
            skill.Initialize();
            AddToActivatedSkills(skillType);
            singleSkillUi.UpdateDone();

            if (skill.IsPassiveSkill())
            {
                _activePassiveSkills.Add(skill);
            }
        }


        private void AddToActivatedSkills(SkillType skillType)
        {
            if (_activatedSkill.ContainsKey(skillType)) return;
            
            _activatedSkill.Add(skillType, _skillDictionary[skillType]);
        }


        public Dictionary<SkillType, Skill> GetActiveSkills() => _activatedSkill;

        public int GetSkillLevel(SkillType skillType)
        {
            return _skillDictionary[skillType].GetCurrentLevel();
        }

        public List<SkillPromptInfo> GetSkillsForPrompt()
        {
            var newSkillPromptList = new List<SkillPromptInfo>();
            var typeList = new List<int>();
            
            var numberOfSKills = Enum.GetValues(typeof(SkillType)).Length;
            var maxCounter = 0;

            if (ScoreManager.Instance.GetStarAmount() == 1)
            {
                typeList.Add((int)SkillType.Shield);
            }
            else
            {
                while (typeList.Count < 3)
                {
                    var flag = true;
                    var randNum = Random.Range(0, numberOfSKills);

                    if (!_skillDictionary.ContainsKey((SkillType)randNum)) continue;
                    if (typeList.Contains(randNum)) continue;
                    if (!DataManager.Instance.IsSkillUnlocked((SkillType)randNum)) continue;

                    if (_skillDictionary[(SkillType)randNum].IsMaxedOutInChapterUpgrade())
                    {
                        maxCounter += 1;
                        flag = false;
                    }

                    if (!flag && maxCounter >= 3 && typeList.Count == 2) continue;

                    typeList.Add(randNum);
                }
            }

            for (var i = 0; i < typeList.Count; i++)
            {
                var newType = (SkillType)typeList[i];
                var skill = _skillDictionary[newType];

                if (skill.IsMaxedOutInChapterUpgrade()) continue;
                
                var newInfo = new SkillPromptInfo();
                
                newInfo.Action = UnlockSkill;
                newInfo.SkillDescription = skill.GetDescription().ToUpper();
                newInfo.SkillName = skill.GetName().ToUpper();
                newInfo.SkillLevel = skill.GetCurrentLevel();
                newInfo.SkillSprite = skill.GetSkillSprite();
                newInfo.SkillType = skill.GetSkillType();
            
                newSkillPromptList.Add(newInfo);
            }

            _canInteractWithPrompt = true; // leave it as it is

            return newSkillPromptList;
        }


        public void ActivatePassiveSkills()
        {
            for (var i = 0; i < _activePassiveSkills.Count; i++)
            {
                _activePassiveSkills[i].ActivatePassive();
            }
        }


        public bool HasClosestEnemy() => _closestEnemy.EnemyBase != null;
        public bool SetClosestEnemy(ClosestEnemyInfo closestEnemyInfo)
        {
            //if (_closestEnemy.EnemyBase != null) return false;
            if (closestEnemyInfo.Distance >= (_skillDictionary[SkillType.MiniGun].GetRadius())) return false;

            _closestEnemy = closestEnemyInfo;
            _skillDictionary[SkillType.MiniGun].SetTarget(_closestEnemy);
            return true;
        }


        public void RemoveClosestEnemy(bool sentByEnemy = false)
        {
            if (!sentByEnemy && _closestEnemy.EnemyBase != null)
            {
                _skillDictionary[SkillType.MiniGun].RemoveTarget();
                _closestEnemy.EnemyBase.RemovedFromClosestTarget();
            }
            
            _closestEnemy = new ClosestEnemyInfo();
        }

        public ClosestEnemyInfo GetClosestEnemy() => _closestEnemy;

        private void ParseImageDataToDictionary()
        {
            if (_skillImageBank != null) return;
            
            _skillImageBank = new Dictionary<SkillType, Sprite>();
            
            foreach (var skill in _skillDictionary)
            {
                _skillImageBank.Add(skill.Key, _skillDictionary[skill.Key].GetSkillSprite());
            }
        }


        public void GameOver()
        {
            foreach (var dictionary in _skillDictionary)
            {
                var skill = _skillDictionary[dictionary.Key];
                skill.ResetSkill();
            }
            
            // _skillDictionary.Clear();
            // _skillImageBank.Clear();
            _activatedSkill.Clear();

            _canInteractWithPrompt = false;
        }
    }
}