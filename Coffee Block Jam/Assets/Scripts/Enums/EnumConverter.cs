using UnityEngine;

namespace Enums
{
    public static class EnumConverter
    {
        public static Color GetUnityColor(EColorTray colorTray) 
        {
            switch (colorTray)
            {
                case EColorTray.Blue:
                    return Color.blue;
                case EColorTray.Red:
                    return Color.red;
                case EColorTray.Yellow:
                    return Color.yellow;
            }

            Debug.LogError("Color not supported");
            return Color.white;
        }
    }
}