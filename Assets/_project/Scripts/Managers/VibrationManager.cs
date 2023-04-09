using _project.Scripts.Berk;
using Lofelt.NiceVibrations;


namespace _project.Scripts.Managers
{
    public static class VibrationManager
    {
        public static void LightVibrate()
        {
            if (!Constants.CanVibrate) return;
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }
        
        public static void SoftVibrate()
        {
            if (!Constants.CanVibrate) return;
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
        }
        
        public static void MediumVibrate()
        {
            if (!Constants.CanVibrate) return;
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
        
        public static void HeavyVibrate()
        {
            if (!Constants.CanVibrate) return;
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
        }
        
        public static void UiClick()
        {
            if (!Constants.CanVibrate) return;
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
        }
    }
}