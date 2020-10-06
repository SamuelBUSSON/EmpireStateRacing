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
        public RobotType type;
        public int nbPaw = 4;
        public int maxBuffer = 10;
        public float maxActionTime = 10;
        [Header("Event")] 
        [SerializeField] private EventCommand eventCommand;

        public int activeLeft;
        [HideInInspector] public Stack<int> bufferLeft;
        public int activeRight;
        [HideInInspector] public Stack<int> bufferRight;

        
        private void Awake()
        {
            if (name.ToLower() == "tesla")
            {
                type = RobotType.tesla;
                tesla = this;
            }
            else
            {
                type = RobotType.edison;
                edison = this;
            }

            eventCommand.onEndAction += EndAction;
            eventCommand.onBlockPaw += BlockPaw;
        }

        private void Update()
        {
            
        }

        private void EndAction(RobotType _type, int paw)
        {
            if (_type != type) return;
            if (paw < nbPaw / 2) NextActionLeft();
            else NextActionRight();
        }

        private void BlockPaw(RobotType _type, int paw)
        {
            if (_type != type) return;
            Debug.Log("BlockPaw");
            EndAction(_type, paw);
        }
        
        public bool Command(int paw)
        {
            if (paw >= nbPaw) return false;

            if (paw < nbPaw / 2)
            {
                if (activeLeft > 0)
                {
                    bufferLeft.Push(paw);
                    if (bufferLeft.Count > maxBuffer) bufferLeft.Pop();
                }
                else
                {
                    activeLeft = paw;
                    eventCommand.CallActivatePaw(type, activeLeft);
                }
                eventCommand.CallSetCurrentSpeed(type, activeLeft,maxActionTime/bufferLeft.Count);
            }
            else
            {
                if (activeRight > 0)
                {
                    bufferRight.Push(paw);
                    if (bufferRight.Count > maxBuffer) bufferRight.Pop();
                }

                else
                {
                    activeRight = paw;
                    eventCommand.CallActivatePaw(type, activeRight);
                    
                }
                eventCommand.CallSetCurrentSpeed(type, activeRight,maxActionTime/bufferRight.Count);
            }
            return true;
        }

        
        private void NextActionLeft()
        {
            if (bufferLeft.Count > 0)
            {
                activeLeft = bufferLeft.Pop();
                eventCommand.CallActivatePaw(type, activeLeft);
            }
            else activeLeft = -1;
            eventCommand.CallSetCurrentSpeed(type, activeLeft, maxActionTime/bufferLeft.Count);
        }
        private void NextActionRight()
        {
            if (bufferRight.Count > 0)
            {
                activeRight = bufferRight.Pop();
                eventCommand.CallActivatePaw(type, activeRight);
            }
            else activeRight = -1;
            eventCommand.CallSetCurrentSpeed(type, activeRight, maxActionTime/bufferRight.Count);
        }
    }
}