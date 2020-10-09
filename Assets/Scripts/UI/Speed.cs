using System;
using System.Collections;
using System.Collections.Generic;
using Twitch;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Speed : MonoBehaviour
{
    public RobotType type;
    [Range(-360, 360)] public float maxRotate;
    [Range(-360, 360)] public float minRotate;
    public AnimationCurve curve;
    
    private float _actualSpeed;
    private float _goToSpeed = 0;
    private float _originSpeed = 0;
    private float _time = 1;



    [SerializeField, FMODUnity.EventRef] private string level1Event;
    private FMOD.Studio.EventInstance _level1Event;
    [SerializeField, FMODUnity.EventRef] private string level2Event;
    private FMOD.Studio.EventInstance _level2Event;
    [SerializeField, FMODUnity.EventRef] private string level3Event;
    private FMOD.Studio.EventInstance _level3Event;
    
    [SerializeField]private EventCommand command;
    // Start is called before the first frame update
    void Start()
    {
        command.onSetCurrentSpeed += SetCurrentSpeed;
        _originSpeed = minRotate;
        transform.eulerAngles = new Vector3(0, 0, minRotate);
        
        _level1Event = FMODUnity.RuntimeManager.CreateInstance(level1Event);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_level1Event, transform, rigidBody2D:null);
        _level2Event = FMODUnity.RuntimeManager.CreateInstance(level2Event);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_level2Event, transform, rigidBody2D:null);
        _level3Event = FMODUnity.RuntimeManager.CreateInstance(level3Event);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_level3Event, transform, rigidBody2D:null);
    }


    private int GetLevel(float speed)
    {
        if (speed < 1 / 3f * _time)
            return 0;
        if (speed < 2 / 3f * _time)
            return 1;
        return 2;
    }
    
    private void SetCurrentSpeed(RobotType type, int paw, float speed)
    {
        SoundEffect(speed);
        double TOLERANCE = 0.1f;
        if (this.type != type || Math.Abs(speed - _actualSpeed) < TOLERANCE) return;
        _time = 0;
        
        _goToSpeed = speed;
        _originSpeed = _actualSpeed;
    }

    private void SoundEffect(float speed)
    {
        if (GetLevel(speed) == GetLevel(_originSpeed)) return;
        if (GetLevel(speed) == 0)
            _level1Event.start();
        else if (GetLevel(speed) == 1)
            _level2Event.start();
        else
            _level3Event.start();
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
