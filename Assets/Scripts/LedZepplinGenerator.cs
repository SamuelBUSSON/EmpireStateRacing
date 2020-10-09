using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class LedZepplinGenerator : MonoBehaviour
{


    public class SpawnedObject
    {
        public GameObject go;
        public float lifetime;

        public SpawnedObject(GameObject newGo, float newLifeTime)
        {
            go = newGo;
            lifetime = newLifeTime;
        }
    }

    public GameObject[] zeppelin;

    public float timeToSpawn;
    public float speed;

    private float _currentTime;
    public List<SpawnedObject> _generatedZeppelin;
    private Camera _camera;
    private int _previousObjNumber;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _generatedZeppelin = new List<SpawnedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > timeToSpawn + Random.Range(-timeToSpawn/10.0f, timeToSpawn/10.0f))
        {

            int randomNumb = Random.Range(0, zeppelin.Length);
            
            while(_previousObjNumber == randomNumb)
                randomNumb = Random.Range(0, zeppelin.Length);

            _previousObjNumber = randomNumb;
            GameObject go = Instantiate(zeppelin[randomNumb], transform.position + Vector3.up * (Random .Range(-20.0f, 20.0f)), Quaternion.identity);
            _generatedZeppelin.Add(new SpawnedObject(go, 0.0f));
            _currentTime = 0.0f;
        }
        int i =0;
        while(i < _generatedZeppelin.Count)
        {
            SpawnedObject o = _generatedZeppelin[i];
            o.go.transform.position += 
                Vector3.right * ( speed + Random.Range(-speed/2.0f, speed/2.0f)) / 100.0f * Time.deltaTime 
                + Vector3.up * (Mathf.Sin(o.go.transform.position.x * 0.05f) * 0.05f)/2;
            o.lifetime += Time.deltaTime;
            if (o.lifetime > 15.0f && !o.go.GetComponentInChildren<Renderer>().isVisible)
            {
                Destroy(o.go);
                _generatedZeppelin.Remove(o);
            }
            else ++i;
        }
    }

    private void FixedUpdate()
    {
        
    }
}
