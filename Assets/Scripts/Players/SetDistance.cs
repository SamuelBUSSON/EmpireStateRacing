using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class SetDistance : MonoBehaviour
{
    
    [FMODUnity.EventRef] [SerializeField] 
    private string applauseSound;
    
    public FloatVariable currentDistance;

    public FloatVariable otherDistance;

    private bool _isWinning;
    
    public GameObject start;
    public GameObject end;

    private void Update()
    {
        currentDistance.SetValue(transform.position.y - start.transform.position.y);

        if (otherDistance.Value < currentDistance.Value && !_isWinning)
        {
            Debug.Log("Play applause Sound");
            FMODUnity.RuntimeManager.PlayOneShot(applauseSound, transform.position);
            _isWinning = true;
        }

        if (_isWinning && otherDistance.Value > currentDistance.Value)
            _isWinning = false;
        
        if(Mathf.RoundToInt(currentDistance.Value * 3.72f) >= 371)
            GetComponent<Movement>().command.CallEndGame( GetComponent<Movement>().team);


    }


    
    
}
