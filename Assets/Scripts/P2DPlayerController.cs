using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class P2DPlayerController : MonoBehaviour
{
    // Controller status
    [Header("Controller Status")]
    public bool enableControl = true;
    public AnimationCurve jumpCurve;

    // Component references 
    [Header("Component")]
    public Rigidbody2D rb;
    public Animator animator;

    // Physics settings
    [Header("Physcis Setting")]
    

    // Player movement configurations
    [Header("Player Setting")]
    public float walkSpeed = 5f;
    public float fallSpeed = 5f; // Custom FallSpeed value
    public float jumpForce = 20f;

    [Space]
    public bool jumpBtnDown = false;
    public bool enableHoldJump = false;
    public float jumpHoldForce = 5f;
    public float maxJumpHoldDuration = 0.2f;
    [SerializeField] private bool isJumping;
    [SerializeField] private float jumpHoldTimer;
    
    public int maxJump = 1;
    public int jumpCount = 0;



    // Ground check settings
    [Header("Ground Checking")]
    public bool isGrounded;    
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;


    private void Start()
    {
        Application.targetFrameRate = 60;
        // Auto assign Rigidbody2D if not set
        if (rb == null) { rb = GetComponent<Rigidbody2D>(); }
        if (animator == null) { animator = GetComponent<Animator>(); }

        StatusInit();
    }

    private void StatusInit()
    {
        isGrounded = false;
        jumpHoldTimer = maxJumpHoldDuration;
    }

    private void Update()
    {
        animator.SetFloat("HorizontalVelocity", rb.velocity.y);
        // Check if Controller is enabled
        if (!enableControl) return;

        MoveHandler();
        JumpHandler();

    }

    private void FixedUpdate()
    {
        
        if (!jumpBtnDown && !isJumping) { return; }
        if (jumpHoldTimer > 0f)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpHoldForce), ForceMode2D.Force);
            jumpHoldTimer -= Time.fixedDeltaTime;
        }
        else
        {
            
            isJumping = false;
        }
    }

    private void InputHandler()
    {

    }

    // Handle player Move
    private void MoveHandler()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalMove * walkSpeed, rb.velocity.y);

        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }


        if (rb.velocity.y < 0) // Apply custom Fall Speed
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
        }
    }

    // Handle player Jump
    private void JumpHandler()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded) { jumpCount = 0;  }
        if (jumpCount > maxJump - 1) { return; }

        // Handle basic jump input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;

            if (!enableHoldJump) { return; }
            isJumping = true;
            
            jumpHoldTimer = maxJumpHoldDuration;
        }

        // Handle hold jump input
        if (enableHoldJump && Input.GetButton("Jump") && isJumping)
        {
            jumpBtnDown = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = Vector2.zero;
            jumpBtnDown = false;
            isJumping = false;
        }
    }

    public void SimpleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void LinearJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            isJumping = true;
            jumpHoldTimer = maxJumpHoldDuration;
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpHoldTimer > 0f)
            {
                rb.AddForce(new Vector2(rb.velocity.x, jumpHoldForce), ForceMode2D.Force);
                jumpHoldTimer -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }
}
