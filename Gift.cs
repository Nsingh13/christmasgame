using UnityEngine;
using TMPro; // Assuming you're using TextMeshPro for the clue display
using DG.Tweening; // For DOTween animations
using System.Collections;
using System.Collections.Generic;


public class Gift : MonoBehaviour
{
    public GameObject bombPrefab; // Reference to the bomb prefab
    public GameObject cluePrefab; // Reference to the clue prefab
    public int bombCount = 0; // Number of bombs around this gift
    public Vector2Int gridPosition; // This gift's position in the grid
    public bool isBomb = false; // Flag to check if this gift contains a bomb

    private bool isOpened = false; // Prevents multiple interactions with the same gift
    private static int stackCount = 0; // Tracks the number of gifts stacked
    private static Transform playerHand; // Reference to the player's hand for stacking

    // Queue to store gifts to process
    private static Queue<Gift> giftQueue = new Queue<Gift>();
    private static bool isProcessingQueue = false;

    public AudioSource SFX_audiosource;
    public AudioClip stackGiftSound;

    void Start()
    {
        // Find and cache the player's hand Transform if not already done
        if (playerHand == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerHand = player.transform.Find("Hand"); // Ensure the "Hand" child exists
        }
    }

    public static int GetStackCount()
    {
        return stackCount;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpened) return; // Prevent further interaction if opened or max stack reached

        if (stackCount >= 5)
        {
            RejectGift(); // Visual indicator when stack is full
            return;
        }

        // Check if Santa interacted with the gift
        if (collision.CompareTag("Player"))
        {
            giftQueue.Enqueue(this); // Add the gift to the queue
            SFX_audiosource.PlayOneShot(stackGiftSound);
            if (!isProcessingQueue)
            {
                StartCoroutine(ProcessGiftQueue()); // Start processing the queue if it's not already
            }
        }
    }

    // Coroutine to process the gift interactions one at a time
    private IEnumerator ProcessGiftQueue()
    {
        isProcessingQueue = true;

        // Process each gift in the queue
        while (giftQueue.Count > 0)
        {
            Gift gift = giftQueue.Dequeue();
            gift.isOpened = true;  // Mark the gift as opened
            gift.OpenGift();  // Perform the gift interaction (opening, stacking, etc.)

            // Wait for the current gift's animation to finish before processing the next one
            yield return new WaitForSeconds(0.2f); // Adjust the time for a smooth transition
        }

        isProcessingQueue = false;
    }

    void OpenGift()
    {
        stackCount++;

        // Scale the gift to half its size
        transform.DOScale(transform.localScale * 0.5f, 0.1f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            StackGift();
        });

        // If this is a bomb, spawn the bomb
        if (isBomb)
        {
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // Spawn the clue and update its text
            if (bombCount > 0)
            {
                GameObject clue = Instantiate(cluePrefab, transform.position, Quaternion.identity);
                TextMeshPro clueText = clue.GetComponentInChildren<TextMeshPro>();
                if (clueText != null)
                {
                    clueText.text = bombCount.ToString(); // Update the clue text with the bomb count
                }
            }
        }
    }

    private void StackGift()
    {
        if (playerHand == null) return;

        // Ensure the stack count is updated before positioning the gift
        Vector3 stackPosition = new Vector3(0f, stackCount * 0.1f, 0f); // Calculate the new stack position

        // Set the gift's parent to the player's hand
        transform.SetParent(playerHand);

        // Immediately align the gift's local position (relative to player hand)
        transform.localPosition = stackPosition;

        // Debugging stack position
        Debug.Log("Stack position (local): " + stackPosition);

        // Tween to move the gift to the player's hand, updating position while the player moves
        transform.DOMove(playerHand.position + stackPosition, 0.1f)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                // Update the target position dynamically while the animation is running
                // Make sure the animation moves based on the current position of the player's hand.
                Vector3 updatedPosition = playerHand.position + stackPosition;
                transform.position = updatedPosition;
            });
    }

    void RejectGift()
    {
        // Scale down slightly
        transform.DOScale(transform.localScale * 0.8f, 0.15f).SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                // Shake the rotation gently
                transform.DORotate(new Vector3(0, 0, 8f), 0.15f).SetLoops(3, LoopType.Yoyo).SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        // Scale back to the original size
                        transform.DOScale(transform.localScale / 0.8f, 0.15f).SetEase(Ease.OutExpo);
                        transform.rotation = Quaternion.identity; // Reset rotation to default
                    });
            });
    }

    public static void ResetStack()
    {
        stackCount = 0; // Reset the stack count
        // Optionally, destroy all stacked gifts
        foreach (Transform child in playerHand)
        {
            Destroy(child.gameObject);
        }
    }

    public static bool IsHoldingGifts()
    {
        return stackCount > 0; // Returns true if there are stacked gifts
    }
}
