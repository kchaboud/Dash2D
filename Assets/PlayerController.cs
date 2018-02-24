using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public LayerMask groundLayer;
    public string movingPlatformTag;
    public PlayerExternalVelocity externalVelocityManager;
    public Rigidbody2D rb;
    public Animator anim;
    public TrailRenderer trailRend;
    public ParticleSystem deathParticle;
    public ParticleSystem dashResetParticle;
    public Transform spawnPoint;
    public Image spawnFadeImage;
    public TimeManager timeManager;
    public SwordController sword;

    public PlayerInputs inputs = new PlayerInputs();
    private CameraWindow cam;

    public float maxMoveSpeed = 9f;
    public float groundAcceleration = 1f;
    public float airAcceleration = 1f;
    public float jumpSpeed = 10f;
    public float dashSpeed = 50f;

    private bool alive = true;
    private bool grounded;
    private string lastGroundedTag;
    private bool jumping;
    private int lastFrameGrounded;
    private bool ableToJump;
    private int lastFrameJumping;
    private bool ableToDash;
    private bool dashSlowMotion = false;
    private int dashing = 0; //0 not dashing, 1 to 1000
    private int lastDashing = 1; //1 to 1000
    private int lastDashingReset = 0; //0 & 1 & 2
    private bool fixedGravityScale = false;

    private bool dashResetState = false;
    private int dashCount = 0;

    private List<DashResetter> dashResettersUsed = new List<DashResetter>();
    private DashResetter dashResetterToUse;
    private Coroutine lastDashAction;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraWindow>();
    }


    private void Update()
    {
        inputs.UpdateInputs();

        FlipSprite();

        sword.Attack(inputs.sword);
    }

    private void FixedUpdate()
    {
        UpdateGroundedValue();
        if (!ableToDash && grounded)
        {
            ableToDash = true;
            dashResetParticle.Play();
        }
        ResetUsedDashResetters();
        //ResetDashCooldownWithResetter();

        ApplyMovement();

        if (inputs.IsNewDash() && ableToDash)
        {
            inputs.UseNewDash();
            ableToDash = false;
            if (dashSlowMotion)
            {
                //StopCoroutine(lastDashAction);
                timeManager.ResetTimeScale();
            }
            lastDashAction = StartCoroutine(DashAction());
            //ResetDashCooldownWithResetter();
        }

        if (dashing == 0)
        {
            jumping = externalVelocityManager.PlayerVelocityWithoutExternal().y > 0 && !grounded;
            ChangeGravityScale();
            //if (grounded && !jumping) rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        anim.SetBool("Running", inputs.horizontal != 0);
        anim.SetBool("Jump", jumping);
        anim.SetBool("Grounded", grounded);
    }

    private void ApplyMovement()
    {
        if (dashing != 0) return;

        //RUNNING
        float horizontalAcceleration = grounded ? (inputs.horizontal * groundAcceleration) : (inputs.horizontal * airAcceleration);
        float horizontalVelocity;
        Vector2 playerVelocity = externalVelocityManager.PlayerVelocityWithoutExternal();
        if (inputs.horizontal == 0 || horizontalAcceleration * playerVelocity.x < 0)
        {
            horizontalVelocity = 0;
        }
        else
        {
            horizontalVelocity = playerVelocity.x + horizontalAcceleration;
        }        
        if (horizontalVelocity > maxMoveSpeed) horizontalVelocity = maxMoveSpeed;
        if (horizontalVelocity < -maxMoveSpeed) horizontalVelocity = -maxMoveSpeed;

        //JUMP
        float verticalVelocity;
        if (ableToJump && inputs.IsNewJump())
        {
            inputs.UseNewJump();
            externalVelocityManager.ResetExternal();
            lastFrameJumping = 0;
            verticalVelocity = jumpSpeed;
            lastFrameGrounded = 10;
        }
        else if (lastFrameJumping < 10 && inputs.jump)
        {
            ++lastFrameJumping;
            verticalVelocity = jumpSpeed;
        }
        else if (!grounded)
        {
            lastFrameJumping = 100;
            verticalVelocity = playerVelocity.y;
        }
        else
        {
            verticalVelocity = 0;
        }

        //FINAL VELOCITY
        rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
    }

    private void FlipSprite()
    {
        if (inputs.horizontal != 0) GetComponent<SpriteRenderer>().flipX = inputs.horizontal < 0;
        sword.FlipSwordPosition(GetComponent<SpriteRenderer>().flipX);
    }

    private void ChangeGravityScale()
    {
        if (fixedGravityScale) return;
        if (!jumping && !grounded)
        {
            rb.gravityScale = 6f;
        }
        else if (grounded && !jumping)
        {
            rb.gravityScale = 0f;
        }
        else if (jumping || grounded)
        {
            rb.gravityScale = 4f;
        }
    }

    private void UpdateGroundedValue()
    {
        grounded = IsGrounded();
        if (grounded)
        { 
            OnGrounded();
        }
        lastFrameGrounded = grounded ? 0 : ++lastFrameGrounded;
        ableToJump = lastFrameGrounded < 4;
    }

    private bool IsGrounded()
    {
        //OLD CHECK if vertical speed is 0
        //if (externalVelocityManager.PlayerVelocityWithoutExternal().y != 0)
        //{
        //    return false;
        //}
        
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.3f;
        Vector2 offset = new Vector2(0.65f*1.25f, 0);

        RaycastHit2D hit = Physics2D.Raycast(position - offset, direction, distance, groundLayer);
        Debug.DrawRay(position - offset, direction * distance, Color.red);
        if (hit.collider != null)
        {
            lastGroundedTag = hit.collider.tag;
            ManageMovingPlateform(hit.collider.gameObject);
            return true;
        }
        hit = Physics2D.Raycast(position + offset, direction, distance, groundLayer);
        Debug.DrawRay(position + offset, direction * distance, Color.blue);
        if (hit.collider != null)
        {
            lastGroundedTag = hit.collider.tag;
            ManageMovingPlateform(hit.collider.gameObject);
            return true;
        }

        return false;
    }

    private void ManageMovingPlateform(GameObject o)
    {
        if (o.tag == movingPlatformTag)
        {
            externalVelocityManager.GroundedWithMovingPlatform(o);
        }
        else
        {
            externalVelocityManager.ResetExternal();
        }
    }

    private IEnumerator DashAction()
    {
        dashing = (lastDashing+1)%1000;
        lastDashing = dashing;
        trailRend.enabled = true;
        rb.gravityScale = 0;

        Vector3 direction = CalculateInputDirection();
     
        rb.velocity = direction * dashSpeed;
        yield return new WaitForSeconds(0.15f);       
        rb.velocity = Vector3.zero;
        dashing = 0;
        trailRend.enabled = false;
        fixedGravityScale = false;
    }

    private Vector3 CalculateInputDirection()
    {
        Vector3 direction;
        if (inputs.horizontal == 0 && inputs.vertical == 0)
        {
            direction = (new Vector3(inputs.lastHorizontal, inputs.lastVertical)).normalized;
        }
        else
        {
            direction = (new Vector3(inputs.lastHorizontal, inputs.lastVertical)).normalized;
        }
        return direction;
    }

    public void Die()
    {
        alive = false;
        deathParticle.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        sword.GetComponent<SpriteRenderer>().enabled = false;
        sword.swordTrail.enabled = false;
        trailRend.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        float alpha = 0;
        yield return new WaitForSeconds(0.5f);
        while (alpha < 1)
        {
            alpha += 0.1f;
            spawnFadeImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0.01f);
        }
        spawnFadeImage.color = new Color(0, 0, 0, 0);
        transform.position = spawnPoint.position;
        alive = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<SpriteRenderer>().flipX = false;
        sword.GetComponent<SpriteRenderer>().enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        alpha = 1;
        while (alpha > 0)
        {
            alpha -= 0.1f;
            spawnFadeImage.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void TriggerDashResetter(DashResetter resetter)
    {
        if (resetter.IsUsed()) return;
        dashResetterToUse = resetter;
        ResetDashCooldownWithResetter();
    }

    private void ResetDashCooldownWithResetter()
    {
        if (dashing > 0 && dashing != lastDashingReset && dashResetterToUse != null)
        {
            //temp
            StopCoroutine(lastDashAction);
            transform.position = dashResetterToUse.transform.position;
            rb.velocity = Vector3.zero;
            dashing = 0;
            fixedGravityScale = true;
            dashResetState = true;
            ++dashCount;
            StartCoroutine(StopDashResetStateAfterTimer(dashCount));
            //fin temp
            lastDashingReset = dashing;
            dashResetterToUse.Use();
            dashResettersUsed.Add(dashResetterToUse);
            ableToDash = true;
            dashSlowMotion = true;
            dashResetParticle.Play();
            dashResetterToUse = null;
            timeManager.StopResetTimeScale();
            timeManager.DoSlowMotion();
            dashSlowMotion = true;
        }
    }

    private IEnumerator StopDashResetStateAfterTimer(int dashCount)
    {
        yield return new WaitForSeconds(dashResetParticle.main.duration);
        if (this.dashCount == dashCount && dashResetState == true)
        {
            dashResetState = false;
            fixedGravityScale = false;
            trailRend.enabled = false;
            ableToDash = false;
            timeManager.ResetTimeScale();
        }

    }

    private void ResetUsedDashResetters()
    {
        if (grounded && dashResettersUsed.Count != 0)
        {
            foreach (DashResetter r in dashResettersUsed) r.Unuse();
            dashResettersUsed.Clear();
            lastDashingReset = 0;
        }
       
    }

    public bool IsDashing()
    {
        return dashing > 0;
    }

    public void OnGrounded()
    {
        cam.PlayerGrounded(lastGroundedTag);
    }
}