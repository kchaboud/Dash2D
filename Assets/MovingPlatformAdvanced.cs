using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformAdvanced : MonoBehaviour {

    public GameObject platform;
    public Transform[] points; //size must be > 2
    public float speed;
        public enum Behaviour {loop, backforth}; //loop: 1->2->3->1->2... backforth: 1->2->3->2->1->2...
    public Behaviour behaviour = Behaviour.loop;

    private Rigidbody2D rb;
    private int currentGoal = 1;
    private Vector3 currentDirection;
    private bool backforthGo = true; //go = true, return = false

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
        if (behaviour == Behaviour.loop) return NextPointWithLoop();
        else return NextPointWithBackforth();
    }

    private int NextPointWithLoop()
    {
        return (currentGoal + 1) % points.Length;
    }

    private int NextPointWithBackforth()
    {
        //BackforthGo change
        if (backforthGo && (currentGoal + 1) == points.Length)
        {
                backforthGo = false;
        }
        else if (!backforthGo && (currentGoal - 1) == -1)
        {
            backforthGo = true;
        }

        //Return next point
        if (backforthGo) return currentGoal + 1;
        else return currentGoal - 1;
    }

    private void ChangeNextPoint()
    {
        currentDirection = (points[NextPoint()].position - points[currentGoal].position).normalized;
        currentGoal = NextPoint();        
    }
}
