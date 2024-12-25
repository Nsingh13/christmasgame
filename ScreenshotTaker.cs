using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    // Set the file name and path for the screenshot
    public string screenshotFileName = "screenshot.png";
    public string screenshotFolder = "Screenshots";

    void Update()
    {
        // Check for key press to take screenshot (e.g., pressing the "P" key)
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        // Create the screenshot folder if it doesn't exist
        if (!System.IO.Directory.Exists(screenshotFolder))
        {
            System.IO.Directory.CreateDirectory(screenshotFolder);
        }

        // Define the full file path
        string filePath = System.IO.Path.Combine(screenshotFolder, screenshotFileName);

        // Capture and save the screenshot
        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log("Screenshot taken and saved to: " + filePath);
    }
}
