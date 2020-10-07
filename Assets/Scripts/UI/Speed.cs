using System;
using System.Collections;
using System.Collections.Generic;
using Twitch;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Speed : MonoBehaviour
{
    public int emisphere;
    public RobotType type;
    [Range(-360, 360)] public float maxRotate;
    [Range(-360, 360)] public float minRotate;
    public AnimationCurve curve;
    
    private float _actualSpeed;
    private float _goToSpeed = 0;
    private float _originSpeed = 0;
    private float _time = 1;

    [SerializeField]private EventCommand command;
    // Start is called before the first frame update
    void Start()
    {
        command.onSetCurrentSpeed += SetCurrentSpeed;
        _originSpeed = minRotate;
        transform.eulerAngles = new Vector3(0, 0, minRotate);
    }

    private void SetCurrentSpeed(RobotType type, int paw, float speed)
    {
        double TOLERANCE = 0.1f;
        if (this.type != type || paw%2 != emisphere || Math.Abs(speed - _actualSpeed) < TOLERANCE) return;
        _time = 0;
        
        _goToSpeed = speed;
        _originSpeed = _actualSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_time < 1)
        {
            _time += Time.deltaTime * 2;
            if (_time > 1) _time = 1;
            
            _actualSpeed = _originSpeed + (_goToSpeed - _originSpeed) * curve.Evaluate(_time);
            transform.eulerAngles = new Vector3(0,0, (_actualSpeed / (Robot.GetByType(type).maxBuffer+1)) * (maxRotate - minRotate) + minRotate);
        }
    }
}
