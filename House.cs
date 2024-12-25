using UnityEngine;
using DG.Tweening;

public class House : MonoBehaviour
{
    private bool isAnimating = false; // Prevent re-triggering during animation
    private Vector3 originalPosition; // Store the original position of the house
    private Vector3 originalScale; // Store the original scale of the house

    public AudioSource SFX_audiosource;
    public AudioClip deliverSound;

    private void Start()
    {
        // Cache the house's original position and scale
        originalPosition = transform.position;
        originalScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAnimating && Gift.IsHoldingGifts())
        {
            // Start the animation and prevent further triggers
            isAnimating = true;

            // Animate the house
            JoyfulAnimation();

            // Reset the stack and destroy stacked gifts
            Gift.ResetStack();
        }
    }

    void JoyfulAnimation()
    {
        // Step 1: Jump up with a subtle ease-in and slight bounce
        transform.DOMoveY(originalPosition.y + 0.3f, 0.2f).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                SFX_audiosource.PlayOneShot(deliverSound);
                
                // Step 2: Shake gently to add excitement
                transform.DOShakeRotation(0.2f, strength: new Vector3(0, 0, 7), vibrato: 4, randomness: 20)
                    .OnComplete(() =>
                    {
                        // Step 3: Smoothly return to the original position
                        transform.DOMoveY(originalPosition.y, 0.2f).SetEase(Ease.OutQuad)
                            .OnComplete(() => isAnimating = false); // Allow re-triggering
                    });
            });
    }
}
