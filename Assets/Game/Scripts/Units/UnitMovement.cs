using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Units
{
    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;

#region Server
        
        [Command]
        public void CmdMove(Vector3 targetPosition)
        {
            if (!NavMesh.SamplePosition(targetPosition, out var hit, 1f, NavMesh.AllAreas)) return;

            _agent.destination = hit.position;
        }
        
#endregion
        

    }
}
