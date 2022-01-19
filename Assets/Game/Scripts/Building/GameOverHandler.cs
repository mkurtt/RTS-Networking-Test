using System.Collections.Generic;
using Mirror;
using UnityEngine;
namespace Game.Scripts.Building
{
    public class GameOverHandler : NetworkBehaviour
    {
        private List<UnitBase> _bases = new List<UnitBase>();
        
        #region Server
        public override void OnStartServer()
        {
            UnitBase.ServerOnBaseSpawned += ServerHandleBaseSpawned;
            UnitBase.ServerOnBaseDespawned += ServerHandleBaseSpawned;
        }

        public override void OnStopServer()
        {
            UnitBase.ServerOnBaseSpawned -= ServerHandleBaseSpawned;
            UnitBase.ServerOnBaseDespawned -= ServerHandleBaseSpawned;
        }

        [Server]
        private void ServerHandleBaseSpawned(UnitBase unitBase)
        {
            _bases.Add(unitBase);
        }
        
        [Server]
        private void ServerHandleBaseDespawned(UnitBase unitBase)
        {
            _bases.Remove(unitBase);

            if (_bases.Count != 1) return;
            
            Debug.Log("GameOver");
        }
  #endregion
    }
}
