using System;
using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;

namespace _project.Scripts.Data
{
    [Serializable]
    public class PlayerData
    {
        public ChapterType currentChapter;
        public CarModels currentCarModel;
        public CharacterType currentCharacter;
        
        public int gemCount;
        public int starLevel;
        
        public bool playedBefore;
        public bool carLevelsCreated;
        public bool canVibrate;
        public bool canPlaySound;
        public bool canPlayMusic;
        
        public List<CarPurchaseData> carPurchaseData;
        public List<ChapterUnlockData> chapterUnlockData;
        public List<CarProfileLevelData> carProfileLevels;
        public List<SkillLevelData> skillLevels;
        public List<SkillUnlockData> skillUnlockData;

        public PlayerData()
        {
            canVibrate = true;
            canPlaySound = true;
            canPlayMusic = true;
            
            Constants.CanVibrate = canVibrate;
            Constants.CanPlaySound = canPlaySound;
            Constants.CanPlayMusic = canPlayMusic;
            
            currentChapter = Constants.InitialChapterType;
            gemCount = Constants.InitialCoinAmount;
            starLevel = Constants.InitialStarLevel;
            currentCarModel = Constants.InitialCarModel;
            currentCharacter = Constants.InitialCharacter;

            carPurchaseData = new List<CarPurchaseData>();
            chapterUnlockData = new List<ChapterUnlockData>();
            carProfileLevels = new List<CarProfileLevelData>();
            skillLevels = new List<SkillLevelData>();
            skillUnlockData = new List<SkillUnlockData>();

            var length = Enum.GetValues(typeof(CarModels)).Length;
            var chapterAmount = Enum.GetValues(typeof(ChapterType)).Length;
            var drivingProfilesCount = Enum.GetValues(typeof(DrivingProfileType)).Length;
            var skillCount = Enum.GetValues(typeof(SkillType)).Length;

            for (int i = 0; i < length; i++)
            {
                var newData = new CarPurchaseData((CarModels)i, false);
                carPurchaseData.Add(newData);
            }

            for (int i = 0; i < chapterAmount; i++)
            {
                var newData = new ChapterUnlockData((ChapterType)i, false);
                chapterUnlockData.Add(newData);
            }
            
            for (var i = 0; i < drivingProfilesCount; i++)
            {
                var newData = new CarProfileLevelData((DrivingProfileType)i, 1);
                carProfileLevels.Add(newData);
            }

            for (var i = 0; i < skillCount; i++)
            {
                var newData = new SkillLevelData((SkillType)i, 1);
                skillLevels.Add(newData);
            }

            for (var i = 0; i < skillCount; i++)
            {
                var newData = new SkillUnlockData((SkillType)i, false);
                skillUnlockData.Add(newData);
            }
            
            
            
            UnlockCar(currentCarModel);
            UnlockChapter(currentChapter);
            
            // UnlockCar(CarModels.SUV);
            // UnlockCar(CarModels.Lambo);
            
            // UnlockChapter(ChapterType.Chapter2);

            UnlockSkill(SkillType.MiniGun);
            UnlockSkill(SkillType.Bomb);
            UnlockSkill(SkillType.Shield);
            UnlockSkill(SkillType.RocketLauncher);
            UnlockSkill(SkillType.HealthGenerator);
            UnlockSkill(SkillType.FuelGenerator);
        }


        public void AddStarLevel()
        {
            starLevel += 1;
        }
        
        public void PlayedBefore()
        {
            playedBefore = true;
        }


        #region Settings

        public void DisableVibration()
        {
            canVibrate = false;
            UpdateVibrationSetting();
        }


        public void EnableVibration()
        {
            canVibrate = true;
            UpdateVibrationSetting();
        }

        private void UpdateVibrationSetting()
        {
            Constants.CanVibrate = canVibrate;
        }
        
        public void DisableSound()
        {
            canPlaySound = false;
            UpdateSoundSetting();
        }


        public void EnableSound()
        {
            canPlaySound = true;
            UpdateSoundSetting();
        }

        private void UpdateSoundSetting()
        {
            Constants.CanPlaySound = canPlaySound;
        }

        public void DisableMusic()
        {
            canPlayMusic = false;
            UpdateMusicSetting();
        }


        public void EnableMusic()
        {
            canPlayMusic = true;
            UpdateMusicSetting();
        }

        private void UpdateMusicSetting()
        {
            Constants.CanPlayMusic = canPlayMusic;
        }


        public void UpdateSettings()
        {
            UpdateVibrationSetting();
            UpdateSoundSetting();
            UpdateMusicSetting();
        }
        
        #endregion
        
        
        #region Chapter

