using System;
using Mirror;
using UnityEngine;
namespace Game.Scripts.Combat
{
    public class Targetable : NetworkBehaviour
    {
        [SerializeField] private Transform _aimAtPoint;

        public Transform GetAimAtPoint => _aimAtPoint;
    }
}
