using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using JetBrains.Annotations;

public class MarshmallowStickController : MonoBehaviour
{
    private Camera mainCamera;
    public TextMeshProUGUI scoreText;

    private int marshmallowsLeft = 21;
    private bool isGameOver = false; // Safeguard to prevent multiple coroutines
    private bool gameWon = false;

    private PulsateEffect pulsate;

    public OpenGiftUI giftUI;

    public AudioSource SFX_audiosource;
    public AudioClip marshmallowChangeSound;
    public AudioClip marshmallowBurntSound;

    public SceneTransition sceneTransitioner;

    private void Start()
    {
        // Cache the main camera
        mainCamera = Camera.main;

        marshmallowsLeft = 21;

        pulsate = mainCamera.GetComponent<PulsateEffect>();
    }

    private void Update()
    {
        if (gameWon)
            return;

        // Check for touch or mouse input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            MoveStickToPosition(touch.position);

            if (pulsate.isPulsating)
                pulsate.StopPulsating();
        }
        else if (Input.GetMouseButton(0))
        {
            MoveStickToPosition(Input.mousePosition);

            if (pulsate.isPulsating)
                pulsate.StopPulsating();
        }
    }

    private void MoveStickToPosition(Vector3 screenPosition)
    {
        // Convert the screen position to world position
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane + 10f));
        
        // Update the stick's position (lock Z if needed)
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }

    public void UpdateScore()
    {
        marshmallowsLeft--;
        scoreText.text = marshmallowsLeft.ToString();

        SFX_audiosource.PlayOneShot(marshmallowChangeSound);

        if (marshmallowsLeft <= 0)
        {
            marshmallowsLeft = 0;
            gameWon = true;
            giftUI.ShowGiftPanel();
        }
    }

    public int GetMarshmallowsLeft()
    {
        return marshmallowsLeft;
    }

    public void GameOver()
    {
        if (isGameOver) return; // Prevent multiple calls to GameOver
        isGameOver = true;

        SFX_audiosource.PlayOneShot(marshmallowBurntSound);

        scoreText.color = Color.red;
        StartCoroutine(RestartCountdown());
    }


    private IEnumerator RestartCountdown()
    {
        // Countdown from 3
        // for (int i = 3; i > 0; i--)
        // {
        //     scoreText.text = $"Burnt! Try again in {i}...";
        //     yield return new WaitForSeconds(1f); // Wait for 1 second
        // }

        scoreText.text = $"Burnt!";
        yield return new WaitForSeconds(0.5f);

        // Restart the scene after the countdown
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        sceneTransitioner.nextSceneName = "Chapter1";
        sceneTransitioner.EndTransition();

    }
}
