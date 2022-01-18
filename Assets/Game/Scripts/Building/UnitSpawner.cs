using System;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Game.Scripts.Building
{
    public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _unitPrefab;
        [SerializeField] private Transform _spawnPoint;

        #region Server

        [Command]
        private void CmdSpawnUnit()
        {
            var unitInstance = Instantiate(_unitPrefab,_spawnPoint.position,_spawnPoint.rotation);
            
            NetworkServer.Spawn(unitInstance, connectionToClient);
        }
        
  #endregion

        #region Client
        
        
        
  #endregion
        
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            if (!hasAuthority) return;
            
            CmdSpawnUnit();
        }
    }
}
