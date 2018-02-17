using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Obsolete("Use MovingPlatformAdvanced instead", false)]
public class MovingPlatform : MonoBehaviour {

    public Vector3 defaultPos;
    public Vector3 finalPos;
    private Rigidbody2D rb;
    private bool toTheRight = true;
    private Vector2 rightVelocity = new Vector2(8f, 3f);
    private Vector2 leftVelocity = new Vector2(-8f, -3f);
    private Vector2 velocity = new Vector2(8f, 3f);

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultPos = transform.position;
        finalPos = defaultPos + Vector3.right * 18 + Vector3.up * 5;
       
    }

    void FixedUpdate ()
    {
        if (transform.position.x > finalPos.x && toTheRight)
        {
            toTheRight = false;
            velocity = leftVelocity;
        }
        else if (transform.position.x < defaultPos.x && !toTheRight)
        {
            toTheRight = true;
            velocity = rightVelocity;
        }

        rb.velocity = velocity;
    }

    public Vector2 Velocity() { return velocity; }
}
