﻿using System.Collections;
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
    public bool revert;
    
    // Start is called before the first frame update
    void Start()
    {
        data = new Dictionary<string, int>();
        leader = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        leader.text = "---";
        command.onSendAction += AddAction;
        command.onEndGame += EndGame;
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
        if(!revert) leader.text = first.Key+" <size=50%>"+first.Value+"</size>";
        else leader.text = "<size=50%>"+first.Value+"</size>  "+first.Key;
    }

    void EndGame(RobotType type)
    {
        if (this.type != type) return;
        string first = "---";
        if(data.Count > 0) first = data.OrderByDescending(key => key.Value).First().Key;
        switch (paw)
        {
            case 0 : 
                FindObjectOfType<Win>().alpha.text = first;
                break;
            case 1 : 
                FindObjectOfType<Win>().beta.text = first;
                break;
            case 2 : 
                FindObjectOfType<Win>().gama.text = first;
                break;
            case 3 : 
                FindObjectOfType<Win>().delta.text = first;
                break;
        }
    }
}