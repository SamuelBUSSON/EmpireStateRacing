using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using Twitch;
using UnityEngine;

public class Movement : MonoBehaviour
{ // Update is called once per frame

    public Transform topLeftLeg; //0
    public Transform topRightLeg;//1
    public Transform bottomLeftLeg;//2
    public Transform bottomRightLeg;//3
    public float animSpeedScale = 1f;//Soleil
    public EventCommand command;

    public List<GameObject> trails = new List<GameObject>();

    private bool _midMovement;
    private TweenerCore<Vector3, Path, PathOptions> _tween;
    public RobotType team;
    
    
   
    /// 
    ///
    ///               FMOD
    /// 
    ///
    
    [FMODUnity.EventRef]
    public string eventStart = "";
    private FMOD.Studio.EventInstance _eventStartFmod;
    
    [FMODUnity.EventRef]
    public string eventPeak = "";
    private FMOD.Studio.EventInstance _eventPeakFmod;

    [FMODUnity.EventRef]
    public string eventImpact = "";
    private FMOD.Studio.EventInstance _eventImpactFmod;
    
    [FMODUnity.EventRef]
    public string eventSlide = "";
    private FMOD.Studio.EventInstance _eventSlideFmod;
    
    [FMODUnity.EventRef]
    public string eventBug = "";
    private FMOD.Studio.EventInstance _eventBugFmod;
    
    private void Awake()
    {
        command.onActivatePaw += OnActivePaw;
        command.onSetCurrentSpeed += OnChangeTimescale;
    }
    
