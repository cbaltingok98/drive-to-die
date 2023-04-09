using System;
using System.Collections.Generic;
using System.IO;
using _project.Scripts.Berk;
using _project.Scripts.Car;
using _project.Scripts.Data;
using _project.Scripts.Enums;
using _project.Scripts.LevelInfo;
using _project.Scripts.Themes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _project.Scripts.Managers
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;
        
        public PlayerData PlayerData { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        
        private void Initialize()
        {
            PlayerData = SaveLoadSystem.Load();
            SaveAppInstallTimeStamp();
            DrivingProfileManager.UpdateDrivingProfileLevels();
            
            PlayerData.UpdateSettings();
        }

        public static IEnumerable<DrivingProfile> GetDrivingProfiles()
        {
            var folderName = Path.Combine("Data", "DrivingProfiles");
            var profiles = Resources.LoadAll<DrivingProfile>(folderName);

            return profiles;
        }

        public static IEnumerable<ThemeImage> GetThemeImages()
        {
            var folderName = Path.Combine("Data", "ThemeImages");
            var images = Resources.LoadAll<ThemeImage>(folderName);

            return images;
        }


        public static IEnumerable<LevelObject> GetLevelObjects()
        {
            var folderName = Path.Combine("Data", "Levels");
            var levelObjects = Resources.LoadAll<LevelObject>(folderName);

            return levelObjects;
        }


        public static IEnumerable<T> GetSkillInfo<T>() where T : Object
        {
            var folderName = Path.Combine("Data", "SkillInfoProfiles");
            var skillInfoProfile = Resources.LoadAll<T>(folderName);

            return skillInfoProfile;
        }

        // public static SkillDataBank GetSkillDataBank()
        // {
        //     var folderName = Path.Combine("Data", "Skills");
        //     var skillDataBank = Resources.Load<SkillDataBank>(folderName);
        //
        //     return skillDataBank;
        // }

        public Level LoadLevel()
        {
            var chapterName = PlayerData.currentChapter;
            
            //var manipulatedLevelId = ManipulateLevelId(levelId);
            
            var folderName = Path.Combine("Data", "Levels");
            var levelName = "Level_" + chapterName;
            
            var levelObject = Resources.Load<LevelObject>(Path.Combine(folderName, levelName));
            
            var level = new Level(levelObject);
            return level;
        }

        // public Level LoadTutorialLevel()
        // {
        //     var folderName = Path.Combine("Data", "Levels", "TutorialLevel");
        //     var levelObject = Resources.Load<LevelObject>(folderName);
        //
        //     var level = new Level(ChapterType.TutorialChapter, levelObject.levelPrefab);
        //     return level;
        // }

        private int ManipulateLevelId(int levelId)
        {
            return (levelId - 1) % TotalLevelCount + 1;
        }
        
        private int _totalLevelCount = -1;
        
        private int TotalLevelCount
        {
            get
            {
                if (_totalLevelCount == -1)
                {
                    Object[] levels = Resources.LoadAll(Path.Combine("Data", "Levels"));
                    _totalLevelCount = levels.Length;
                    Resources.UnloadUnusedAssets();
                }

                return _totalLevelCount;
            }
        }


        public void AddStarLevel()
        {
            PlayerData.AddStarLevel();
            SaveLoadSystem.Save(PlayerData);
        }


        private bool HasEnoughCoinToPurchase(int required)
        {
            return PlayerData.gemCount >= required;
        }
        
        public void AddCoin(int coin)
        {
            var previous = PlayerData.gemCount;
            PlayerData.AddGem(coin);
            SaveLoadSystem.Save(PlayerData);
            CanvasManager.Instance.GetMenuUi().CoinContainerUI().UpdateCoinTextFromData(previous);
        }


        public bool SpendCoin(int amount)
        {
            if (HasEnoughCoinToPurchase(amount))
            {
                var previous = PlayerData.gemCount;
                PlayerData.SpendCoin(amount);
                CanvasManager.Instance.GetMenuUi().CoinContainerUI().UpdateCoinTextFromData(previous);
                return true;
            }

            return false;
        }
        
        public void UnlockCar(CarModels newModel)
        {
            PlayerData.UnlockCar(newModel);
            SaveLoadSystem.Save(PlayerData);
        }

        public bool IsCarUnlocked(CarModels carModel)
        {
            return PlayerData.IsCarUnlocked(carModel);
        }


        public CarModels GetCurrentCarModel()
        {
            return PlayerData.currentCarModel;
        }


        public CharacterType GetCurrentCharacter()
        {
            return PlayerData.currentCharacter;
        }

        public void LevelPassed()
        {
            //PlayerData.LevelPassed();
            SaveLoadSystem.Save(PlayerData);
        }

        public bool IsPlayedBefore()
        {
            return PlayerData.playedBefore;
        }

        public void PlayedBefore()
        {
            PlayerData.PlayedBefore();
            SaveLoadSystem.Save(PlayerData);
        }


        public bool IsChapterUnlocked(ChapterType chapterType)
        {
            return PlayerData.IsChapterUnlocked(chapterType);
        }


        public void SetChapter(ChapterType chapterType)
        {
            PlayerData.currentChapter = chapterType;
            SaveLoadSystem.Save(PlayerData);
        }


        public void UnlockChapter(ChapterType chapterType)
        {
            PlayerData.UnlockChapter(chapterType);
            SaveLoadSystem.Save(PlayerData);
        }


        public void UpdateCarLevel(DrivingProfileType profileType)
        {
            PlayerData.UpdateCarLevel(profileType);
            SaveLoadSystem.Save(PlayerData);
            DrivingProfileManager.UpdateDrivingProfileLevels();
        }


        public int GetDrivingProfileLevelFor(DrivingProfileType profileType)
        {
            return PlayerData.GetDrivingProfileLevelFor(profileType);
        }


        public void UpdateSkillLevel(SkillType skillType)
        {
            PlayerData.UpdateSkillLevel(skillType);
            SaveLoadSystem.Save(PlayerData);
        }


        public int GetSkillLevelFor(SkillType skillType)
        {
            return PlayerData.GetSkillLevelFor(skillType);
        }


        public bool IsSkillUnlocked(SkillType skillType)
        {
            return PlayerData.IsSkillUnlocked(skillType);
        }


        public void UnlockSkill(SkillType skillType)
        {
            PlayerData.UnlockSkill(skillType);
            SaveLoadSystem.Save(PlayerData);
        }


        public void ControlVibration(bool set)
        {
            if (set)
            {
                PlayerData.EnableVibration();
            }
            else
            {
                PlayerData.DisableVibration();
            }
            
            SaveLoadSystem.Save(PlayerData);
        }


        public void ControlSound(bool set)
        {
            if (set)
            {
                PlayerData.EnableSound();
                SoundManager.Instance.PlayThemeSong();
            }
            else
            {
                PlayerData.DisableSound();
                SoundManager.Instance.StopAll();
            }
            
            SaveLoadSystem.Save(PlayerData);
        }
        
        public void ControlMusic(bool set)
        {
            if (set)
            {
                PlayerData.EnableMusic();
                SoundManager.Instance.PlayThemeSong();
            }
            else
            {
                PlayerData.DisableMusic();
                SoundManager.Instance.StopThemeSong();
            }
            
            SaveLoadSystem.Save(PlayerData);
        }
        
        public static bool IsInitialLaunch()
        {
            return !PlayerPrefs.HasKey(Constants.AppInstallTimeStampKey);
        }

        private static void SaveAppInstallTimeStamp()
        {
            if (!IsInitialLaunch())
            {
                return;
            }
        
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            PlayerPrefs.SetString(Constants.AppInstallTimeStampKey, timestamp.ToString());
        }
    }
}