using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    public TrailRenderer swordTrail;
    public PlayerController player;
    public SwordSound sound;

    private int floatUp = 1;
    private Vector3 floatingMovement = new Vector3(0f, 0.2f, 0f);
    private Coroutine alternateFloatUpCo;
    private int state = 0; //0 repos, 1 first slash
    private bool toTheRight = true;

    private float xPos;
    private float yPos;
    private float zRotation;

    private void Start()
    {
        alternateFloatUpCo = StartCoroutine(AlternateFloatUp());
        xPos = transform.localPosition.x;
        yPos = transform.localPosition.y;
        zRotation = transform.eulerAngles.z;
    }

    private void Update()
    {
        if (state == 0) transform.position = transform.position + (floatingMovement * Time.deltaTime * floatUp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == 0) return;
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(gameObject);
        }
    }



    private IEnumerator AlternateFloatUp()
    { 
        while (true)
        {
            yield return new WaitForSeconds(0.65f);
            floatUp = (floatUp == 1) ? -1 : 1;
        }
    }

    public void FlipSwordPosition(bool flip)
    {
        if (state != 0) return;
        toTheRight = !flip;
        if (!flip)
        {
            GetComponent<SpriteRenderer>().flipY = true;
            transform.localPosition = new Vector3(xPos, yPos, 0);
            transform.eulerAngles = new Vector3(0, 0, zRotation);
        }
        else
        {
            GetComponent<SpriteRenderer>().flipY = false;
            transform.localPosition = new Vector3(-xPos, yPos, 0);
            transform.eulerAngles = new Vector3(0, 0, zRotation + 40f);
        }
    }

    public void Attack(bool swordInput)
    {
        if (!swordInput) return;
        if (state == 0) StartCoroutine(FirstSlash());

    }
    
    public IEnumerator FirstSlash()
    {
        state = 1;
        swordTrail.enabled = true;

        sound.PlaySwingSound();

        //Rotation and sword movement
        float rotationFactor = 10f;
        float rightFactor = 0.06f;
        float downFactor = 0.04f;
        if (toTheRight)
        {
            while (transform.eulerAngles.z < 260f || transform.eulerAngles.z > 340f)
            {
                transform.Rotate(Vector3.back, rotationFactor);
                transform.position = transform.position + Vector3.right * rightFactor + Vector3.down * downFactor;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (transform.eulerAngles.z > 280f || transform.eulerAngles.z < 200f)
            {
                transform.Rotate(Vector3.forward, rotationFactor);
                transform.position = transform.position + Vector3.left * rightFactor + Vector3.down * downFactor;
                yield return new WaitForSeconds(0.01f);
            }
        }
        
        while (player.IsDashing())
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.01f);
        }

        GetComponent<SpriteRenderer>().color = Color.white;
        state = 0;
        swordTrail.enabled = false;
    } 
}