        public void UnlockChapter(ChapterType newChapter)
        {
            for (var i = 0; i < chapterUnlockData.Count; i++)
            {
                var data = chapterUnlockData[i];
                if (data.chapterType == newChapter)
                {
                    data.isUnlocked = true;
                    chapterUnlockData[i] = data;
                    break;
                }
            }
        }
        

        public bool IsChapterUnlocked(ChapterType chapterType)
        {
            for (var i = 0; i < chapterUnlockData.Count; i++)
            {
                var data = chapterUnlockData[i];
                if (data.chapterType == chapterType)
                {
                    return data.isUnlocked;
                }
            }

            return false;
        }

        #endregion


        #region Gem

        public void AddGem(int value)
        {
            gemCount += value;
        }


        public void SpendCoin(int amount)
        {
            gemCount -= amount;
        }

        #endregion

        
        #region Car

        public void UnlockCar(CarModels newModel)
        {
            for (var i = 0; i < carPurchaseData.Count; i++)
            {
                var data = carPurchaseData[i];
                if (data.carModel == newModel)
                {
                    data.isUnlocked = true;
                    carPurchaseData[i] = data;
                    break;
                }
            }
        }
        
        public bool IsCarUnlocked(CarModels carModel)
        {
            for (var i = 0; i < carPurchaseData.Count; i++)
            {
                var data = carPurchaseData[i];
                if (data.carModel == carModel)
                {
                    return data.isUnlocked;
                }
            }

            return false;
        }
        
        public void UpdateCarLevel(DrivingProfileType drivingProfileType)
        {
            for (var i = 0; i < carProfileLevels.Count; i++)
            {
                if (carProfileLevels[i].carProfileType == drivingProfileType)
                {
                    carProfileLevels[i].level += 1;
                    break;
                }
            }
        }


        public int GetDrivingProfileLevelFor(DrivingProfileType drivingProfileKey)
        {
            for (var i = 0; i < carProfileLevels.Count; i++)
            {
                if (carProfileLevels[i].carProfileType == drivingProfileKey)
                {
                    return carProfileLevels[i].level;
                }
            }

            return -1;
        }
        
        #endregion


        #region Skill

        public void UpdateSkillLevel(SkillType skillType)
        {
            for (var i = 0; i < skillLevels.Count; i++)
            {
                if (skillLevels[i].skillType == skillType)
                {
                    skillLevels[i].level += 1;
                    break;
                }
            }
        }


        public int GetSkillLevelFor(SkillType skillType)
        {
            for (var i = 0; i < skillLevels.Count; i++)
            {
                if (skillLevels[i].skillType == skillType)
                {
                    return skillLevels[i].level;
                }
            }

            return -1;
        }


        public void UnlockSkill(SkillType skillType)
        {
            for (var i = 0; i < skillUnlockData.Count; i++)
            {
                if (skillUnlockData[i].skillType == skillType)
                {
                    skillUnlockData[i].isUnlocked = true;
                    break;
                }
            }
        }


        public bool IsSkillUnlocked(SkillType skillType)
        {
            for (var i = 0; i < skillUnlockData.Count; i++)
            {
                if (skillUnlockData[i].skillType == skillType)
                {
                    return skillUnlockData[i].isUnlocked;
                }
            }
            return false;
        }

        #endregion
    }

    [Serializable]
    public class CarPurchaseData
    {
        public CarModels carModel;
        public bool isUnlocked;


        public CarPurchaseData(CarModels carModel, bool unlocked)
        {
            this.carModel = carModel;
            this.isUnlocked = unlocked;
        }
    }
    
    [Serializable]
    public class ChapterUnlockData
    {
        public ChapterType chapterType;
        public bool isUnlocked;


        public ChapterUnlockData(ChapterType chapter, bool unlocked)
        {
            this.chapterType = chapter;
            this.isUnlocked = unlocked;
        }
    }
    
    [Serializable]
    public class CarProfileLevelData
    {
        public DrivingProfileType carProfileType;
        public int level;


        public CarProfileLevelData(DrivingProfileType profile, int level)
        {
            this.carProfileType = profile;
            this.level = level;
        }
    }

    [Serializable]
    public class SkillLevelData
    {
        public SkillType skillType;
        public int level;


        public SkillLevelData(SkillType skillType, int level)
        {
            this.skillType = skillType;
            this.level = level;
        }
    }
    
    [Serializable]
    public class SkillUnlockData
    {
        public SkillType skillType;
        public bool isUnlocked;


        public SkillUnlockData(SkillType skillType, bool isUnlocked)
        {
            this.skillType = skillType;
            this.isUnlocked = isUnlocked;
        }
    }

}