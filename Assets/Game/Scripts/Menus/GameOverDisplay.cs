using System;
using Game.Scripts.Buildings;
using Mirror;
using TMPro;
using UnityEngine;
namespace Game.Scripts.Menus
{
    public class GameOverDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _winnerNameText;
        [SerializeField] private GameObject _gameObjectDisplayParent;
        
        private void OnEnable()
        {
            GameOverHandler.ClientOnGameOver += OnClientOnGameOver;
        }
        
        private void OnDisable()
        {
            GameOverHandler.ClientOnGameOver -= OnClientOnGameOver;
        }

        public void LeaveGame()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopHost();
            }
            else
            {
                NetworkManager.singleton.StopClient();
            }
        }
        
        private void OnClientOnGameOver(string winner)
        {
            _winnerNameText.text = $"{winner} Has Won!";
            _gameObjectDisplayParent.SetActive(true);
        }
    }
}
