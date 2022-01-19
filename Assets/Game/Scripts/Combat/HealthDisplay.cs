using System;
using UnityEngine;
using UnityEngine.UI;
namespace Game.Scripts.Combat
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private GameObject _healthBarParent;
        [SerializeField] private Image _healthBarImage;

        private void OnEnable()
        {
            _health.ClientOnHealthUpdated += HandleHealthUpdated;
        }

        private void OnDisable()
        {
            _health.ClientOnHealthUpdated -= HandleHealthUpdated;
        }

        private void OnMouseEnter()
        {
            _healthBarParent.SetActive(true);
        }
        
        private void OnMouseExit()
        {
            _healthBarParent.SetActive(false);
        }
        
        private void HandleHealthUpdated(int currentHealth, int maxHealth)
        {
            _healthBarImage.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}
