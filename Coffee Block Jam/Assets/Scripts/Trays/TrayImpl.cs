using CoffeeBlockJam.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public class TrayImpl : MonoBehaviour, ITray
    {
        private List<ITraySection> _traySections = null;
        private Rigidbody _rigidbody = null;
        private float _speed = 25f;
        private float _speedFit = 10f;
        private Vector3 _delta = Vector3.zero;
        private List<Vector3> _gridPositions = null;
        private Coroutine _fitCoroutine = null;

        void ITray.Initialize(GridDataJson gridData)
        {
            SetGridPositions(gridData);
            _traySections = new();
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.mass = 1000f;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            _rigidbody.useGravity = false;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
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
            if (_fitCoroutine != null)
            {
                StopCoroutine(_fitCoroutine);
                _fitCoroutine = null;
            }
            _rigidbody.isKinematic = false;
            _rigidbody.mass = 0.1f;
            _delta = transform.position - initialPosition;
        }

        void ITray.EndToMove(Vector2 endPos)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.mass = 1000f;
            FitTray(endPos);
        }

        void ITray.MoveToTouchPos(Vector3 screenPosition)
        {
            Vector3 target = Camera.main.WorldToScreenPoint(transform.position - _delta);
            Vector3 direction = (screenPosition - target);
            float distance = Vector3.Distance(screenPosition, target);
            direction = new Vector3(direction.x, direction.y, 0f);
            direction = direction.normalized;
            _rigidbody.velocity = (direction * GetSpeed(distance));
        }

        private void SetGridPositions(GridDataJson gridData) 
        {
            _gridPositions = new List<Vector3>();

            for (int y = 0; y < gridData.height; y++)
            {
                for (int x = 0; x < gridData.width; x++)
                {
                    _gridPositions.Add(new Vector3(x * gridData.offsetX, -y * gridData.offsetY, 0f));
                }
            }
        }

        private float GetSpeed(float distance) 
        {
            if (distance < 10) return 0f;
            if (distance < 20) return _speed / 5f;
            if (distance < 40) return _speed / 3f;
            if (distance < 60) return _speed / 2f;
            if (distance < 80) return _speed / 1.5f;
            return _speed;
        }

        private void FitTray(Vector2 endPos) 
        {
            if (_fitCoroutine != null)
            {
                StopCoroutine(_fitCoroutine);
                _fitCoroutine = null;
            }
            ITraySection tray = GetNearTraySection(endPos);
            Vector3 gridPos = GetNearPosInGrid(tray.GetPosition());
            gridPos = new Vector3(gridPos.x, gridPos.y, tray.GetPosition().z);
            Vector3 direction = (gridPos - tray.GetPosition()).normalized;
            _fitCoroutine = StartCoroutine(MoveTray(direction, tray, gridPos));
        }


        private ITraySection GetNearTraySection(Vector2 endPosScreen) 
        {
            ITraySection closest = null;
            float minDistanceSqr = float.MaxValue;
            Vector3 endPosWorld = (Vector3)endPosScreen;
            foreach (var traySection in _traySections)
            {
                float distanceSqr = (traySection.GetPosition() - endPosWorld).sqrMagnitude;
                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    closest = traySection;
                }
            }
            return closest;
        }

        private Vector3 GetNearPosInGrid(Vector3 trayPos)
        {
            Vector3 closest = Vector3.zero;
            float minDistanceSqr = float.MaxValue;
            foreach (var gridPos in _gridPositions)
            {
                float distanceSqr = (gridPos - trayPos).sqrMagnitude;
                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    closest = gridPos;
                }
            }
            return closest;
        }

        private IEnumerator MoveTray(Vector3 direction, ITraySection traySelected, Vector3 gridPos)
        {
            float distance = Vector3.Distance(traySelected.GetPosition(), gridPos);
            float lastDistance = distance;
            while (distance > 0.05f)
            {
                transform.position += direction * GetSpeedForFit(distance, _speedFit) * Time.deltaTime;
                lastDistance = distance;
                distance = Vector3.Distance(traySelected.GetPosition(), gridPos);
                if (distance > lastDistance) 
                {
                    direction = -direction;
                }
                yield return null;
            }
        }

        private float GetSpeedForFit(float distance, float baseSpeed)
        {
            if (distance < 0.05f) return 0f;
            if (distance < 0.1f) return baseSpeed / 20f;
            if (distance < 0.2f) return baseSpeed / 10f;
            if (distance < 0.4f) return baseSpeed / 5f;
            if (distance < 0.6f) return baseSpeed / 3f;
            if (distance < 0.8f) return baseSpeed / 2f;
            if (distance < 1f) return baseSpeed / 1.5f;
            return _speed;
        }
    }
}