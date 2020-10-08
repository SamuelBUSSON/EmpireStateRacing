using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class SetDistance : MonoBehaviour
{
    
    [FMODUnity.EventRef] [SerializeField] 
    private string applauseSound;
    
    public FloatVariable currentDistancePercent;
    public FloatVariable currentDistance;

    public FloatVariable otherDistance;

    public bool isWinning;
    
    public GameObject start;
    public GameObject end;

    private void Update()
    {
        currentDistancePercent.SetValue(transform.position.y / (start.transform.position.y + end.transform.position.y));
        currentDistance.SetValue(transform.position.y - start.transform.position.y);

        if (otherDistance.Value < currentDistance.Value && !isWinning)
        {
            Debug.Log("Play applause Sound");
            FMODUnity.RuntimeManager.PlayOneShot(applauseSound, transform.position);
            isWinning = true;
        }

        if (isWinning && otherDistance.Value > currentDistance.Value)
            isWinning = false;


    }


    
    
}
