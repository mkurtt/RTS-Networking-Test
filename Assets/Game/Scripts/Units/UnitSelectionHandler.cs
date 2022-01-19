using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Networking;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
namespace Game.Scripts.Units
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform _unitSelectionArea;
        [SerializeField] private LayerMask _layerMask;

        private Vector2 _startPos;

        private RTSPlayer _player;
        private Camera _cam;

        public List<Unit> SelectedUnits = new List<Unit>();

        private void Start()
        {
            _cam = Camera.main;

            Unit.ClientOnUnitDespawned += ClientHandleUnitDespawned;
        }

        private void OnDestroy()
        {
            Unit.ClientOnUnitDespawned -= ClientHandleUnitDespawned;
        }

        private void Update()
        {
            if (!_player)
            {
                _player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
            }
            
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartSelectionArea();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearSelectionArea();
            }
            else if (Mouse.current.leftButton.isPressed)
            {
                UpdateSelectionArea();
            }
        }


        private void StartSelectionArea()
        {
            if (!Keyboard.current.leftShiftKey.isPressed)
            {
                foreach (var selectedunit in SelectedUnits)
                {
                    selectedunit.Deselect();
                }
                SelectedUnits.Clear();
            }

            _unitSelectionArea.gameObject.SetActive(true);

            _startPos = Mouse.current.position.ReadValue();

            UpdateSelectionArea();
        }

        private void UpdateSelectionArea()
        {
            var mousePos = Mouse.current.position.ReadValue();

            var width = mousePos.x - _startPos.x;
            var height = mousePos.y - _startPos.y;

            _unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            _unitSelectionArea.anchoredPosition = _startPos + new Vector2(width / 2, height / 2);
        }


        private void ClearSelectionArea()
        {
            _unitSelectionArea.gameObject.SetActive(false);

            if (_unitSelectionArea.sizeDelta.magnitude == 0)
            {
                var ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _layerMask))
                {
                    if (hit.collider.TryGetComponent<Unit>(out var unit))
                    {
                        if (!unit.hasAuthority) return;

                        SelectedUnits.Add(unit);
                        foreach (var selectedunit in SelectedUnits)
                        {
                            selectedunit.Select();
                        }
                    }
                }
                return;
            }

            Vector2 min = _unitSelectionArea.anchoredPosition - (_unitSelectionArea.sizeDelta / 2);
            Vector2 max = _unitSelectionArea.anchoredPosition + (_unitSelectionArea.sizeDelta / 2);

            foreach (var unit in _player.GetMyUnits)
            {
                if (SelectedUnits.Contains(unit)) continue;

                Vector3 screenPos = _cam.WorldToScreenPoint(unit.transform.position);
                if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
                {
                    SelectedUnits.Add(unit);
                    unit.Select();
                }
            }
        }
        
        private void ClientHandleUnitDespawned(Unit unit)
        {
            SelectedUnits.Remove(unit);
        }
    }
}
