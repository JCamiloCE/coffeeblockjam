using Enums;
using UnityEngine;

namespace CoffeeBlockJam.Map.Editor
{ 
    [System.Serializable]
    public class GridMarkEditor
    {
        public Vector2Int position;
        public int id;
        public Color color;

        public GridMarkEditor(Vector2Int position, EColorTray color, int id) 
        {
            this.position = position;
            this.id = id;
            this.color = EnumConverter.GetUnityColor(color);
        }
    }
}

