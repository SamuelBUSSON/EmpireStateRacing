using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class Savior : MonoBehaviour
{
    public Dictionary<string, int> data;
    public EventCommand command;
    public RobotType type;
    public TextMeshProUGUI leader;
    public bool revert;
    
    // Start is called before the first frame update
    void Start()
    {
        data = new Dictionary<string, int>();
        leader = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        leader.text = "---";
        command.onSendSavior += AddAction;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void AddAction(RobotType type, string pseudo)
    {
        if (this.type != type) return;
        if (!data.ContainsKey(pseudo)) data.Add(pseudo, 0);
        ++data[pseudo];
        var first = data.OrderByDescending(key => key.Value).First();
        if(!revert) leader.text = first.Key+" <size=50%>"+first.Value+"</size>";
        else leader.text = "<size=50%>"+first.Value+"</size>  "+first.Key;
    }

    void EndGame()
    {
        
        var fileName = "last_savior_"+type+".log";
        var writer = new StreamWriter("Score/" + fileName, false);
        
        var first = data.OrderByDescending(key => key.Value).First();
        writer.WriteLine(first.Key + " " + first.Value);
        
        writer.Close();
        data = new Dictionary<string, int>();
    }
}
