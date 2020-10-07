using System.Collections;
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
    public delegate void ActivatePaw(RobotType type, int paw);
    public event ActivatePaw onActivatePaw;

    public void CallActivatePaw(RobotType type, int paw)
    {
        onActivatePaw?.Invoke(type, paw);
    }
    
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
    
    // Activate paw   
    public delegate void EndAction(RobotType type, int paw);
    public event EndAction onEndAction;
    
    public void CallEndAction(RobotType type, int paw)
    {
        onEndAction?.Invoke(type, paw);
    }
}
