using UnityEngine;
using UnityEngine.SceneManagement; // For restarting the scene
using DG.Tweening;
using Unity.VisualScripting; // DoTween namespace

public class BombExplosion : MonoBehaviour
{
    public float glowDuration = 2f; // Time for the glowing effect (randomly between 2-4 seconds)
    public float explosionRadius = 2f; // Radius of the explosion
    public float explosionDuration = 0.5f; // Duration of the explosion effect

    private SpriteRenderer spriteRenderer; // For bomb visual changes

    public AudioSource SFX_audiosource;
    public AudioClip explodeAudio;

    public SceneTransition sceneTransitioner;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        glowDuration = Random.Range(0.1f, 0.5f); // Set a random glow time

        // Start the glow effect
        GlowRed();

        SFX_audiosource = GameObject.Find("Canvas_Main").GetComponent<AudioSource>();
        sceneTransitioner = GameObject.Find("SceneTransition").GetComponent<SceneTransition>();
    }

    void GlowRed()
    {
        // Tween the color to red repeatedly for the glow effect
        spriteRenderer.DOColor(Color.red, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => Explode());
        
        // Call Explode after the glow duration
        Invoke(nameof(Explode), glowDuration);
    }

    void Explode()
    {
        SFX_audiosource.PlayOneShot(explodeAudio);

        // Stop glowing
        DOTween.Kill(spriteRenderer);

        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        // Visual explosion effect: Tween the scale
        transform.DOScale(new Vector3(10f, 10f, 1f), explosionDuration)
            .SetEase(Ease.OutExpo)
            .OnComplete(() => {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                CheckForPlayerInRange(); // Check if the player is within range
            });

        // Optional: Flash the bomb to white during the explosion
        spriteRenderer.DOColor(Color.white, explosionDuration / 2)
            .SetLoops(2, LoopType.Yoyo);
    }

void CheckForPlayerInRange()
{
    bool playerFound = false;
    // Detect all objects within the explosion radius
    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

    foreach (Collider2D hit in hits)
    {
        if (hit.CompareTag("Player")) // If the player is within the radius
        {
            playerFound = true;

            // Get the player's Rigidbody2D component
            Rigidbody2D playerRb = hit.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.gameObject.transform.GetChild(14).SetParent(null);
                playerRb.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                playerRb.gravityScale = 15f;

                // Apply a torque to make the player spin
                float torqueMagnitude = 0.1f; // Adjust the torque as needed
                playerRb.AddTorque(torqueMagnitude, ForceMode2D.Force);
            }

            // After 1 second, reset the scene
            Invoke(nameof(RestartScene), 0.2f);
        }
    }

    if (!playerFound)
    {
        Destroy(gameObject); // Destroy the bomb object after exploding
    }
}


    void OnDrawGizmosSelected()
    {
        // Draw the explosion radius in the Scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

        void RestartScene()
    {
        // Reset the stack and destroy stacked gifts
        Gift.ResetStack();
        DOTween.KillAll(); // Ensure all tweens are killed
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        sceneTransitioner.nextSceneName = "Chapter3";
        sceneTransitioner.EndTransition();
    }
}
