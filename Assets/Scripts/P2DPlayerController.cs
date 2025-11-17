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
    public float Peak_y;
    public float LastForce_y;

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
        // Auto assign Rigidbody2D if not set
        if (rb == null) { rb = GetComponent<Rigidbody2D>(); }

        StatusInit();
    }

    private void StatusInit()
    {
        isGrounded = false;
        jumpHoldTimer = maxJumpHoldDuration;
        Peak_y = 0;
    }

    private void Update()
    {
        // Check if Controller is enabled
        if (!enableControl) return;

        MoveHandler();
        JumpHandler();

        Peak_y = Mathf.Max(Peak_y, transform.position.y);

    }

    private void FixedUpdate()
    {
        
    }

    private void InputHandler()
    {

    }

    // Handle player Move
    private void MoveHandler()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalMove * walkSpeed, rb.velocity.y);

        if (rb.velocity.y < 0.1) // Apply custom Fall Speed
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
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;

            if (!enableHoldJump) { return; }
            isJumping = true;
            
            jumpHoldTimer = maxJumpHoldDuration;
        }

        // Handle hold jump input
        if (enableHoldJump && Input.GetButton("Jump") && isJumping)
        {
            if (jumpHoldTimer > 0f)
            {
                LastForce_y = -4;
                rb.AddForce(new Vector2(rb.velocity.x, jumpHoldForce), ForceMode2D.Impulse);
                jumpHoldTimer -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                LastForce_y = Mathf.Max(LastForce_y, transform.position.y);
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
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
