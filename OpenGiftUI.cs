using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class OpenGiftUI : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup panel;         // The panel that fades in
    public RectTransform gift;        // The gift object that slides and shakes
    public Image giftContent;         // The revealed content image
    public TextMeshProUGUI giftDescription;      // The description text

    [Header("Animation Settings")]
    public float fadeDuration = 0.5f; // Duration of the panel fade-in
    public float slideDuration = 1f;  // Duration of the gift sliding in
    public float shakeDuration = 0.5f; // Duration of the gift shaking
    public float shakeStrength = 30f; // Intensity of the shake
    public int shakeVibrato = 10;     // Vibration count for the shake

    private bool isGiftOpen = false;  // Tracks if the gift has been opened

    public SceneTransition sceneTransitioner;

    public AudioSource SFX_audiosource;
    public AudioClip openGiftSound;
    public AudioClip swooshSound;

    void Start()
    {
        // Ensure initial states
        panel.alpha = 0;
        panel.gameObject.SetActive(false);
        gift.localPosition = new Vector3(0, Screen.height + 100, 0); // Start off-screen
        giftContent.gameObject.SetActive(false);
        giftDescription.gameObject.SetActive(false);
    }

    public void ShowGiftPanel()
    {
        // Fade in the panel and slide the gift
        panel.gameObject.SetActive(true);
        panel.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            SFX_audiosource.PlayOneShot(swooshSound);

            gift.DOLocalMove(Vector3.zero, slideDuration).SetEase(Ease.OutBack);
        });
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect user taps
        {
            if (!isGiftOpen)
            {
                OpenGift();
            }
            else
            {
                ReturnToMainMenu();
            }
        }
    }

    private void OpenGift()
    {
        isGiftOpen = true;

        switch(SceneManager.GetActiveScene().name)
        {
            case "Chapter1":
                PlayerPrefs.SetInt("GiftsOpened", 1);
                break;
            case "Chapter2":
                PlayerPrefs.SetInt("GiftsOpened", 2);
                break;
            case "Chapter3":
                PlayerPrefs.SetInt("GiftsOpened", 3);
                break;
        }

        PlayerPrefs.Save();
        
        // Shake the gift and reveal content
        gift.DOShakeRotation(shakeDuration, shakeStrength, shakeVibrato)
            .OnComplete(() =>
            {
                gift.gameObject.SetActive(false);
                giftContent.gameObject.SetActive(true);
                giftDescription.gameObject.SetActive(true);

                // Add a slight pop effect for the gift content
                giftContent.transform.DOScale(1.2f, 0.3f).SetLoops(2, LoopType.Yoyo);

                SFX_audiosource.PlayOneShot(openGiftSound);
            });
        
    }

    private void ReturnToMainMenu()
    {
        // Return to main menu scene
        //SceneManager.LoadScene("Menu"); // Replace with your main menu scene name

        sceneTransitioner.nextSceneName = "Menu";
        sceneTransitioner.EndTransition();
    }
}
