using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening;

public class DistanceTracker : MonoBehaviour
{
    public TextMeshProUGUI distanceText;  // Reference to the TextMeshPro UI Text
    public Transform player;             // Reference to the player transform
    public float distanceMultiplier = 0.01f; // Multiplier to convert units to km (adjust as needed)
    public float startingDistance = 100f; // Starting distance in km (100 km)

    private float startingX;             // Starting position of the player (x-coordinate)

    public IcicleSpawner icicles;

    public OpenGiftUI giftUI;

    void Start()
    {
        // Store the player's starting x-coordinate
        if (player != null)
        {
            startingX = player.position.x;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate distance covered by the player
            float distanceCovered = (player.position.x - startingX) * distanceMultiplier;

            // Calculate remaining distance
            float remainingDistance = startingDistance - distanceCovered;

            // Update the UI text with the remaining distance
            distanceText.text = $"{remainingDistance:F1} km";

            // Enable icicle spawner if past 70 and disabled
            if (remainingDistance <= 50 && icicles.enabled == false)
            {
                icicles.enabled = true;
            }

            // If the remaining distance is 0 or less, you can add a completion condition
            if (remainingDistance <= 0 && player.gameObject.activeInHierarchy)
            {
                distanceText.text = "Goal Reached!";
                player.gameObject.SetActive(false);
                giftUI.ShowGiftPanel();
            }
        }
    }
}
