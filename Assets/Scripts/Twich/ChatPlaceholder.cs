using System.Collections;
using System.Collections.Generic;
using Twitch;
using UnityEngine;

public class ChatPlaceholder : MonoBehaviour
{
    public List<string> pseudoPlaceholder;

    private string pseudo
    {
        get { return pseudoPlaceholder[Mathf.FloorToInt(Random.Range(0, pseudoPlaceholder.Count))]; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) Robot.tesla.Command(pseudo, 0);
        if (Input.GetKeyDown(KeyCode.Z)) Robot.tesla.Command(pseudo, 1);
        if (Input.GetKeyDown(KeyCode.E)) Robot.tesla.Command(pseudo, 2);
        if (Input.GetKeyDown(KeyCode.R)) Robot.tesla.Command(pseudo, 3);
        
        if (Input.GetKeyDown(KeyCode.Q)) Robot.edison.Command(pseudo, 0);
        if (Input.GetKeyDown(KeyCode.S)) Robot.edison.Command(pseudo, 1);
        if (Input.GetKeyDown(KeyCode.D)) Robot.edison.Command(pseudo, 2);
        if (Input.GetKeyDown(KeyCode.F)) Robot.edison.Command(pseudo, 3);
    }
}
