using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Contributor : MonoBehaviour
{
    public List<TextMeshProUGUI> contributors;

    public EventCommand command;
    // Start is called before the first frame update
    void Start()
    {
        contributors = new List<TextMeshProUGUI>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            contributors.Add(transform.GetChild(i).GetComponent<TextMeshProUGUI>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
