using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExternalVelocity : MonoBehaviour {

    public LayerMask movingPlatformMask;
    public PlayerController player;
    private Rigidbody2D rb;
    private Vector2 externalVelocity = Vector2.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public Vector2 PlayerVelocityWithoutExternal()
    {
        return rb.velocity - externalVelocity;
    }

    public void GroundedWithMovingPlatform(GameObject o)
    {
        externalVelocity = o.GetComponent<Rigidbody2D>().velocity;
    }

    public void ResetExternal()
    {
        externalVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (!player.IsDashing()) rb.velocity += externalVelocity;
        else externalVelocity = Vector2.zero;

    }

}
