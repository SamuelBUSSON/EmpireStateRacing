using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class Historical : MonoBehaviour
{
    public RobotType type;
    public TextMeshProUGUI historicalContributor;
    public TextMeshProUGUI lastContributor;
    public TextMeshProUGUI lastSavior;

    public bool revert;
    // Start is called before the first frame update
    void Awake()
    {
        GetHistoricalContributor();
        GetLastContributor();
        GetLastSavior();
    }


    void GetHistoricalContributor()
    {
        Dictionary<string, int> data = new Dictionary<string, int>();
        var fileName = "historical_contributor_"+type+".log";
        var sr = File.OpenText("Score/" + fileName);
        
        string input;
        while (!sr.EndOfStream)
        {
            input = sr.ReadLine();
            string[] split = input.Split(' ');
            data.Add(split[0], int.Parse(split[1]));
        }
        
        var bestContributor = data.OrderByDescending(key => key.Value).First();
        if(!revert) historicalContributor.text = bestContributor.Key + " <size=50%>"+bestContributor.Value+"</size>";
        else historicalContributor.text ="<size=50%>"+bestContributor.Value+"</size> " + bestContributor.Key;
        
    }
    
    private void GetLastContributor()
    {
        
        var fileName = "last_contributor_"+type+".log"; 
        var sr = File.OpenText("Score/" + fileName);
        string line = sr.ReadLine();
        if (string.IsNullOrEmpty(line))
        {
            lastContributor.text = "---"; return;
        }
        string[] input = line.Split(' ');
        if(!revert) lastContributor.text = input[0] + " <size=50%>"+int.Parse(input[1])+"</size>";
        else lastContributor.text = "<size=50%>"+int.Parse(input[1])+"</size> "+input[0];
    }
    
    private void GetLastSavior()
    {
        var fileName = "last_savior_"+type+".log"; 
        var sr = File.OpenText("Score/" + fileName);
        string line = sr.ReadLine();
        if (line == "")
        {
            lastSavior.text = "---"; return;
        }
        
        string[] input = line.Split(' ');
        if(!revert) lastSavior.text = input[0] + " <size=50%>"+int.Parse(input[1])+"</size>";
        else lastSavior.text = "<size=50%>"+int.Parse(input[1])+"</size> "+input[0];
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
