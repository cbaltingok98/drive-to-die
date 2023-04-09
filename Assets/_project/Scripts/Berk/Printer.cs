using UnityEngine;

namespace _project.Scripts.Berk
{
    public enum DesiredColor
    {
        Black,
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Purple,
        Cyan,
        Magenta,
        Olive,
        DarkGreen,
        DarkRed,
        DarkOrange,
        Gold,
        Violet,
        LightBlue
    }
    
    public class Printer : MonoBehaviour
    {
        public static void Print(string text, DesiredColor color = DesiredColor.Black)
        {
            if (!Constants.IsPrintOn) return;

            print(color == DesiredColor.Black ? text : GetColoredText(text, GetColor(color)));
        }
        
        private static string GetColoredText(string text, string color)
        {
            return "<color=" + color + ">" + text + "</color>"; 
        }
        
        private static string GetColor(DesiredColor color)
        {
            switch (color)
            {
                case DesiredColor.Red:
                    return "red";
                case DesiredColor.Green:
                    return "green";
                case DesiredColor.Blue:
                    return "blue";
                case DesiredColor.Yellow:
                    return "yellow";
                case DesiredColor.Orange:
                    return "orange";
                case DesiredColor.Purple:
                    return "purple";
                case DesiredColor.Cyan:
                    return "cyan";
                case DesiredColor.Magenta:
                    return "magenta";
                case DesiredColor.Olive:
                    return "olive";
                case DesiredColor.DarkGreen:
                    return "darkgreen";
                case DesiredColor.DarkRed:
                    return "darkred";
                case DesiredColor.DarkOrange:
                    return "darkorange";
                case DesiredColor.Gold:
                    return "gold";
                case DesiredColor.Violet:
                    return "violet";
                case DesiredColor.LightBlue:
                    return "lightblue";
                default:
                    return "black";
            }
        }
    }
}