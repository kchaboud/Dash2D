﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform player;
    private float leftBoundary;
    private float rightBoundary;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //if (GetComponent<Camera>().aspect > 2.3) transform.position += Vector3.right * 6.7f;
        //else if (GetComponent<Camera>().aspect < 1.4) transform.position -= Vector3.right * 5.3f;
        //leftBoundary = transform.position.x;
        //rightBoundary = leftBoundary + 100;
    }

    private void LateUpdate()
    {
        //float newPosition = player.position.x;
        //if (newPosition < leftBoundary) newPosition = leftBoundary;
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
