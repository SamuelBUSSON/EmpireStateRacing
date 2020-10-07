using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class SetDistance : MonoBehaviour
{
    public FloatVariable currentDistancePercent;
    public FloatVariable currentDistance;
    public GameObject start;
    public GameObject end;

    private void Update()
    {
        currentDistancePercent.SetValue(transform.position.y / (start.transform.position.y + end.transform.position.y));
        currentDistance.SetValue(transform.position.y - start.transform.position.y);
    }


    
    
}
