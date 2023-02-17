using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [Header("Basics")]
    Rigidbody2D rb;
    BoxCollider2D bc;
    Animator animator;

    [Header("Jump")]
    public float jumpForce;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    bool jumpRequest;

    [Header("Movement")]
    private float moveInput;
    public float moveSpeed;
    public float acceleration;
    public float decceleration;
    public float velPower;
    bool runRequest;

    [Header("GroundCheck")]
    [SerializeField] private LayerMask groundLayer;
    public float lastGroundedTime;

    [Header("FacingComponents")]
    [SerializeField] private float scale;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        lastGroundedTime -= Time.deltaTime;
    }

    void Update()
    {
        #region Inputs
        if (Input.GetButtonDown ("Jump") && isGrounded())
        {
            jumpRequest = true;
        }
        moveInput = Input.GetAxisRaw("Horizontal");
        #endregion
    }
    void FixedUpdate()
    {
        #region Jump

        if(rb.velocity.y < 0) {
            rb.gravityScale = fallMultiplier;
        } else if(rb.velocity.y > 0 && !Input.GetButton ("Jump")) {
            rb.gravityScale = lowJumpMultiplier;
        } else {
            rb.gravityScale = 1f;
        }

        if (jumpRequest)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpRequest = false;
        }
        #endregion

        #region Walk

        float targetSpeed = moveInput * moveSpeed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (MathF.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.right);
        #endregion

        #region Run Animation
        if (rb.velocity.x != 0)
        {
            animator.SetBool("Running" , true);
        }
        else
        {
            animator.SetBool("Running" , false);
        }
        #endregion 

        #region Facing
    
        if(rb.velocity.x >= 0.5)
        {
            transform.localScale = new Vector2 (scale , transform.localScale.y);
        }

        if(rb.velocity.x <= 0.5)
        {
            transform.localScale = new Vector2 (-scale , transform.localScale.y);
        }
    #endregion
    }

    #region GroundCheck

    private bool isGrounded ()
    {
        float extraHeightText = .03f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, bc.bounds.extents.y + extraHeightText, groundLayer);
        return raycastHit.collider != null;
    }
    #endregion

    #region Death

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    #endregion
}


