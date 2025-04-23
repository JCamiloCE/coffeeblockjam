using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public interface ITray
    {
        public void Initialize();
        public void AddTraySection(ITraySection traySection);
    }
}