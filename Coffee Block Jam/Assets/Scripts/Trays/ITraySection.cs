using Enums;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public interface ITraySection
    {
        public ETypeTray GetTypeTray();
        public Color GetColorTray();
        public int GetIdTray();
        public Vector3 GetPosition();
        public void SetTraySectionData(Color colorTraySection, int traySectionId);
        public void SetParent(Transform parent);
    }
}