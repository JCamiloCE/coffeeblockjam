using Enums;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public interface ITraySection
    {
        public ETypeTray GetTypeTray();
        public void SetTraySectionData(Color colorTraySection, int traySectionId);
    }
}