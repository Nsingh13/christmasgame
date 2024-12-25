using UnityEngine;
using DG.Tweening; // Make sure DOTween is imported
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private RectTransform transitionRect; // Assign your RectTransform here in the Inspector

    public string nextSceneName; // Set this to the name of the scene you want to load

    private void Start()
    {
        // Call the start transition animation when the game starts
        StartTransition();
    }

    // Method for the start transition
    private void StartTransition()
    {
        if (transitionRect != null)
        {
            transitionRect.gameObject.SetActive(true);

            // Animate the width and height of the RectTransform
            transitionRect.sizeDelta = Vector2.zero; // Start at 0 width and height
            transitionRect.DOSizeDelta(new Vector2(2000, 2000), 1f) // Change to 2000x2000 over 1 second
                .SetEase(Ease.OutQuad) // Add easing for smooth animation
                .OnComplete(() => {
                    transitionRect.gameObject.SetActive(false);
                });
        }
        else
        {
            Debug.LogError("Transition RectTransform is not assigned!");
        }
    }

    // Method for the end transition
    public void EndTransition()
    {
        if (transitionRect != null)
        {
            transitionRect.gameObject.SetActive(true);

            // Animate the width and height of the RectTransform back to 0
            transitionRect.DOSizeDelta(Vector2.zero, 1f) // Shrink back to 0 width and height
                .SetEase(Ease.InQuad) // Add easing for smooth animation
                .OnComplete(() => SwitchScene()); // Switch scene after animation completes
        }
        else
        {
            Debug.LogError("Transition RectTransform is not assigned!");
        }
    }

    // Switches the scene after the DOTween animation is complete
    private void SwitchScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not set!");
        }
    }
}
