using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public class TrayImpl : MonoBehaviour, ITray
    {
        private List<ITraySection> _traySections = null;
        private Rigidbody _rigidbody = null;
        [SerializeField] private float _speed = 3f;
        private Vector3 _delta = Vector3.zero;

        //Temp
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>(); 
        }

        void ITray.Initialize()
        {
            _traySections = new();
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            _rigidbody.useGravity = false;
        }

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

        void ITray.PrepareToMove(Vector3 initialPosition)
        {
            _rigidbody.isKinematic = false;
            _delta = transform.position - initialPosition;
        }

        void ITray.EndToMove()
        {
            _rigidbody.isKinematic = true;
        }

        void ITray.MoveToTouchPos(Vector3 _screenPosition)
        {
            Vector3 target = Camera.main.WorldToScreenPoint(transform.position - _delta);
            Vector3 direction = (_screenPosition - target).normalized;
            _rigidbody.velocity = (direction * _speed);
        }
    }
}