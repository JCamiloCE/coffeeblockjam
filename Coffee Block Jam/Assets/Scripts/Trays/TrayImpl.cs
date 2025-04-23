using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public class TrayImpl : MonoBehaviour, ITray
    {
        private List<ITraySection> _traySections = null;

        void ITray.AddTraySection(ITraySection traySection)
        {
            if (_traySections.Contains(traySection)) 
            {
                Debug.LogError("Trying to add duplicate TraySection");
                return;
            }
            traySection.SetParent(transform);
            _traySections.Add(traySection);
        }

        void ITray.Initialize()
        {
            _traySections = new();
        }
    }
}