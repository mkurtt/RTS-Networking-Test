using Mirror;
using UnityEngine;
namespace Game.Scripts.Networking
{
    public class RTSNetworkManager : NetworkManager
    {

        [SerializeField] private GameObject unitSpawnerPrefab;
        
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);

            NetworkServer.Spawn(Instantiate(unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation), conn);
            
        }
    }
}
