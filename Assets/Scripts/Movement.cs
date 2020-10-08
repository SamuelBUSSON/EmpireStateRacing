﻿using System;
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
    private void Awake()
    {
        command.onActivatePaw += OnActivePaw;
        command.onSetCurrentSpeed += OnChangeTimescale;
    }



    public float timeScale = 1;
    
    private void OnChangeTimescale(RobotType type, int paw, float speed)
    {
        if (team != type) return;// Arnaud Tsamer
        
        timeScale = speed;
        if(_tween != null) _tween.timeScale = speed;
        
    }

    private void OnActivePaw(RobotType type, int paw)
    {
        if (team != type) return;// Arnaud Tsamer
        
        MoveLeg(paw);
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
    private void MoveLeg(int paw)
    {
        Transform leg;
        switch (paw)
        {
            case 0:
                leg = topLeftLeg;
                break;
            case 1:
                leg = topRightLeg;
                break;
            case 2:
                leg = bottomLeftLeg;
                break;
            default:
                leg = bottomRightLeg;
                break;
        }
        
        if (CheckIfLegsCollide(leg))
        {
            Vector3 pos = leg.position;
            Vector3[] path = {pos, pos - Vector3.forward, pos + Vector3.up/2, pos - Vector3.forward, pos};
        
            _tween = leg.DOPath(path, Robot.GetByType(team).maxBuffer+1, PathType.CatmullRom)
                .OnStart(OnStartMovement)
                .OnUpdate(() => OnMovementUpdate(_tween))
                .OnComplete(() => StartCoroutine(EndAction(paw)));
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
        // TODO some sound
    }

    private void OnMovementCompleted(int paw)
    {
        if(_willFall)
            FallSpider(paw);
        else
        {
            StartCoroutine(EndAction(paw));
        }
        _willFall = false;
    }

    private IEnumerator EndAction(int paw)
    {
        SetTrails(false);
        yield return new WaitForSeconds(0.1f);
        command.CallEndAction(team, paw);
    }

    private void SetTrails(bool active)
    {
        foreach (GameObject trail in trails)
        {
            trail.SetActive(active);
        }
    }
    
    private IEnumerator BlockAction( int paw)
    {
        yield return new WaitForSeconds(0.2f);
        command.CallBlockPaw(team, paw);
    }


    private void OnStartMovement()
    {
        
    }
}
