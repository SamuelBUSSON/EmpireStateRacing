using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
    public class Robot : MonoBehaviour
    {
        public static Robot tesla;
        public static Robot edison;

        public string name;
        public int nbPaw = 4;
        public int maxBuffer = 10;
        public float maxActionTime = 10;
        public Stack<int> bufferLeft;
        public Stack<int> bufferRight;

        private bool active;
        
        void Awake()
        {
            if (name.ToLower() == "tesla") tesla = this;
            else edison = this;
        }

        void Update()
        {
            
        }

        public void EndAction()
        {
            // TODO END ACTION OR NEXT ACTION
        }
        
        public bool Command(int paw)
        {
            if (paw >= nbPaw) return false;

            if (paw < nbPaw / 2)
            {
                if (active)
                {
                    bufferLeft.Push(paw);
                    if (bufferLeft.Count > maxBuffer) bufferLeft.Pop();
                }
                else
                {
                    active = true;
                    //TODO LAUNCH ACTION
                }
            }
            else
            {
                if (active)
                {
                    bufferRight.Push(paw);
                    if (bufferRight.Count > maxBuffer) bufferRight.Pop();
                }

                else
                {
                    active = true;
                    //TODO LAUNCH ACTION
                }
            }
            return true;
        }
    }
}