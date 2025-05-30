using CoffeeBlockJam.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public class TrayBuilder 
    {
        public List<ITray> CreateTrays(GridDataJson dataInJson, List<ITraySection> traySections, Transform parent)
        {
            GameObject TraysParent = new GameObject("Trays");
            TraysParent.transform.SetParent(parent);
            Dictionary<(int, Color), ITray> trays = new();
            foreach (ITraySection traySection in traySections)
            {
                var key = (traySection.GetIdTray(), traySection.GetColorTray());
                if (!trays.ContainsKey(key)) 
                {
                    ITray newTray = CreateNewTray(dataInJson, key, TraysParent.transform);
                    trays[key] = newTray;
                }
                trays[key].AddTraySection(traySection);
            }
            return null;
        }

        private ITray CreateNewTray(GridDataJson dataInJson, (int, Color) key, Transform parent) 
        {
            GameObject tray = new GameObject("Tray_" + key.Item1 + "_" + key.Item2);
            tray.transform.SetParent(parent);
            ITray trayImpl = tray.AddComponent<TrayImpl>();
            trayImpl.Initialize(dataInJson);
            return trayImpl;
        }
    }
}