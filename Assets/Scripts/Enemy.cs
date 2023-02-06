using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 destination;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        iTween.MoveTo (gameObject, iTween.Hash ("position", destination, "time", time, "easetype", iTween.EaseType.easeInOutSine, "looptype", iTween.LoopType.pingPong));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
