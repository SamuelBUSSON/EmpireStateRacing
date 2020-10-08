using System;
using UnityEngine;

namespace Players
{
    public class Ambient : MonoBehaviour
    {
        [FMODUnity.EventRef]
        public string eventStart = "";
        private FMOD.Studio.EventInstance _eventAmbientFmod;

        private void Start()
        {
            _eventAmbientFmod = FMODUnity.RuntimeManager.CreateInstance(eventStart);
            _eventAmbientFmod.start();
        }
    }
}