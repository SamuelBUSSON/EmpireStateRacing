using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Leader : MonoBehaviour
{
    public Dictionary<string, int> data;
    public EventCommand command;
    public RobotType type;
    public TextMeshProUGUI leader;
    public int paw;
    
    // Start is called before the first frame update
    void Start()
    {
        data = new Dictionary<string, int>();
        leader = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        leader.text = "---";
        command.onSendAction += AddAction;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void AddAction(RobotType type, int _paw, string pseudo)
    {
        if (this.type != type || paw != _paw) return;
        if (!data.ContainsKey(pseudo)) data.Add(pseudo, 0);
        ++data[pseudo];
        var first = data.OrderByDescending(key => key.Value).First();
        leader.text = first.Key+" <size=50%>"+first.Value+"</size>";

    }
}