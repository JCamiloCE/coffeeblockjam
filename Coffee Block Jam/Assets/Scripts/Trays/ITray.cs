using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public interface ITray
    {
        public void SetDataForTray(List<int> indexInGrid, Color color);
        public void CreateVisualTray(Transform parent);
    }
}