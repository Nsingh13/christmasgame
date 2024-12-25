using UnityEngine;

public class OrnamentDrag : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    private Vector3 offset;    // Offset between ornament and mouse position
    private bool isDragging = false;
    private Collider2D treeCollider; // Tree's 2D collider

    void Start()
    {
        mainCamera = Camera.main;

        // Find the tree's collider (ensure the tree GameObject has a Collider2D)
        treeCollider = GameObject.FindGameObjectWithTag("Tree").GetComponent<Collider2D>();

        if (treeCollider == null)
        {
            Debug.LogError("Tree collider not found! Ensure the tree object has a Collider2D and is tagged 'Tree'.");
        }
    }

    void OnMouseDown()
    {
        // Calculate the offset between the ornament's position and the mouse position in world space
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        offset = transform.position - mouseWorldPosition;

        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (!isDragging || treeCollider == null) return;

        // Get the mouse position in world space and add the offset
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        Vector3 newPosition = mouseWorldPosition + offset;

        // Clamp the new position to the bounds of the tree's collider
        newPosition = ClampToTreeBounds(newPosition);

        // Set the ornament's position
        transform.position = newPosition;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Convert mouse position to world position (2D perspective)
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 0f; // Ensure we're working in 2D
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    private Vector3 ClampToTreeBounds(Vector3 position)
    {
        // Use the tree's 2D collider bounds to constrain movement
        Bounds treeBounds = treeCollider.bounds;

        float clampedX = Mathf.Clamp(position.x, treeBounds.min.x, treeBounds.max.x);
        float clampedY = Mathf.Clamp(position.y, treeBounds.min.y, treeBounds.max.y);

        // Return the clamped position (Z remains unchanged in 2D)
        return new Vector3(clampedX, clampedY, transform.position.z);
    }
}
