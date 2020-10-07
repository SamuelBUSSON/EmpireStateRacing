using DG.Tweening;
using UnityEngine;

public class Movement : MonoBehaviour
{ // Update is called once per frame

    public Transform topLeftLeg;
    public Transform topRightLeg;
    public Transform bottomLeftLeg;
    public Transform bottomRightLeg;

    private bool _movementInProgress;
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
    
   

    private void MoveLeg(Transform leg)
    {
        if (_movementInProgress) return;
        Vector3 pos = leg.position;
        Vector3[] path = {pos, pos - Vector3.forward, pos + Vector3.up};
        leg.DOPath(path, 0.5f, PathType.CatmullRom)
            .OnStart(() => _movementInProgress = true)    
            .OnComplete(() => _movementInProgress = false);
    }
}
