using System;
using Game.Scripts.Buildings;
using Mirror;
using UnityEngine;
namespace Game.Scripts.Combat
{
    public class Health : NetworkBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        [SyncVar(hook = nameof(HandleHealthUpdated))]
        private int _currentHealth;

        public event Action ServerOnDie;
        public event Action<int, int> ClientOnHealthUpdated;        
        
        #region Server
        public override void OnStartServer()
        {
            _currentHealth = maxHealth;
            UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;
        }

        public override void OnStopServer()
        {
            UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;
        }
        
        private void ServerHandlePlayerDie(int connectionId)
        {
            if (connectionToClient.connectionId != connectionId) return;
            
            DealDamage(_currentHealth+1);
        }

        [Server]
        public void DealDamage(int damageAmount)
        {
            if (_currentHealth == 0) return;

            _currentHealth = Mathf.Max(_currentHealth - damageAmount, 0);

            if (_currentHealth != 0) return;
            
            ServerOnDie?.Invoke();
        }
  #endregion

        #region Client

        private void HandleHealthUpdated(int oldHealth, int newHealth)
        {
            ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
        }
        
  #endregion
    }
}
