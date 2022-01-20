using System;
using Game.Scripts.Buildings;
using Game.Scripts.Combat;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Game.Scripts.Units
{
    public class 
        UnitCommandGiver : MonoBehaviour
    {
        [SerializeField] private UnitSelectionHandler _unitSelectionHandler;
        [SerializeField] private LayerMask layerMask;

        private Camera _cam;
        
        private void Start()
        {
            _cam = Camera.main;
            GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
        }

        private void OnDisable()
        {
            GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;
        }

        private void Update()
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out var hit ,Mathf.Infinity, layerMask))
                {
                    if (hit.collider.TryGetComponent<Targetable>(out Targetable target))
                    {
                        if (target.hasAuthority)
                        {
                            TryMove(hit.point);
                            return;
                        }
                        TryTarget(target);
                        return;
                    }
                    TryMove(hit.point);
                }
            }
        }
        
        private void TryTarget(Targetable target)
        {
            foreach (var unit in _unitSelectionHandler.SelectedUnits)
            {
                unit.GetTargeter.CmdSetTarget(target.gameObject);
            }
        }

        private void TryMove(Vector3 hitInfoPoint)
        {
            foreach (var unit in _unitSelectionHandler.SelectedUnits)
            {
                unit.GetUnitMovement.CmdMove(hitInfoPoint);
            }
        }
        
        private void ClientHandleGameOver(string winnerName)
        {
            enabled = false;
        }
    }
}
