using UnityEngine;
using UnityEngine.Events;

public class PulsateEffect : MonoBehaviour
{
    public GameObject targetObject; // The GameObject to pulsate (assign in Inspector)

    [SerializeField]
    private Material normalMaterial; // The material without an outline
    [SerializeField]
    private Material outlineMaterial; // The outline material

    [SerializeField]
    private float pulsateSpeed = 1f; // Speed of the pulsation
    [SerializeField]
    private float pulsateScale = 1.1f; // Scale multiplier for pulsation

    [System.Serializable]
    public class ClickedEvent : UnityEvent<string> { }
    public ClickedEvent onClicked; // Callback event for when the object is clicked

    private Vector3 originalScale;
    public bool isPulsating = true;

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("No target object assigned. Please assign a GameObject to pulsate.");
            return;
        }

        // Store the original scale of the target object
        originalScale = targetObject.transform.localScale;

        SetOutline();
    }

    public void SetOutline()
    {
        // Set the outline material as the initial material
        var spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && outlineMaterial != null)
        {
            spriteRenderer.material = outlineMaterial;
        }

        // make child material normal as well if needed
        var spriteRenderer2 = targetObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (spriteRenderer2 != null)
        {
            spriteRenderer2.material = outlineMaterial;
        }
    }

    void Update()
    {
        if (isPulsating && targetObject != null)
        {
            // Calculate pulsate scale
            float scale = Mathf.Lerp(1f, pulsateScale, (Mathf.Sin(Time.time * pulsateSpeed) + 1f) / 2f);
            targetObject.transform.localScale = originalScale * scale;
        }

        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast from the mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // Check if the target object was clicked
            if (hit.collider != null && hit.collider.gameObject == targetObject && isPulsating)
            {
                Debug.Log("Item clicked");

                StopPulsating();
                // Trigger the callback event
                onClicked.Invoke(targetObject.name);
            }
        }
    }

    public void StopPulsating()
    {
        isPulsating = false;

        // Reset the object to its original scale
        targetObject.transform.localScale = originalScale;

        // Set the normal material
        var spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && normalMaterial != null)
        {
            spriteRenderer.material = normalMaterial;
        }

        // make child material normal as well if needed
        var spriteRenderer2 = targetObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (spriteRenderer2 != null)
        {
            spriteRenderer2.material = normalMaterial;
        }
    }
}
