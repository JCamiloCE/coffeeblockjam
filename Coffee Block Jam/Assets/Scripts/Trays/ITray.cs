using CoffeeBlockJam.Grid;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public interface ITray
    {
        public void Initialize(GridDataJson gridData);
        public void AddTraySection(ITraySection traySection);
        public void PrepareToMove(Vector3 initialPosition);
        public void EndToMove(Vector2 endPos);
        public void MoveToTouchPos(Vector3 screenPosition);
    }
}