using Enums;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBlockJam.Grid 
{
    [System.Serializable]
    public class GridDataJson
    {
        public int width;
        public int height;
        public float offsetX;
        public float offsetY;
        public List<CellDataJson> cellsData;
    }

    [System.Serializable]
    public class CellDataJson
    {
        public Vector2Int position;
        public int id;
        public Color color;

        public CellDataJson(Vector2Int position, EColorTray color, int id)
        {
            this.position = position;
            this.id = id;
            this.color = EnumConverter.GetUnityColor(color);
        }
    }
}

