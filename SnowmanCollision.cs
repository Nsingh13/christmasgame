using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnowmanCollision : MonoBehaviour
{
    public AudioSource SFX_audiosource;
    public AudioClip hitSound;

    public SceneTransition sceneTransitioner;

    private void Start()
    {
        sceneTransitioner = GameObject.Find("SceneTransition").GetComponent<SceneTransition>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            BoxCollider2D playerCollider = collision.gameObject.GetComponent<BoxCollider2D>();
            CameraFollow camFollow = Camera.main.gameObject.GetComponent<CameraFollow>();
            DistanceTracker distTracker = Camera.main.gameObject.GetComponent<DistanceTracker>();

            if (playerRb != null && playerCollider != null && camFollow.enabled == true)
            {
                camFollow.enabled = false;
                distTracker.enabled = false;
                
                // Turn off freeze rotation
                playerRb.freezeRotation = false;

                // Disable the player's collider
                playerCollider.enabled = false;

                // Optional: Add rotation or force for dramatic effect
                playerRb.AddTorque(50f); // Spins the player

                SFX_audiosource.PlayOneShot(hitSound);
            }

            // Optionally, trigger a Game Over screen or logic here
            RestartScene();

        }
    }

    private void RestartScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        DOTween.KillAll();
        sceneTransitioner.nextSceneName = "Chapter2";
        sceneTransitioner.EndTransition();
    }
}
