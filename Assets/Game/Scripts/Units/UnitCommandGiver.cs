using UnityEngine;
using UnityEngine.InputSystem;
namespace Game.Scripts.Units
{
    public class UnitCommandGiver : MonoBehaviour
    {
        [SerializeField] private UnitSelectionHandler _unitSelectionHandler;
        [SerializeField] private LayerMask layerMask;

        private Camera _cam;
        
        private void Start()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out var hit ,Mathf.Infinity, layerMask))
                {
                    TryMove(hit.point);
                }
            }
        }
        
        private void TryMove(Vector3 hitInfoPoint)
        {
            foreach (var unit in _unitSelectionHandler.SelectedUnits)
            {
                unit.GetUnitMovement.CmdMove(hitInfoPoint);
            }
        }
    }
}
