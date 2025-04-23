using Enums;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public class TraySectionImpl : MonoBehaviour, ITraySection
    {
        [SerializeField] private ETypeTray _typeTray = ETypeTray.None;
        [SerializeField] private MeshRenderer _meshRender = null;

        private Color _traySectionColor = Color.white;
        private int _traySectionId = -1;

        ETypeTray ITraySection.GetTypeTray() => _typeTray;

        void ITraySection.SetTraySectionData(Color colorTraySection, int traySectionId)
        {
            _traySectionColor = colorTraySection;
            _traySectionId = traySectionId;

            for (int i = 0; i < _meshRender.materials.Length; i++)
            {
                _meshRender.materials[i].color = _traySectionColor;
            }
        }
    }
}