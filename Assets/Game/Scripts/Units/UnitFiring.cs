using System;
using Game.Scripts.Combat;
using Mirror;
using UnityEngine;
namespace Game.Scripts.Units
{
    public class UnitFiring : NetworkBehaviour
    {
        [SerializeField] private Targeter _targeter;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _projectileSpawnPoint;
        [SerializeField] private float _fireRange = 5f;
        [SerializeField] private float _fireRate = 1f;
        [SerializeField] private float _rotationSpeed = 999f;

        private float _lastFireTime;
        
        [ServerCallback]
        private void Update()
        {
            var target = _targeter.GetTarget;
            
            if (!target) return;

            if (!CanFireAtTarget()) return;

            var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);

            if (Time.time > (1 / _fireRate) + _lastFireTime)
            {
                var projectileRot = Quaternion.LookRotation(target.GetAimAtPoint.position - _projectileSpawnPoint.position);

                var projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, projectileRot);

                NetworkServer.Spawn(projectile,connectionToClient);
                
                _lastFireTime = Time.time;
            }
        }

        [Server]
        private bool CanFireAtTarget()
        {
            return (_targeter.GetTarget.transform.position - transform.position).sqrMagnitude < _fireRange * _fireRange;
        }
    }
}
