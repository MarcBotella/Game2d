using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    public GameObject player;
    void Start() { }
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x,
        player.transform.position.y,
        transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
