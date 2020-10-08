using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
}
