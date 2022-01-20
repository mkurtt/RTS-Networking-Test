using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Buildings;
using Game.Scripts.Units;
using Mirror;
using UnityEngine;
namespace Game.Scripts.Networking
{
    public class RTSPlayer : NetworkBehaviour
    {
        [SerializeField] private Building[] _buildings = new Building[0];
        
        private List<Unit> _myUnits = new List<Unit>();
        private List<Building> _myBuildings = new List<Building>();

        public List<Unit> GetMyUnits => _myUnits;
        public List<Building> GetMyBuildings => _myBuildings;
        
        #region Server
        
        public override void OnStartServer()
        {
            Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
            Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
            Building.ServerOnBuildingSpawned += ServerHandleBuildingSpawned;
            Building.ServerOnBuildingDespawned += ServerHandleBuildingDespawned;
        }
        
        public override void OnStopServer()
        {
            if (NetworkServer.active) return;
            
            Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
            Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
            Building.ServerOnBuildingSpawned -= ServerHandleBuildingSpawned;
            Building.ServerOnBuildingDespawned -= ServerHandleBuildingDespawned;
        }

        [Command]
        public void CmdTryPlaceBuilding(int buldingId, Vector3 pos)
        {
            var buildingToPlace = _buildings.ToList().Find(x => x.GetId == buldingId);

            if (!buildingToPlace) return;

            var buildingInstance = Instantiate(buildingToPlace.gameObject, pos, buildingToPlace.transform.rotation);
            
            NetworkServer.Spawn(buildingInstance, connectionToClient);
        }
        
        private void ServerHandleBuildingSpawned(Building building)
        {
            if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
            
            _myBuildings.Add(building);
        }
        private void ServerHandleBuildingDespawned(Building building)
        {
            if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
            
            _myBuildings.Remove(building);
        }

        private void ServerHandleUnitSpawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

            _myUnits.Add(unit);
        }

        private void ServerHandleUnitDespawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

            _myUnits.Remove(unit);
        }

  #endregion

        #region Client
        
        public override void OnStartAuthority()
        {
            if (NetworkServer.active) return;
           
            Unit.ClientOnUnitSpawned += AuthorityHandleUnitSpawned;
            Unit.ClientOnUnitDespawned += AuthorityHandleUnitDespawned;
            Building.ClientOnBuildingSpawned += AuthorityHandleBuildingSpawned;
            Building.ClientOnBuildingDespawned += AuthorityHandleBuildingDespawned;
        }
        
        public override void OnStopClient()
        {
            if (!hasAuthority) return;

            Unit.ClientOnUnitSpawned -= AuthorityHandleUnitSpawned;
            Unit.ClientOnUnitDespawned -= AuthorityHandleUnitDespawned;
            Building.ClientOnBuildingSpawned -= AuthorityHandleBuildingSpawned;
            Building.ClientOnBuildingDespawned -= AuthorityHandleBuildingDespawned;
        }
        
        private void AuthorityHandleBuildingSpawned(Building building)
        {
            _myBuildings.Add(building);
        }
        
        private void AuthorityHandleBuildingDespawned(Building building)
        {
            _myBuildings.Remove(building);
        }
        
        private void AuthorityHandleUnitSpawned(Unit unit)
        {
            _myUnits.Add(unit);
        }
        
        private void AuthorityHandleUnitDespawned(Unit unit)
        {
            _myUnits.Remove(unit);
        }
        
  #endregion
    }
}
