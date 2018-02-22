using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWindow : MonoBehaviour
{
    public float changeSideDuration = 0.4f;
    public bool direction = true; //true -> right, false -> left
    public float directionOffset = 2f;
    public float windowWidth = 2f;
    public float verticalOffset = 0f;

    private Transform player;
    private bool changingSide = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(player.position.x + directionOffset, player.position.y + verticalOffset, transform.position.z);
    }

    private void Update()
    {
        if (changingSide) return;

        float playerXOffset = player.position.x - (transform.position.x - (direction ? directionOffset : -directionOffset));
        if (playerXOffset > windowWidth)
        {
            if (!direction && !player.GetComponent<SpriteRenderer>().flipX)
            {
                direction = true;
                StartCoroutine(ChangeSide());
                return;
            }
            transform.position += new Vector3(playerXOffset - windowWidth, 0, 0);
        }
        else if (playerXOffset < -windowWidth)
        {
            if (direction && player.GetComponent<SpriteRenderer>().flipX)
            {
                direction = false;
                StartCoroutine(ChangeSide());
                return;
            }
            transform.position += new Vector3(playerXOffset + windowWidth, 0, 0);
        }
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
            Debug.Log(xPos);
            yield return new WaitForFixedUpdate();
        }
        changingSide = false;
    }

}
