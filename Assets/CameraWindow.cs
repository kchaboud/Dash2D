using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWindow : MonoBehaviour
{
    public bool platformSnappingEnabled = true;
    public float changeSideDuration = 0.4f;
    public float platformSnappingDuration = 0.4f;
    public bool direction = true; //true -> right, false -> left
    public float directionOffset = 2f;
    public float windowWidth = 2f;
    public float verticalOffset = 0f;
    public float minimumX = -1000f;
    public float minimumY = -1000f;

    private Transform player;
    private bool changingSide = false;
    private Coroutine lastChangeSide;
    private Coroutine lastPlatformSnapping;
    private float lastPlayerYPos;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(player.position.x + directionOffset, player.position.y + verticalOffset, transform.position.z);
    }

    private void Update()
    {
        if (!changingSide)
        {
            //Horizontal
            float playerXOffset = player.position.x - (transform.position.x - (direction ? directionOffset : -directionOffset));
            if (playerXOffset > windowWidth)
            {
                if (!direction && !player.GetComponent<SpriteRenderer>().flipX)
                {
                    direction = true;
                    StartChangeSide();
                    return;
                }
                transform.position += new Vector3(playerXOffset - windowWidth, 0, 0);
            }
            else if (playerXOffset < -windowWidth)
            {
                if (direction && player.GetComponent<SpriteRenderer>().flipX)
                {
                    direction = false;
                    StartChangeSide();
                    return;
                }
                transform.position += new Vector3(playerXOffset + windowWidth, 0, 0);
            }
        }
     
        //Vertical
        if (player.transform.position.y < lastPlayerYPos && transform.position.y > minimumY)
        {
            transform.position -= new Vector3(0, lastPlayerYPos - player.transform.position.y, 0);
        }
        
        if(transform.position.x < minimumX)
        {
            transform.position = new Vector3(minimumX, transform.position.y, transform.position.z);
        }
        if (transform.position.y < minimumY)
        {
            transform.position = new Vector3(transform.position.x, minimumY, transform.position.z);
        }
        lastPlayerYPos = player.transform.position.y;
    }

    private IEnumerator ChangeSide()
    {
        changingSide = true;   
        float originalX = transform.position.x;
        int numberSteps = (int) (changeSideDuration / Time.fixedDeltaTime);
        int step = 0;
        while (step < numberSteps)
        {
            float goal = direction ? player.position.x + directionOffset - windowWidth : player.position.x - directionOffset + windowWidth;
            float xPos = Mathf.Lerp(originalX, goal, (float)++step/numberSteps);
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
        changingSide = false;
    }

    public void PlayerGrounded(string groundedTag)
    {
        lastPlayerYPos = player.transform.position.y;
        if (platformSnappingEnabled) StartPlatformSnapping();
    }
    private void StartPlatformSnapping()
    {
        if (lastPlatformSnapping != null)
        {
            StopCoroutine(lastPlatformSnapping);
        }
        lastPlatformSnapping = StartCoroutine(PlatformSnappingVertical());
    }

    private void StartChangeSide()
    {
        if (lastChangeSide != null)
        {
            StopCoroutine(lastChangeSide);
        }
        lastChangeSide = StartCoroutine(ChangeSide());
    }

    private IEnumerator PlatformSnappingVertical()
    {
        float originalY = transform.position.y;
        int numberSteps = (int)(platformSnappingDuration / Time.fixedDeltaTime);
        int step = 0;
        float goal = player.position.y + verticalOffset;
        while (step < numberSteps)
        {            
            float yPos = Mathf.Lerp(originalY, goal, (float)++step / numberSteps);
            transform.position = new Vector3(transform.position.x, yPos , transform.position.z);
            yield return new WaitForFixedUpdate();
        }
    }

}
