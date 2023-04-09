using DG.Tweening;
using TMPro;

namespace _project.Scripts.Utils
{
    public static class Utilities
    {
        public static string GetTimeFromSecondsToFormattedText(int seconds)
        {
            var minutes = seconds / 60;
            var remainder = seconds - (minutes * 60);

            string timeString = "";

            if (minutes < 10)
            {
                timeString += "0" + minutes;
            }
            else
            {
                timeString += minutes;
            }

            timeString += ":";
            
            if (remainder < 10)
            {
                timeString += "0" + remainder;
            }
            else
            {
                timeString += remainder;
            }

            return timeString;
        }


        public static string FormatPriceToString(int value)
        {
            return value.ToString("#,##0");
        }
    }
}