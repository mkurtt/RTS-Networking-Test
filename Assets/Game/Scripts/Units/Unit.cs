using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
namespace Game.Scripts.Units
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField] private UnitMovement _unitMovement;
        
        [SerializeField] private UnityEvent _onSelected;
        [SerializeField] private UnityEvent _onDeselected;

        public static event Action<Unit> ServerOnUnitSpawned;
        public static event Action<Unit> ServerOnUnitDespawned;
        
        public static event Action<Unit> ClientOnUnitSpawned;
        public static event Action<Unit> ClientOnUnitDespawned;

        public UnitMovement GetUnitMovement => _unitMovement;

        #region Server
        public override void OnStartServer()
        {
            ServerOnUnitSpawned?.Invoke(this);
        }

        public override void OnStopServer()
        {
            ServerOnUnitDespawned?.Invoke(this);
        }
  #endregion
        
        #region Client
        
        public override void OnStartClient()
        {
            if (!isClientOnly || !hasAuthority) return;
            
            ClientOnUnitSpawned?.Invoke(this);
        }

        public override void OnStopClient()
        {
            if (!isClientOnly || !hasAuthority) return;
            
            ClientOnUnitDespawned?.Invoke(this);
        }
        
        [Client]
        public void Select()
        {
            if (!hasAuthority) return;
            _onSelected?.Invoke();
        }
        
        [Client]
        public void Deselect()
        {
            if (!hasAuthority) return;
            _onDeselected?.Invoke();
        }
        
  #endregion

    }
}
