using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollTexture : MonoBehaviour
{

    public float speed = 0.25f;
    public float timeToChange = 0.25f;
    public AnimationCurve curve;
    public float offset;
    
    private float y = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        y += 1 * Time.deltaTime * speed;
        
        GetComponent<RawImage>().materialForRendering.SetTextureOffset("_MainTex", new Vector2(0, y));
    }
}
