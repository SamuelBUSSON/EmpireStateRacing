using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public GameObject contributors;
    public int maxContributors = 6;
    
    public TextMeshProUGUI winner;
    public TextMeshProUGUI alpha;
    public TextMeshProUGUI beta;
    public TextMeshProUGUI gama;
    public TextMeshProUGUI delta;
    public TextMeshProUGUI best;
    public TextMeshProUGUI timerInfo;
    public EventCommand command;

    public float timer;
    public void SetContributors(Dictionary<string, int> data)
    {
        
            
        List<TextMeshProUGUI> lists = new List<TextMeshProUGUI>();
        for (int i = 0; i < contributors.transform.childCount; ++i)
        {
            TextMeshProUGUI c = contributors.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            lists.Add(c);
            c.text = "";
        }

        int listId = 0;
        int contributorId = 0;
        
        foreach (var contributor in data)
        {
            lists[Mathf.FloorToInt(contributorId / (float) maxContributors)].text += contributor.Key+"\n";
            ++contributorId;
            if (contributorId >= lists.Count * maxContributors) return;
        }
    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape)) command.CallEndGame((Random.value>0.5f)?RobotType.edison:RobotType.tesla);
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timerInfo.text = "Next game in " + Mathf.Ceil(timer) + " seconds";
            if (timer < 0) SceneManager.LoadScene(0,LoadSceneMode.Single);
            
        }
    }
    public void StartTimer()
    {
        timer = 20;
        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        float timerShow = 1;
        while(timerShow > 0)
        {
            yield return new WaitForSeconds(0.05f);
            group.alpha = 1 - timerShow;
            timerShow -= 0.05f;
        }

        group.alpha = 1;
    }
    
}
