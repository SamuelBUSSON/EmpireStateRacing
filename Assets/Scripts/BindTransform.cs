using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class BindTransform : MonoBehaviour
    {
        public Transform lookTo;
        public bool initLookTo = true;
        private Transform _myTransform;
        private void Start()
        {
            if (!initLookTo) return;
            _myTransform = transform;
            lookTo.position = _myTransform.position;
            lookTo.rotation = _myTransform.rotation;
        }

        private void Update()
        {
            _myTransform.position = lookTo.position;
            _myTransform.rotation = lookTo.rotation;
        }
    }
}