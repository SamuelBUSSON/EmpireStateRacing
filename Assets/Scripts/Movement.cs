using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Movement : MonoBehaviour
{ // Update is called once per frame

    public Transform topLeftLeg;
    public Transform topRightLeg;
    public Transform bottomLeftLeg;
    public Transform bottomRightLeg;
    public float animSpeedScale = 1f;
    
    
    private bool _movementInProgress;
    private bool _midMovement;
    private TweenerCore<Vector3, Path, PathOptions> _tween;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            MoveLeg(bottomLeftLeg);
        else if (Input.GetKeyDown(KeyCode.Keypad2))
            MoveLeg(bottomRightLeg);
        else if (Input.GetKeyDown(KeyCode.Keypad4))
            MoveLeg(topLeftLeg);
        else if (Input.GetKeyDown(KeyCode.Keypad5)) 
            MoveLeg(topRightLeg);

        
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
    private void MoveLeg(Transform leg)
    {
        if (_movementInProgress || CheckIfLegsCollide(leg)) return;
         _willFall = (Math.Abs(leg.position.y - GetHighestLeg().position.y) < 0.2f && !CheckDistanceLegs());
        _midMovement = false;
        Vector3 pos = leg.position;
        Vector3[] path = {pos, pos - Vector3.forward, pos + Vector3.up};
        _tween = leg.DOPath(path, 1f, PathType.CatmullRom)
            .OnStart(OnStartMovement)
            .OnUpdate(OnMovementUpdate)
            .OnComplete(OnMovementCompleted);
        
    }

    private void FallSpider()
    {
        var positionLowest = GetLowestLeg().position;
        bottomLeftLeg.DOMoveY(positionLowest.y, 1f).SetDelay(0.1f).OnStart(() => _movementInProgress = true);
        bottomRightLeg.DOMoveY(positionLowest.y, 1f).SetDelay(0.1f);
        topLeftLeg.DOMoveY(positionLowest.y + 1,1f).SetDelay(0.1f);
        topRightLeg.DOMoveY(positionLowest.y + 1, 1f).SetDelay(0.1f).OnComplete(() => _movementInProgress = false);
    }

    private void OnMovementUpdate()
    {
        _tween.timeScale = animSpeedScale;
        if (_tween.ElapsedPercentage() > 0.5f && !_midMovement)
        {
            _midMovement = true;
            OnMovementMid();
        } 
    }

    private void OnMovementMid()
    {
        // TODO some sound
    }

    private void OnMovementCompleted()
    {
        _movementInProgress = false;
        _tween.Kill();
        _tween = null;
        if(_willFall)
            FallSpider();
        _willFall = false;
    }

    private void OnStartMovement()
    {
        _movementInProgress = true;
    }
}
