using System;
using Mirror;
using UnityEngine;
namespace Game.Scripts.Buildings
{
    public class Building : NetworkBehaviour
    {
        [SerializeField] private GameObject _buildingPreview;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _id = -1;
        [SerializeField] private int _price = 100;
        
        public static event Action<Building> ServerOnBuildingSpawned;
        public static event Action<Building> ServerOnBuildingDespawned;
        
        public static event Action<Building> ClientOnBuildingSpawned;
        public static event Action<Building> ClientOnBuildingDespawned;

        public GameObject GetBuildingPreview => _buildingPreview;
        public Sprite GetIcon => _icon;
        public int GetId => _id;
        public int GetPrice => _price;

        #region Server
        public override void OnStartServer()
        {
            ServerOnBuildingSpawned?.Invoke(this);
        }
        
        public override void OnStopServer()
        {
            ServerOnBuildingDespawned?.Invoke(this);
        }
  #endregion

        #region Client
        public override void OnStartAuthority()
        {
            ClientOnBuildingSpawned?.Invoke(this);
        }
        
        public override void OnStopClient()
        {
            if (!hasAuthority) return;
            
            ClientOnBuildingDespawned?.Invoke(this);
        }
  #endregion
        
    }
}
