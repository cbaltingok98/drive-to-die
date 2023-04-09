using _project.Scripts.Enums;

namespace _project.Scripts.Berk
{
    public static class Constants
    {
        public const bool IsPrintOn = true; // TODO false
        public const bool IsDebugOn = true; // TODO false
        
        public static bool CanVibrate;
        public static bool CanPlaySound;
        public static bool CanPlayMusic;
        
        public const CarModels InitialCarModel = CarModels.CamaroZombie;
        public const ChapterType InitialChapterType = ChapterType.Chapter1;
        public const CharacterType InitialCharacter = CharacterType.Punk;
        
        public const int InitialCoinAmount = 1000000; // 1,000
        public const int InitialStarLevel = 0;

        public const int MimicDataFolderAmount = 1;
        
        public const string AppInstallTimeStampKey = "appInstallTimeStamp";
    }
}