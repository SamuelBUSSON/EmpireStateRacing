using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceGestor : MonoBehaviour
{
    public List<CanvasGroup> data;

    public int actual = 0;
    private float nextTimer = 20;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nextTimer -= Time.deltaTime;
        if (nextTimer < 0)
        {
            data[actual].alpha = 0;
            ++actual;
            if (actual >= data.Count) actual = 0;
            data[actual].alpha = 1;
            nextTimer = 20;
        }
    }
}
