using System;
using Game.Scripts.Buildings;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Game.Scripts.Networking
{
    public class RTSNetworkManager : NetworkManager
    {

        [SerializeField] private GameObject unitSpawnerPrefab;
        [SerializeField] private GameOverHandler _gameOverHandlerPrefab;
        
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);

            NetworkServer.Spawn(Instantiate(unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation), conn);
            
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (SceneManager.GetActiveScene().name.StartsWith("Scene_Map"))
            {
                var GOHandler = Instantiate(_gameOverHandlerPrefab);
                NetworkServer.Spawn(GOHandler.gameObject);
            }
        }
    }
}
