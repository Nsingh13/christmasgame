using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float runSpeed = 5f;             // Initial running speed
    public float maxRunSpeed = 15f;        // Maximum running speed
    public float speedIncreaseRate = 0.1f; // Rate at which speed increases over time

    public float jumpForce = 5f;           // Base jump force
    public float maxJumpForce = 10f;       // Maximum jump force for a long tap
    public float jumpChargeSpeed = 10f;    // Speed at which jump force increases
    private bool isGrounded = true;        // Check if the character is on the ground
    private float currentJumpForce;        // Current jump force while charging

    private Rigidbody2D rb;                // Reference to the Rigidbody2D component
    private bool isJumping;                // Check if the player is holding the screen
    private float jumpHoldTime;            // How long the screen is held

    public AudioSource SFX_audiosource;
    public AudioClip jumpSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Gradually increase running speed over time
        if (runSpeed < maxRunSpeed)
        {
            runSpeed += speedIncreaseRate * Time.deltaTime;
        }

        // Charge jump if the screen is being touched
        if (Input.GetMouseButtonDown(0)) // Touch or left mouse button
        {
            isJumping = true;
            jumpHoldTime = 0f;
        }

        if (Input.GetMouseButton(0) && isGrounded)
        {
            jumpHoldTime += Time.deltaTime;
            currentJumpForce = Mathf.Lerp(jumpForce, maxJumpForce, jumpHoldTime * jumpChargeSpeed);
        }

        if (Input.GetMouseButtonUp(0) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Constant running with the current speed
        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
            isGrounded = false; // Prevent double jumps
            isJumping = false;

            SFX_audiosource.PlayOneShot(jumpSound);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player lands on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
