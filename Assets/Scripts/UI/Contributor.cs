using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class Contributor : MonoBehaviour
{
    public List<TextMeshProUGUI> contributors;
    public EventCommand command;
    public RobotType type;
    public Dictionary<string, int> data;
    public bool revert;
    
    // Start is called before the first frame update
    void Start()
    {
        data = new Dictionary<string, int>();
        command.onSendAction += AddAction;
        command.onEndGame += EndGame;
        contributors = new List<TextMeshProUGUI>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            contributors.Add(transform.GetChild(i).GetComponent<TextMeshProUGUI>());
        }
        ShowContributors();
    }

    // Update is called once per frame
    void AddAction(RobotType type, int paw, string pseudo)
    {
        if (this.type != type) return;
        if(!data.ContainsKey(pseudo))data.Add(pseudo, 0);
        ++data[pseudo];
        ShowContributors();
    }

    void ShowContributors()
    {
        int i = 0;
        foreach(var contributor in data.OrderByDescending(key => key.Value))
        {
            if (i == contributors.Count) return;
            if(!revert) contributors[i].text = $"<size=12>{i + 1}  </size>{contributor.Key}  <size=50%>{contributor.Value}</size>";
            else contributors[i].text = $"<size=50%>{contributor.Value}</size>  {contributor.Key}<size=12>  {i + 1}</size>";
            contributors[i].enabled = true;
            ++i;
        }

        for (int disable = i; disable < contributors.Count; ++disable)
        {
            contributors[disable].enabled = false;
        }
    }

    void EndGame(RobotType type)
    {
        Win win = FindObjectOfType<Win>();
        win.SetContributors(data);
        win.winner.text = (type == RobotType.edison)?"Thomas Edison Win":"Nicola Tesla Win";
        win.StartTimer();
        
        
        Dictionary<string, int> allData = new Dictionary<string, int>();
        var fileName = "historical_contributor_"+type+".log";
        var sr = File.OpenText("Score/" + fileName);
        
        string input;
        while (!sr.EndOfStream)
        {
            input = sr.ReadLine();
            string[] split = input.Split(' ');
            allData.Add(split[0], int.Parse(split[1]));
        }
        sr.Close();
        
        StreamWriter writer = new StreamWriter("Score/" + fileName, false);

        foreach (var contributor in data)
        {
            if(!allData.ContainsKey(contributor.Key)) allData.Add(contributor.Key, contributor.Value);
            else
            {
                allData[contributor.Key] += contributor.Value;
            }
        }
        
        foreach (var contributor in allData)
        {
            writer.WriteLine(contributor.Key + " " + contributor.Value);
        }
        writer.Close();


        if (data.Count == 0) return;
        
        fileName = "last_contributor_"+type+".log";
        writer = new StreamWriter("Score/" + fileName, false);
        
        var first = data.OrderByDescending(key => key.Value).First();
        win.best.text = first.Key;
        writer.WriteLine(first.Key + " " + first.Value);
        
        writer.Close();
        data = new Dictionary<string, int>();
    }
}
