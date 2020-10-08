using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

public enum Digit
{
    One,
    Ten,
    Hundred
}
public class ScrollTexture : MonoBehaviour
{


    
    
    public float timeToChange = 0.25f;
    public float offset;
    public float baseOffset;
    public Digit digit;
    public FloatEvent distanceChange;
    
    private float y;
    private RawImage _img;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    private bool _isChangingNumber;
    private int _currentNumber;

    // Start is called before the first frame update
    void Start()
    {
        _img = GetComponent<RawImage>();
        _img.material = Instantiate(_img.materialForRendering);
        _img.material.SetTextureOffset(MainTex, new Vector2(0, baseOffset));
        y = baseOffset;
        
        distanceChange.Register(Increment);
    }

    private void OnDestroy()
    {
        distanceChange.Unregister(Increment);
    }

    private void Increment(float newNumber)
    {

        //if (!_isChangingNumber)
        //{
            int count = 0;
            int number = Mathf.RoundToInt(newNumber * 3.72f);

            switch (digit)
            {
                case Digit.One:
                    count = number % 10;
                    break;
                case Digit.Ten:
                    count = number / 10 % 10;
                    break;
                case Digit.Hundred:
                    count = number / 100 % 10;
                    break;
            }
            
            if (count != _currentNumber)
            {
                //if(digit == Digit.Ten)
                    //TODO : Add juicy sound
                
                DOVirtual.Float(y, GetOffset(count), timeToChange, SetOffset)
                    .OnStart(BeginTween)
                    .OnComplete(() => SetYoffset(count))
                    .SetEase(Ease.OutBounce);
            
                _currentNumber = count;
            }
        //}
    }
    
    private void SetYoffset(int count)
    {
        y = GetOffset(count);
        _isChangingNumber = false;
    }
    
    private void BeginTween()
    {
        _isChangingNumber = true;
    }

    private void SetOffset(float value)
    {
        _img.materialForRendering.SetTextureOffset(MainTex, new Vector2(0, value));
    }

    private float GetOffset(int number)
    {
        return baseOffset + offset * number;
    }
    
    public float Remap (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
