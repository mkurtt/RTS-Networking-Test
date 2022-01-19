using System;
using System.Runtime.CompilerServices;
using Game.Scripts.Combat;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
namespace Game.Scripts.Units
{
    public class UnitProjectile : NetworkBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody = null;
        [SerializeField] private int _damage = 20;
        [SerializeField] private float _aliveTime = 5f;
        [SerializeField] private float _launchForce = 10f;

        private void Start()
        {
            _rigidbody.velocity = transform.forward * _launchForce;
        }

        public override void OnStartServer()
        {
            Invoke(nameof(DestroySelf), _aliveTime);
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<NetworkIdentity>(out var networkIdentity))
            {
                if (networkIdentity.connectionToClient == connectionToClient) return;
            }

            if (other.TryGetComponent<Health>(out var health))
            {
                health.DealDamage(_damage);
            }
            
            DestroySelf();
        }

        [Server]
        private void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }

    }
}
