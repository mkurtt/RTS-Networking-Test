using System;
using System.Collections.Generic;
using Game.Scripts.Units;
using Mirror;
namespace Game.Scripts.Networking
{
    public class RTSPlayer : NetworkBehaviour
    {
        private List<Unit> _myUnits = new List<Unit>();

        #region Server
        
        
        public override void OnStartServer()
        {
            Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
            Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
        }
        
        public override void OnStopServer()
        {
            Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
            Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
        }
        private void ServerHandleUnitSpawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;
            
            _myUnits.Add(unit);
        }
        
        private void ServerHandleUnitDespawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;
            
            _myUnits.Remove(unit);
        }
  #endregion

        #region Client
        
        public override void OnStartClient()
        {
            if (!isClientOnly) return;
           
            Unit.ClientOnUnitSpawned += ClientHandleUnitSpawned;
            Unit.ClientOnUnitDespawned += ClientHandleUnitDespawned;
        }
        
        public override void OnStopClient()
        {
            if (!isClientOnly) return;

            Unit.ClientOnUnitSpawned -= ClientHandleUnitSpawned;
            Unit.ClientOnUnitDespawned -= ClientHandleUnitDespawned;
        }
        private void ClientHandleUnitSpawned(Unit unit)
        {
            if (!hasAuthority) return;
                
            _myUnits.Add(unit);
        }
        
        private void ClientHandleUnitDespawned(Unit unit)
        {
            if (!hasAuthority) return;
            
            _myUnits.Remove(unit);
        }
        
  #endregion
    }
}
