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
        Color ITraySection.GetColorTray() => _traySectionColor;
        int ITraySection.GetIdTray() => _traySectionId;
        Vector3 ITraySection.GetPosition() => transform.position;

        void ITraySection.SetTraySectionData(Color colorTraySection, int traySectionId)
        {
            _traySectionColor = colorTraySection;
            _traySectionId = traySectionId;

            if (Application.isPlaying) 
            {
                for (int i = 0; i < _meshRender.materials.Length; i++)
                {
                    _meshRender.materials[i].color = _traySectionColor;
                }
            }
        }

        void ITraySection.SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
    }
}