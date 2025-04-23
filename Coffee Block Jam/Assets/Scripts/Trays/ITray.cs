using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public interface ITray
    {
        public void Initialize();
        public void AddTraySection(ITraySection traySection);
        public void PrepareToMove(Vector3 initialPosition);
        public void EndToMove();
        public void MoveToTouchPos(Vector3 screenPosition);
    }
}