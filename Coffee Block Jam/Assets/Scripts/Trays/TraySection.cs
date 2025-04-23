using Enums;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public class TraySection : MonoBehaviour, ITraySection
    {
        [SerializeField] private ETypeTray _typeTray = ETypeTray.None;

        ETypeTray ITraySection.GetTypeTray() => _typeTray;
    }
}