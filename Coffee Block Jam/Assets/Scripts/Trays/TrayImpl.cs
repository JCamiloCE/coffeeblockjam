using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public class TrayImpl : MonoBehaviour, ITray
    {
        private List<ITraySection> _traySections = null;
        private Rigidbody _rigidbody = null;

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
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            _rigidbody.useGravity = false;
        }
    }
}