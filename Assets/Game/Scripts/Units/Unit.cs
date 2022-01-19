using System;
using Game.Scripts.Combat;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
namespace Game.Scripts.Units
{
    public class Unit : NetworkBehaviour
    {
        [SerializeField] private UnitMovement _unitMovement;
        [SerializeField] private Targeter _targeter;
        [SerializeField] private Health _health;
        
        [SerializeField] private UnityEvent _onSelected;
        [SerializeField] private UnityEvent _onDeselected;

        public static event Action<Unit> ServerOnUnitSpawned;
        public static event Action<Unit> ServerOnUnitDespawned;
        
        public static event Action<Unit> ClientOnUnitSpawned;
        public static event Action<Unit> ClientOnUnitDespawned;

        public UnitMovement GetUnitMovement => _unitMovement;
        public Targeter GetTargeter => _targeter;

        #region Server
        public override void OnStartServer()
        {
            ServerOnUnitSpawned?.Invoke(this);
            _health.ServerOnDie += ServerHandleDie;
        }

        public override void OnStopServer()
        {
            ServerOnUnitDespawned?.Invoke(this);
            _health.ServerOnDie -= ServerHandleDie;
        }
        
        [Server]
        private void ServerHandleDie()
        {
            NetworkServer.Destroy(gameObject);
        }
        
  #endregion
        
        #region Client
        
        public override void OnStartAuthority()
        {
            ClientOnUnitSpawned?.Invoke(this);
        }

        public override void OnStopClient()
        {
            if (!hasAuthority) return;
            
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
