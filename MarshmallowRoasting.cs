using UnityEngine;
using DG.Tweening;
using TMPro;

public class MarshmallowRoasting : MonoBehaviour
{
    public Transform fireTransform; // Reference to the fire GameObject
    public SpriteRenderer marshmallowSprite; // The marshmallow sprite renderer
    public Gradient roastingGradient; // Gradient for marshmallow colors (white -> brown -> black)
    public Transform marshmallowTransform; // The marshmallow Transform for animations
    public float roastingRadius = 2f; // Distance within which roasting starts
    public Vector3 spawnOffset = new Vector3(0, 2, 0); // Offset for new marshmallow spawn position

    private float roastingProgress = 0f; // Tracks the roasting progress (0 = raw, 1 = perfect, >1 = burned)
    private bool hasWon = false; // Flag to track if the player already won
    private float roastingSpeedMultiplier; // Random roasting speed for the marshmallow

    public MarshmallowStickController controller;

    public static int activeMarshmallows = 1;

    private void Start()
    {
        // Initialize the first marshmallow with a random roasting speed
        SetRandomRoastingSpeed();

        controller = this.transform.parent.gameObject.GetComponent<MarshmallowStickController>();
    }

    private void Update()
    {
        // Calculate distance to fire
        float distanceToFire = Vector3.Distance(marshmallowTransform.position, fireTransform.position);

        if (distanceToFire <= roastingRadius)
        {
            // Increase roasting progress based on proximity and roasting speed
            float roastingSpeed = roastingSpeedMultiplier * (1 - (distanceToFire / roastingRadius));
            roastingProgress += roastingSpeed * Time.deltaTime;
        }

        // Clamp roasting progress (stays in range 0 to 2)
        roastingProgress = Mathf.Clamp(roastingProgress, 0f, 2f);

        // Update marshmallow color
        UpdateMarshmallowColor();

        // Check for roasting phase
        CheckRoastingPhase(distanceToFire);
    }

    private void UpdateMarshmallowColor()
    {
        // Map roasting progress to gradient color
        marshmallowSprite.color = roastingGradient.Evaluate(roastingProgress / 2f);
    }

    private void CheckRoastingPhase(float distanceToFire)
    {
        if (roastingProgress >= 1.5f)
        {
            // Marshmallow burned
            //Debug.Log("Burned! Try again.");

            controller.GameOver();
        }
        else if (roastingProgress >= 1f && roastingProgress < 1.5f)
        {
            // Perfectly roasted zone
            //Debug.Log("Perfectly roasted! Pull away now!");

            // Check if player pulls the stick away (out of roasting radius)
            if (distanceToFire > roastingRadius && !hasWon)
            {
                //Debug.Log("You won! Perfect marshmallow!");
                hasWon = true; // Prevent multiple win messages

                // Update score
                controller.UpdateScore();

                activeMarshmallows--;

                if (activeMarshmallows == 0)
                {
                    int numMarshmallows = 1;

                    int marshmallowsLeft = controller.GetMarshmallowsLeft();

                    if (marshmallowsLeft <= 15 && marshmallowsLeft >= 2)
                    {
                        numMarshmallows = Random.Range(1, 3);
                    }

                    // Trigger marshmallow replacement
                    ReplaceMarshmallow(numMarshmallows);
                    activeMarshmallows = numMarshmallows;
                }
                else
                {
                    RemoveMarshmallow();
                }
            }
        }
        else if (roastingProgress < 1f)
        {
            // Marshmallow is still raw
            //Debug.Log("Still raw. Move closer to the fire.");
        }
    }

private void RemoveMarshmallow()
{
        // Scale down the current marshmallow
    marshmallowTransform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
    {
        // Move old marshmallow out of view
        marshmallowTransform.gameObject.SetActive(false);

    });
}

private void ReplaceMarshmallow(int marshmallowCount = 1)
{
    // Scale down the current marshmallow
    marshmallowTransform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
    {
        // Reset roasting progress and win flag
        roastingProgress = 0f;
        hasWon = false;

        // Move old marshmallow out of view
        marshmallowTransform.gameObject.SetActive(false);

        Transform previousMarshmallow = null;

        // Loop to create the specified number of marshmallows
        for (int i = 0; i < marshmallowCount; i++)
        {
            // Create a new marshmallow
            Transform newMarshmallow = Instantiate(marshmallowTransform, marshmallowTransform.parent);
            newMarshmallow.localPosition = spawnOffset; // Spawn above the stick
            newMarshmallow.localScale = Vector3.zero; // Start invisible
            newMarshmallow.gameObject.SetActive(true);

            // Determine the position
            float yOffset = (i == 0) ? 0.5f : (0.5f); // Push the second marshmallow down
            newMarshmallow.DOScale(new Vector3(0.87f, 0.071f, 0f), 0.5f); // Scale up
            newMarshmallow.DOLocalMove(new Vector3(0f, yOffset, 0f), 0.5f); // Position vertically

            // Assign a random roasting speed for each marshmallow
            SetRandomRoastingSpeed();
            //Debug.Log($"Marshmallow {i + 1} Roasting Speed: {roastingSpeedMultiplier}");

            // Update primary marshmallowTransform to the first one
            if (i == 0)
            {
                marshmallowTransform = newMarshmallow;
            }
            else if (previousMarshmallow != null)
            {
                // Push the previous marshmallow down
                previousMarshmallow.DOLocalMoveY(yOffset - 0.15f, 0.5f);
            }

            // Track the current marshmallow
            previousMarshmallow = newMarshmallow;
        }
    });
}



    private void SetRandomRoastingSpeed()
    {
        // Set a random speed multiplier between 0.5x and 4x
        roastingSpeedMultiplier = Random.Range(0.5f, 5f);
        //Debug.Log("New marshmallow roasting speed: " + roastingSpeedMultiplier);
    }
}