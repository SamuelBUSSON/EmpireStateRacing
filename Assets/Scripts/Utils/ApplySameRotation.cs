using System;
using UnityEngine;

namespace Utils
{
    public class ApplySameRotation : MonoBehaviour
    {
        public Transform from;
        public Transform target;


        private Vector3 _lastRotation;


        private void OnValidate()
        {
            if(target == null)
                target = transform;
        }

        private void Start()
        {
            _lastRotation = from.eulerAngles;
        }

        private void Update()
        {
            target.eulerAngles += from.eulerAngles - _lastRotation;
            _lastRotation = from.eulerAngles;
        }
    }
}