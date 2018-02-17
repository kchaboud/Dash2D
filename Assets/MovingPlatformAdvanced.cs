using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformAdvanced : MonoBehaviour {

    public GameObject platform;
    public Transform[] points; //size must be > 2
    public float speed;
    public enum Behaviour {loop, continuous}; //default loop

    private Rigidbody2D rb;
    private int currentGoal = 1;
    private Vector3 currentDirection;

    private void Start()
    {
        rb = platform.GetComponent<Rigidbody2D>();
        platform.transform.position = points[0].position;
        currentDirection = (points[1].position - points[0].position).normalized;
    }

    private void FixedUpdate()
    {
        Debug.Log(currentGoal);
        if (IsNextPointReached())
        {
            ChangeNextPoint();
        }

        rb.velocity = currentDirection * speed;
    }

    private bool IsNextPointReached()
    {
        float currentDistance = Vector2.Distance(platform.transform.position, points[currentGoal].position);
        float nextDistance = Vector2.Distance(platform.transform.position + (currentDirection * speed * Time.fixedDeltaTime), points[currentGoal].position);

        return currentDistance < nextDistance;
    }

    private int NextPoint()
    {
        return (currentGoal + 1) % points.Length;
    }

    private void ChangeNextPoint()
    {
        currentDirection = (points[NextPoint()].position - points[currentGoal].position).normalized;
        currentGoal = NextPoint();        
    }
}
