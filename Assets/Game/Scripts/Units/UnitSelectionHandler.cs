using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
namespace Game.Scripts.Units
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        
        private Camera _cam;

        public List<Unit> SelectedUnits = new List<Unit>();

        private void Start()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                foreach (var selectedunit in SelectedUnits)
                {
                    selectedunit.Deselect();
                }
                SelectedUnits.Clear();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearSelectionArea();
            }
        }
        
        private void ClearSelectionArea()
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
        }
    }
}
