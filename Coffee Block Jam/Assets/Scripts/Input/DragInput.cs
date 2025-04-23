using CoffeeBlockJam.Trays;
using UnityEngine;

namespace CoffeeBlockJam.Inputs
{
    public class DragInput : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundMask;

        private bool _isDragging = false;
        private Vector2 _screenPosition = Vector2.one;
        private ITray _currentTray = null;

        void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                _currentTray.EndToMove();
                _currentTray = null;
            }

            if (_isDragging)
            {
                _screenPosition = Input.mousePosition;
                if (Input.touchCount > 0)
                {
                    _screenPosition = Input.GetTouch(0).position;
                }

                if (_currentTray == null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(_screenPosition);
                    if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _groundMask))
                    {
                        _currentTray = hitInfo.rigidbody.gameObject.GetComponent<ITray>();
                        _currentTray.PrepareToMove(hitInfo.point);
                    }
                }
                else {
                    _currentTray.MoveToTouchPos(_screenPosition);
                    //_currentTray.MoveToTouchPos(_screenPosition);
                }
            }
        }
    }
}