    private void Start()
    {
        // Init all FMOD events
        _eventStartFmod = FMODUnity.RuntimeManager.CreateInstance(eventStart);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_eventStartFmod, transform, rigidBody:null);
        _eventPeakFmod = FMODUnity.RuntimeManager.CreateInstance(eventPeak);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_eventPeakFmod, transform, rigidBody:null);
        _eventImpactFmod = FMODUnity.RuntimeManager.CreateInstance(eventImpact);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_eventImpactFmod, transform, rigidBody:null);
        _eventSlideFmod = FMODUnity.RuntimeManager.CreateInstance(eventSlide);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_eventSlideFmod, transform, rigidBody:null);
        _eventBugFmod = FMODUnity.RuntimeManager.CreateInstance(eventBug);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_eventBugFmod, transform, rigidBody:null);
    }

    public float timeScale = 1;
    
    private void OnChangeTimescale(RobotType type, int paw, float speed)
    {
        if (team != type) return;// Arnaud Tsamer
        
        timeScale = speed;
        if(_tween != null) _tween.timeScale = speed;
        
    }

    private void OnActivePaw(RobotType type, string pseudo, int paw)
    {
        if (team != type) return;// Arnaud Tsamer
        
        MoveLeg(pseudo, paw);
    }

    private void Update()
    {
        // Replace correctly the spider body
        float z = transform.position.z;
        Vector3 pos = (topLeftLeg.position + topRightLeg.position + bottomLeftLeg.position +
                             bottomRightLeg.position) / 4;
        pos.z = z;
        transform.position = pos;
    }


    private Transform GetLowestLeg()
    {
        Transform result = bottomLeftLeg;
        if (result.position.y > bottomRightLeg.position.y)
            result = bottomRightLeg;
        return result;
    }

    private Transform GetHighestLeg()
    {
        Transform result = topLeftLeg;
        if (result.position.y < topRightLeg.position.y)
            result = topRightLeg;
        return result;
    }

    private bool CheckIfLegsCollide(Transform leg)
    {
        return Math.Abs(leg.position.y + 1 - GetAssociatedLeg(leg).position.y) < 0.3f;
    }

    private Transform GetAssociatedLeg( Transform leg)
    {
        return leg == topLeftLeg ? bottomLeftLeg :
            leg == bottomLeftLeg ? topLeftLeg :
            leg == topRightLeg ? bottomRightLeg :
            leg == bottomRightLeg ? topRightLeg : leg;
    }
    
    
    private bool CheckDistanceLegs()
    {
        return !(GetHighestLeg().position.y - GetLowestLeg().position.y > 2.8f);
    }

    private bool _willFall;
    private void MoveLeg(string pseudo, int paw)
    {
        Transform leg;
        bool top = false;
        switch (paw)
        {
            case 0:
                leg = topLeftLeg;
                top = true;
                break;
            case 1:
                leg = topRightLeg;
                top = true;
                break;
            case 2:
                leg = bottomLeftLeg;
                break;
            default:
                leg = bottomRightLeg;
                break;
        }

        float TOLERANCE = 0.2f;
        Transform other = GetAssociatedLeg(leg);
        if (top && CheckIfLegsCollide(other))
        {
            // Debug.Log("Savior Collide");
            command.CallSavior(team, pseudo);

        }
        else
        {
            if (!top && (leg == GetLowestLeg() || Math.Abs(bottomRightLeg.position.y - bottomLeftLeg.position.y) < TOLERANCE) && !CheckDistanceLegs())
            {
                // Debug.Log("Savior Fall");
                command.CallSavior(team, pseudo);
            }
        }

        if (CheckIfLegsCollide(leg))
        {
            Vector3 pos = leg.position;
            Vector3[] path = {pos, pos - Vector3.forward, pos + Vector3.up/2, pos - Vector3.forward, pos};
        
            _tween = leg.DOPath(path, Robot.GetByType(team).maxBuffer+1, PathType.CatmullRom)
                .OnStart(OnStartMovement)
                .OnUpdate(() => OnMovementUpdate(_tween))
                .OnComplete(() =>
                {
                    _eventBugFmod.start();
                    StartCoroutine(EndAction(paw));
                });
            _tween.timeScale = timeScale;
        }
        else
        {
            _willFall = (Math.Abs(leg.position.y - GetHighestLeg().position.y) < 0.2f && !CheckDistanceLegs());
            _midMovement = false;
            Vector3 pos = leg.position;
            Vector3[] path = {pos, pos - Vector3.forward, pos + Vector3.up};
        
        

            _tween = leg.DOPath(path, Robot.GetByType(team).maxBuffer+1, PathType.CatmullRom)
                .OnStart(OnStartMovement)
                .OnUpdate(() => OnMovementUpdate(_tween))
                .OnComplete(() => OnMovementCompleted(paw));
            _tween.timeScale = timeScale;
        }
    }
    
    
    private void FallSpider(int paw)
    {
        _eventSlideFmod.start();
        SetTrails(true);
        var positionLowest = GetLowestLeg().position;
        bottomLeftLeg.DOMoveY(positionLowest.y, 1f).SetDelay(0.1f);
        bottomRightLeg.DOMoveY(positionLowest.y, 1f).SetDelay(0.1f);
        topLeftLeg.DOMoveY(positionLowest.y + 1,1f).SetDelay(0.1f);
        topRightLeg.DOMoveY(positionLowest.y + 1, 1f).SetDelay(0.1f).OnComplete(() => StartCoroutine(EndAction(paw)));
    }

    private void OnMovementUpdate(TweenerCore<Vector3, Path, PathOptions> tweenerCore)
    {
        if (tweenerCore.ElapsedPercentage() > 0.1f && !_midMovement)
        {
            _midMovement = true; //TODO PLUS TARD FDP (IL Y EN A 2 MTN)
            OnMovementMid();
        } 
    }

    private void OnMovementMid()
    {
        _eventPeakFmod.start();
    }

    private void OnMovementCompleted(int paw)
    {
        if(_willFall)
            FallSpider(paw);
        else
        {
            _eventImpactFmod.start();
            StartCoroutine(EndAction(paw));
        }
        _willFall = false;
    }

    private IEnumerator EndAction(int paw)
    {
        _eventSlideFmod.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _eventPeakFmod.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _eventStartFmod.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        
        SetTrails(false);
        yield return new WaitForSeconds(0.1f);
        command.CallEndAction(team, paw);
    }

    private void SetTrails(bool active)
    {
        foreach (GameObject trail in trails)
        {
            if(trail != null) trail.GetComponent<TrailRenderer>().enabled = active;
        }
    }
    
    private IEnumerator BlockAction( int paw)
    {
        yield return new WaitForSeconds(0.2f);
        command.CallBlockPaw(team, paw);
    }


    private void OnStartMovement()
    {
        _eventStartFmod.start();
    }
}
