using System;
using Game.Scripts.Combat;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Game.Scripts.Building
{
    public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
    {
        [SerializeField] private Health _health;
        [SerializeField] private GameObject _unitPrefab;
        [SerializeField] private Transform _spawnPoint;

        #region Server
        public override void OnStartServer()
        {
            _health.ServerOnDie += ServerHandleDie;
        }

        public override void OnStopServer()
        {
            _health.ServerOnDie -= ServerHandleDie;
        }
        
        [Server]
        private void ServerHandleDie()
        {
            //NetworkServer.Destroy(gameObject);
        }

        [Command]
        private void CmdSpawnUnit()
        {
            var unitInstance = Instantiate(_unitPrefab,_spawnPoint.position,_spawnPoint.rotation);
            
            NetworkServer.Spawn(unitInstance, connectionToClient);
        }
        
        #endregion

        #region Client
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            if (!hasAuthority) return;
            
            CmdSpawnUnit();
        }
        
  #endregion
        
        
        
    }
}
