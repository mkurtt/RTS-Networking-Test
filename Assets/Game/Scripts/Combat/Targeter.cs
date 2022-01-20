using Game.Scripts.Buildings;
using Mirror;
using UnityEngine;
namespace Game.Scripts.Combat
{
    public class Targeter : NetworkBehaviour
    {
        private Targetable _target;

        public Targetable GetTarget => _target;

        public override void OnStartServer()
        {
            GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
        }

        public override void OnStopServer()
        {
            GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
        }

        [Command]
        public void CmdSetTarget(GameObject targetGameObject)
        {
            if (!targetGameObject.TryGetComponent<Targetable>(out Targetable target)) return;

            _target = target;
        }

        [Server]
        public void ClearTarget()
        {
            _target = null;
        }
        
        [Server]
        private void ServerHandleGameOver()
        {
            ClearTarget();
        }
    }
}
