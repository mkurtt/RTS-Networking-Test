using System;
using System.Reflection;
using Game.Scripts.Networking;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace Game.Scripts.Buildings
{
    public class BuildingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Building _building;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private LayerMask floorMask;

        private Camera _cam;
        private RTSPlayer _player;
        private GameObject _buildingPreviewInstance;
        private Renderer _buildingRendererInstance;

        private void Start()
        {
            _cam = Camera.main;

            _icon.sprite = _building.GetIcon;
            _priceText.text = _building.GetPrice.ToString();
        }

        private void Update()
        {
            if (!_player)
            {
                _player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
            }

            if (_buildingPreviewInstance)
            {
                UpdateBuildingPreview();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            _buildingPreviewInstance = Instantiate(_building.GetBuildingPreview);
            _buildingRendererInstance = _buildingPreviewInstance.GetComponentInChildren<Renderer>();
            _buildingPreviewInstance.SetActive(false);

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_buildingPreviewInstance) return;

            var ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, floorMask))
            {
                _player.CmdTryPlaceBuilding(_building.GetId, hit.point);
            }

            Destroy(_buildingPreviewInstance);
        }

        private void UpdateBuildingPreview()
        {
            var ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, floorMask)) return;

            _buildingPreviewInstance.transform.position = hit.point;

            if (!_buildingPreviewInstance.activeSelf)
            {
                _buildingPreviewInstance.SetActive(true);
            }
        }
    }
}
