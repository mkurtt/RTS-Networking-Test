using System;
using System.Collections.Generic;
using Game.Scripts.Units;
using Mirror;
namespace Game.Scripts.Networking
{
    public class RTSPlayer : NetworkBehaviour
    {
        private List<Unit> _myUnits = new List<Unit>();

        public List<Unit> GetMyUnits => _myUnits;
        
        #region Server
        
        public override void OnStartAuthority()
        {
            if (NetworkServer.active) return;
            
            Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
            Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;
        }
        
        public override void OnStopServer()
        {
            if (NetworkServer.active) return;
            
            Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
            Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;
        }
        private void ServerHandleUnitSpawned(Unit unit)
        {
            _myUnits.Add(unit);
        }
        
        private void ServerHandleUnitDespawned(Unit unit)
        {
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
