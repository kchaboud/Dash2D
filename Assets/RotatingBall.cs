using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBall : MonoBehaviour {

    public bool invert = false;
    public float hSpeed = 3, vSpeed = 3; //horizontal & vertical speeds
    public float distance = 3; //distance between base and ball

    private GameObject ball;

    private void Start()
    {
        ball = transform.GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        ball.transform.localPosition = new Vector3((invert ? 1 : -1) * Mathf.Cos(Time.time * hSpeed),  Mathf.Sin(Time.time * vSpeed),0) * distance;
    }

}
