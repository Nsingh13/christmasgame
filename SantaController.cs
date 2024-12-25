using UnityEngine;

public class SantaController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed of Santa
    private Vector2 moveDirection; // Direction of movement
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        // Get the Animator component attached to Santa
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Move Santa based on the current direction
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Update animation state
        if (moveDirection != Vector2.zero)
        {
            animator.SetBool("isWalking", true); // Trigger walk animation
        }
        else
        {
            animator.SetBool("isWalking", false); // Trigger idle animation
        }
    }

    // Called by UI buttons to set movement direction
    public void MoveUp() => moveDirection = Vector2.up;
    public void MoveDown() => moveDirection = Vector2.down;
    public void MoveLeft() => moveDirection = Vector2.left;
    public void MoveRight() => moveDirection = Vector2.right;

    // Called when no button is pressed (stop moving)
    public void StopMoving() => moveDirection = Vector2.zero;
}
