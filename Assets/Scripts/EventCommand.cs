﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RobotType
{
    tesla, edison
};

[CreateAssetMenu(fileName = "Command", menuName = "Event/Command", order = 0)]
public class EventCommand : ScriptableObject
{
    // Activate paw
    public delegate void ActivatePaw(RobotType type, string pseudo,  int paw);
    public event ActivatePaw onActivatePaw;

    public void CallActivatePaw(RobotType type, string pseudo, int paw)
    {
        //Debug.Log(pseudo + " : " + type + " -> "+ paw);
        onActivatePaw?.Invoke(type, pseudo, paw);
        onSendAction?.Invoke(type, paw,pseudo);
    }
    
    
    public delegate void SendAction(RobotType type, int paw, string pseudo);
    public event SendAction onSendAction;



    // Activate paw   
    public delegate void BlockPaw(RobotType type, int paw);
    public event BlockPaw onBlockPaw;

    public void CallBlockPaw(RobotType type, int paw)
    {
        onBlockPaw?.Invoke(type, paw);
    }
    
    // Change speed       
    public delegate void SetCurrentSpeed(RobotType type, int paw, float speed);
    public event SetCurrentSpeed onSetCurrentSpeed;
    public void CallSetCurrentSpeed(RobotType type, int paw, float speed)
    {
        onSetCurrentSpeed?.Invoke(type, paw, speed);
    }
    
    public delegate void EndAction(RobotType type, int paw);
    public event EndAction onEndAction;
    
    public void CallEndAction(RobotType type, int paw)
    {
        onEndAction?.Invoke(type, paw);
    }
    
    
    public delegate void EndGame(RobotType winner);
    public event EndGame onEndGame;
    
    public void CallEndGame(RobotType winner)
    {
        onEndGame?.Invoke(winner);
    }

    
    public delegate void SendSavior(RobotType type, string pseudo);
    public event SendSavior onSendSavior;
    public void CallSavior(RobotType team, string pseudo)
    {
        onSendSavior?.Invoke(team,pseudo);
    }
}
