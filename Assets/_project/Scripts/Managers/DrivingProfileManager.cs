using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Berk;
using _project.Scripts.Car;
using _project.Scripts.Data;
using _project.Scripts.Enums;
using TMPro;

namespace _project.Scripts.Managers
{
    public static class DrivingProfileManager
    {
        private static List<DrivingProfile> drivingProfilesList;

        private static bool isInitialized;

        private static void InitializeList()
        {
            if (isInitialized) return;
            
            drivingProfilesList = new List<DrivingProfile>();
            drivingProfilesList = DataManager.GetDrivingProfiles().ToList();
            isInitialized = true;
            //UpdateDrivingProfileLevels();
        }

        public static DrivingProfile GetDrivingProfile(DrivingProfileType drivingProfileType)
        {
            InitializeList();
            return drivingProfilesList.First(profile => profile.profileType == drivingProfileType);
        }


        public static void UpdateDrivingProfileLevels()
        {
            InitializeList();
            for (var i = 0; i < drivingProfilesList.Count; i++)
            {
                var level = DataManager.Instance.GetDrivingProfileLevelFor(drivingProfilesList[i].profileType);
                drivingProfilesList[i].level = level;
            }
        }


        public static bool IsCarMaxedOut(DrivingProfileType drivingProfileType)
        {
            var profile = drivingProfilesList.First(profile => profile.profileType == drivingProfileType);
            return profile.level >= profile.drivingStatsList.Count;
        }


        public static int GetUpgradePriceFor(DrivingProfileType drivingProfileType)
        {
            var profile = drivingProfilesList.First(profile => profile.profileType == drivingProfileType);
            var level = profile.level;
            return profile.upgradePrices[level - 1];
        }


        public static List<DrivingProfile> GetDrivingProfiles()
        {
            InitializeList();
            return drivingProfilesList;
        }

    }
}