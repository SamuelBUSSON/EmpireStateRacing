﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
    public class Robot : MonoBehaviour
    {
        public static Robot tesla;
        public static Robot edison;

        public Emisphere emisphere;
        //public Emisphere right;
        [Header("Information")]
        public string name;
        public RobotType type;
        
        [Header("Statistic")]
        public int nbPaw = 4;
        public int maxBuffer = 9;
        [Header("Event")] 
        public EventCommand eventCommand;
        
        [Header("Buffer")] 
        public List<Buffer> buffer;
        
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
            eventCommand.onEndGame += (RobotType _type) => EndGame();
            
            emisphere = new Emisphere(this);
        }

        private void EndAction(RobotType _type, int paw)
        {
            if (_type != type) return;
            emisphere.NextAction();
        }

        private void BlockPaw(RobotType _type, int paw)
        {
            if (_type != type) return;
            EndAction(_type, paw);
        }
        
        public bool Command(string pseudo, int paw)
        {
            if (paw >= nbPaw) return false;
            emisphere.Command(pseudo, paw);
            //if (paw%2 == 0) left.Command(pseudo, paw);
            //else right.Command(pseudo, paw);
            
            return true;
        }

        public void EndGame()
        {
            buffer = new List<Buffer>();
            
        }
        public static Robot GetByType(RobotType robotType)
        {
            if (robotType == RobotType.edison) return edison;
            else return tesla;
        }
    }
}

[Serializable]
public class Buffer
{
    public string pseudo;
    public int paw;

    public Buffer(string _pseudo, int _paw)
    {
        pseudo = _pseudo;
        paw = _paw;
    }
}