using System;
using Game.Scripts.Buildings;
using Game.Scripts.Combat;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Units
{
    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Targeter _targeter;
        [SerializeField] private float _chaseRange;

#region Server
        public override void OnStartServer()
        {
            GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
        }

        public override void OnStopServer()
        {
            GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
        }

        [ServerCallback]
        private void Update()
        {
            var target = _targeter.GetTarget;
            
            if (target)
            {
                if ((target.transform.position - transform.position).sqrMagnitude > _chaseRange*_chaseRange )
                {
                    _agent.SetDestination(target.transform.position);
                }
                else if (_agent.hasPath)
                {
                    _agent.ResetPath();
                }
                
                return;
            }
            if (!_agent.hasPath) return;
            if (_agent.remainingDistance > _agent.stoppingDistance) return;

            _agent.ResetPath();
        }

        [Command]
        public void CmdMove(Vector3 targetPosition)
        {
            _targeter.ClearTarget();

            if (!NavMesh.SamplePosition(targetPosition, out var hit, 1f, NavMesh.AllAreas)) return;

            _agent.destination = hit.position;
        }
        
        [Server]
        private void ServerHandleGameOver()
        {
            _agent.ResetPath();
        }
#endregion


    }
}
