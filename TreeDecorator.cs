using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeDecorator : MonoBehaviour
{
    // References for the three levels of tree decoration
    public Button[] colorButtons;       // First level (ornaments)
    public Button[] decorButtons;      // Second level (decorations)
    public Button[] starButtons;       // Third level (star)

    // Lists to store selected buttons for each level
    private List<Button> selectedColorButtons = new List<Button>();
    private List<Button> selectedDecorButtons = new List<Button>();
    private List<Button> selectedStarButton = new List<Button>();

    // Selected data for tree decoration
    public List<string> selectedColors = new List<string>(); // Ornaments
    public List<string> selectedDecors = new List<string>(); // Second level (decor strings or IDs)
    public string selectedStar; // Final level (star string or ID)

    public ListSaver lists;
    public GameManager GM;

    // Tree decor items
    public Sprite[] ornamentSprites;
    public Sprite[] decorSprites;
    
    public SpriteRenderer[] ornaments_color1;
    public SpriteRenderer[] ornaments_color2;

    public GameObject decorMainGO;
    public SpriteRenderer[] decorOrnaments_1;
    public SpriteRenderer[] decorOrnaments_2;

    public Transform star;
    public Vector3 smallStarScale;
    public Vector3 mediumStarScale;
    public Vector3 largeStarScale;

    // --- End tree decor items ---

    void Start()
    {
        // Initialize listeners for the first level
        foreach (var button in colorButtons)
        {
            button.onClick.AddListener(() => OnButtonClick(button, selectedColorButtons, selectedColors, 2));
        }

        // Initialize listeners for the second level
        foreach (var button in decorButtons)
        {
            button.onClick.AddListener(() => OnButtonClick(button, selectedDecorButtons, selectedDecors, 2));
        }

        // Initialize listeners for the third level
        foreach (var button in starButtons)
        {
            button.onClick.AddListener(() => OnButtonClick(button, selectedStarButton, out selectedStar, 1));
        }
    }

    // Generic method for button click handling (for List<Color>)
    private void OnButtonClick(Button clickedButton, List<Button> selectedButtons, List<Color> selectedData, int maxSelections)
    {
        var outline = clickedButton.GetComponent<Outline>();

        if (selectedButtons.Contains(clickedButton))
        {
            // Unselect the button
            selectedButtons.Remove(clickedButton);
            outline.enabled = false;
        }
        else
        {
            // Ensure the selection count doesn't exceed the maximum allowed
            if (selectedButtons.Count >= maxSelections)
            {
                Debug.Log($"You can only select up to {maxSelections} options.");
                return;
            }

            // Select the button
            selectedButtons.Add(clickedButton);
            outline.enabled = true;
        }

        // Update the data for the level
        UpdateSelectedColors(selectedButtons, selectedData);
    }

    // Generic method for button click handling (for List<string>)
    private void OnButtonClick(Button clickedButton, List<Button> selectedButtons, List<string> selectedData, int maxSelections)
    {
        var outline = clickedButton.GetComponent<Outline>();

        if (selectedButtons.Contains(clickedButton))
        {
            // Unselect the button
            selectedButtons.Remove(clickedButton);
            outline.enabled = false;
        }
        else
        {
            // Ensure the selection count doesn't exceed the maximum allowed
            if (selectedButtons.Count >= maxSelections)
            {
                Debug.Log($"You can only select up to {maxSelections} options.");
                return;
            }

            // Select the button
            selectedButtons.Add(clickedButton);
            outline.enabled = true;
        }

        // Update the data for the level
        UpdateSelectedStrings(selectedButtons, selectedData);
    }

    // Overload for single selection (third level)
    private void OnButtonClick(Button clickedButton, List<Button> selectedButtons, out string selectedData, int maxSelections)
    {
        selectedData = null;
        var outline = clickedButton.GetComponent<Outline>();

        // Unselect if it's already selected
        if (selectedButtons.Contains(clickedButton))
        {
            selectedButtons.Remove(clickedButton);
            outline.enabled = false;
        }
        else
        {
            // Deselect previously selected button if maxSelections is 1
            if (selectedButtons.Count == maxSelections)
            {
                selectedButtons[0].GetComponent<Outline>().enabled = false;
                selectedButtons.Clear();
            }

            // Select the new button
            selectedButtons.Add(clickedButton);
            outline.enabled = true;
        }

        // Update the selected data (e.g., name or ID of the star)
        if (selectedButtons.Count > 0)
        {
            selectedData = selectedButtons[0].name; // Or another identifier
        }
    }

    // Updates selected colors for levels that use List<Color>
    private void UpdateSelectedColors(List<Button> selectedButtons, List<Color> selectedData)
    {
        selectedData.Clear();
        foreach (var button in selectedButtons)
        {
            var buttonImage = button.GetComponent<RawImage>();
            selectedData.Add(buttonImage.color);
        }
    }

    // Updates selected strings for levels that use List<string>
    private void UpdateSelectedStrings(List<Button> selectedButtons, List<string> selectedData)
    {
        selectedData.Clear();
        foreach (var button in selectedButtons)
        {
            selectedData.Add(button.name); // Or another identifier
        }
    }

    // Submit function to finalize all selections
    public void SubmitSelection()
    {
        lists.SaveList(selectedColors, "treeColors");
        lists.SaveList(selectedDecors, "treeDecors");

        PlayerPrefs.SetString("treeStar", selectedStar);

        Debug.Log($"Selections finalized: Ornaments ({string.Join(", ", selectedColors)}), Decor ({string.Join(", ", selectedDecors)}), Star ({selectedStar}).");

        LoadTree();
        
        // Proceed with tree decoration logic
        GM.CloseTreeDecorMenu();
    }

    public void LoadTree()
    {
        // Step 1: Set Ornament Colors
        if (selectedColors.Count == 2)
        {
            // Set first color ornaments
            foreach (SpriteRenderer ornament in ornaments_color1)
            {
                ornament.gameObject.SetActive(true);
                
                switch(selectedColors[0])
                {
                    case "red":
                        ornament.sprite = ornamentSprites[0];
                        break;
                    case "yellow":
                        ornament.sprite = ornamentSprites[1];
                    break;
                    case "pink":
                        ornament.sprite = ornamentSprites[2];
                    break;
                    case "orange":
                        ornament.sprite = ornamentSprites[3];
                    break;
                    case "blue":
                        ornament.sprite = ornamentSprites[4];
                    break;
                    case "purple":
                        ornament.sprite = ornamentSprites[5];
                    break;
                }

            }

            // Set second color ornaments
            foreach (SpriteRenderer ornament in ornaments_color2)
            {
                ornament.gameObject.SetActive(true);
                
                switch(selectedColors[1])
                {
                    case "red":
                        ornament.sprite = ornamentSprites[0];
                        break;
                    case "yellow":
                        ornament.sprite = ornamentSprites[1];
                    break;
                    case "pink":
                        ornament.sprite = ornamentSprites[2];
                    break;
                    case "orange":
                        ornament.sprite = ornamentSprites[3];
                    break;
                    case "blue":
                        ornament.sprite = ornamentSprites[4];
                    break;
                    case "purple":
                        ornament.sprite = ornamentSprites[5];
                    break;
                }

            }
        }

        // Step 2: Set Decor Ornaments
        if (selectedDecors.Count == 2)
        {
            decorMainGO.SetActive(true);

            // Set first color ornaments
            foreach (SpriteRenderer ornament in decorOrnaments_1)
            {
                ornament.gameObject.SetActive(true);
                
                switch(selectedDecors[0])
                {
                    case "candycane":
                        ornament.sprite = decorSprites[0];
                        break;
                    case "snowflake":
                        ornament.sprite = decorSprites[1];
                    break;
                    case "bell":
                        ornament.sprite = decorSprites[2];
                    break;
                    case "mistletoe":
                        ornament.sprite = decorSprites[3];
                    break;
                    case "pinecone":
                        ornament.sprite = decorSprites[4];
                    break;
                }

            }

            // Set second color ornaments
            foreach (SpriteRenderer ornament in decorOrnaments_2)
            {
                ornament.gameObject.SetActive(true);
                
                switch(selectedDecors[1])
                {
                    case "candycane":
                        ornament.sprite = decorSprites[0];
                        break;
                    case "snowflake":
                        ornament.sprite = decorSprites[1];
                    break;
                    case "bell":
                        ornament.sprite = decorSprites[2];
                    break;
                    case "mistletoe":
                        ornament.sprite = decorSprites[3];
                    break;
                    case "pinecone":
                        ornament.sprite = decorSprites[4];
                    break;
                }

            }
        }

        // Step 3: Select a star
        if (selectedStar == "small")
        {
            star.gameObject.SetActive(true);
            star.localScale = smallStarScale;

            GM.EndOfGameAdjustments();
        }

        if (selectedStar == "medium")
        {
            star.gameObject.SetActive(true);
            star.localScale = mediumStarScale;

            GM.EndOfGameAdjustments();
        }

        if (selectedStar == "large")
        {
            star.gameObject.SetActive(true);
            star.localScale = largeStarScale;

            // also move header up, change header text, enable daytoggle
            GM.EndOfGameAdjustments();
        }
    }
}
