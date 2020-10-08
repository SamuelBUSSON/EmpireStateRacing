using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class DebugEndGame : MonoBehaviour
{
    public EventCommand command;
    public void EndGame()
    {
        command.CallEndGame((Random.value>0.5f)?RobotType.edison:RobotType.tesla);
    }
}
