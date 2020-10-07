using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : MonoBehaviour
{
    
    public EventCommand eventCommand;
    [Header("Timer")]
    [SerializeField] private float teslaLeftTimer = -1;
    [SerializeField] private float teslaRightTimer = -1;
    [SerializeField] private float edisonLeftTimer = -1;
    [SerializeField] private float edisonRightTimer = -1;
    
    [Header("Speed")]
    [SerializeField] private float teslaLeftSpeed = 1;
    [SerializeField] private float teslaRightSpeed = 1;
    [SerializeField] private float edisonLeftSpeed = 1;
    [SerializeField] private float edisonRightSpeed = 1;
    

    void Start()
    {
        eventCommand.onActivatePaw += ActivePaw;
        eventCommand.onSetCurrentSpeed += CurrentSpeed;
    }

    void Update()
    {
        if (teslaLeftTimer > 0)
        {
            teslaLeftTimer -= Time.deltaTime * teslaLeftSpeed;
            if (teslaLeftTimer <= 0)
            {
                eventCommand.CallEndAction(RobotType.tesla, 0);
            }
        }
        if (teslaRightTimer > 0)
        {
            teslaRightTimer -= Time.deltaTime * teslaRightSpeed;
            if (teslaRightTimer <= 0)
            {
                eventCommand.CallEndAction(RobotType.tesla, 1);
            }
        }
        if (edisonLeftTimer > 0)
        {
            edisonLeftTimer -= Time.deltaTime * edisonLeftSpeed;
            if (edisonLeftTimer <= 0)
            {
                eventCommand.CallEndAction(RobotType.edison, 0);
            }
        }
        if (edisonRightTimer > 0)
        {
            edisonRightTimer -= Time.deltaTime * edisonRightSpeed;
            if (edisonRightTimer <= 0)
            {
                eventCommand.CallEndAction(RobotType.edison, 1);
            }
        }
    }

    public float maxActionFloat = 10;
    private void ActivePaw(RobotType type, int paw)
    {
        if (type == RobotType.tesla)
        {
            if (paw % 2 == 0) teslaLeftTimer = maxActionFloat;
            else teslaRightTimer = maxActionFloat;
        }
        else
        {
            if (paw % 2 == 0) edisonLeftTimer = maxActionFloat;
            else edisonRightTimer = maxActionFloat;
        }
    }

    public void CurrentSpeed(RobotType type, int paw, float speed)
    {
        if (type == RobotType.tesla)
        {
            if (paw % 2 == 0) teslaLeftSpeed = speed;
            else teslaRightSpeed = speed;
        }
        else
        {
            if (paw % 2 == 0) edisonLeftSpeed = speed;
            else edisonRightSpeed = speed;
        }
    }
}
