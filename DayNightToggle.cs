using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DayNightToggle : MonoBehaviour
{
    // The two colors for day and night
    public Color dayColor = Color.cyan;
    public Color nightColor = Color.black;

    // UI button and icon references
    public Button toggleButton;
    public Image buttonIcon;

    // Sun and Moon sprites
    public Sprite sunIcon;
    public Sprite moonIcon;

    // Camera reference
    public Camera mainCamera;
    private bool isDay = true;

    public GameObject treeLight;

    public ParticleSystem SnowParticles;
    public GameObject Stars;
    private void Awake()
    {
        // Set the button onClick listener
        toggleButton.onClick.AddListener(ToggleDayNight);

        // Set initial camera color and button icon
        //mainCamera.backgroundColor = dayColor;
        buttonIcon.sprite = sunIcon;
        //treeLight.SetActive(false);
    }

    void ToggleDayNight()
    {
        // Toggle the state between day and night
        isDay = !isDay;

        // Determine the target color based on the current state
        Color targetColor = isDay ? dayColor : nightColor; 

        // Perform the color transition with DoTween
        //mainCamera.backgroundColor = targetColor;
        mainCamera.DOColor(targetColor, 2f).SetEase(Ease.OutQuad);

        // Change the button icon based on the time of day
        buttonIcon.sprite = isDay ? sunIcon : moonIcon;

        treeLight.SetActive(!isDay);

        bool showSnow = isDay;

        if (showSnow)
        {
            Stars.SetActive(false);
            SnowParticles.Play();
        } else
        {
            SnowParticles.Stop();
            Stars.SetActive(true);
        }
    }

    public void SwitchToNightImmediate()
    {
        mainCamera.backgroundColor = nightColor;
        SnowParticles.Stop();
        Stars.SetActive(true);
        treeLight.SetActive(true);
        isDay = false;
    }

    public void DayNightToggleExternalCall()
    {
        ToggleDayNight();
    }
}
