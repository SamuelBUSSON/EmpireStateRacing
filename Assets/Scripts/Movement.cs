using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Movement : MonoBehaviour
{ // Update is called once per frame

    public Transform topLeftLeg; //0
    public Transform topRightLeg;//1
    public Transform bottomLeftLeg;//2
    public Transform bottomRightLeg;//3
    public float animSpeedScale = 1f;//Soleil
    public EventCommand command;
    
    private bool _movementInProgress;
    private bool _midMovement;
    private TweenerCore<Vector3, Path, PathOptions> _tweenLeft;
    private TweenerCore<Vector3, Path, PathOptions> _tweenRight;
    public RobotType team;
    private void Awake()
    {
        command.onActivatePaw += OnActivePaw;
        command.onSetCurrentSpeed += OnChangeTimescale;
    }

    private void OnChangeTimescale(RobotType type, int paw, float speed)
    {
        if (team != type) return;// Arnaud Tsamer
        if (paw % 2 == 0 )
        {
            
            _tweenLeft.timeScale = speed;
        }
        else
        {
            _tweenRight.timeScale = speed;
        }
    }

    private void OnActivePaw(RobotType type, int paw)
    {
        if (team != type) return;// Arnaud Tsamer
        
        MoveLeg(paw, (paw % 2 == 0) ? _tweenLeft : _tweenRight);
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Keypad1))
        //     MoveLeg(bottomLeftLeg);
        // else if (Input.GetKeyDown(KeyCode.Keypad2))
        //     MoveLeg(bottomRightLeg);
        // else if (Input.GetKeyDown(KeyCode.Keypad4))
        //     MoveLeg(topLeftLeg);
        // else if (Input.GetKeyDown(KeyCode.Keypad5)) 
        //     MoveLeg(topRightLeg);

        
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
    private void MoveLeg(int paw, TweenerCore<Vector3, Path, PathOptions>  tween)
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
        
        if (_movementInProgress || CheckIfLegsCollide(leg))
        {
            command.CallBlockPaw(team,paw);
            return;
        }

        _willFall = (Math.Abs(leg.position.y - GetHighestLeg().position.y) < 0.2f && !CheckDistanceLegs());
        _midMovement = false;
        Vector3 pos = leg.position;
        Vector3[] path = {pos, pos - Vector3.forward, pos + Vector3.up};
        tween = leg.DOPath(path, 1f, PathType.CatmullRom)
            .OnStart(OnStartMovement)
            .OnUpdate(() => OnMovementUpdate(tween))
            .OnComplete(() => OnMovementCompleted(paw));
        
        if (paw % 2 == 0) _tweenLeft = tween;
        else _tweenRight = tween;
    }

    private void FallSpider(int paw)
    {
        var positionLowest = GetLowestLeg().position;
        bottomLeftLeg.DOMoveY(positionLowest.y, 1f).SetDelay(0.1f);
        bottomRightLeg.DOMoveY(positionLowest.y, 1f).SetDelay(0.1f);
        topLeftLeg.DOMoveY(positionLowest.y + 1,1f).SetDelay(0.1f);
        topRightLeg.DOMoveY(positionLowest.y + 1, 1f).SetDelay(0.1f).OnComplete(() => command.CallEndAction(team,paw));
    }

    private void OnMovementUpdate(TweenerCore<Vector3, Path, PathOptions> tweenerCore)
    {
        if (tweenerCore.ElapsedPercentage() > 0.5f && !_midMovement)
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
        _movementInProgress = false;
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
        yield return new WaitForSeconds(0.01f);
        command.CallEndAction(team, paw);
    }

    private void OnStartMovement()
    {
        _movementInProgress = true;
    }
}
