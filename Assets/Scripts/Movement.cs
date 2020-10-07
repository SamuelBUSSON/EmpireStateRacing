using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Movement : MonoBehaviour
{ // Update is called once per frame

    public Transform endEffector;
    private bool _test = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Move();
        }
    }
    
   

    private void Move()
    {
        if (!_test) return;
        
        Vector3 pos = endEffector.localPosition;
        Vector3[] path = {pos, pos - Vector3.forward, pos + Vector3.up};

        endEffector.DOLocalPath(path, 8f, PathType.CatmullRom)
            .OnStart(() => _test = false)
            .OnComplete(() => _test = true);
        
    }
}
