using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [Header("Elements")]
    public GameObject gift1;
    public GameObject gift2;
    public GameObject gift3;
    public GameObject tree;
    public Button backButton;
    public Button audioButton;
    public Button dayToggleButton;
    public TextMeshProUGUI header;

    [Header("Animation Parameters")]
    public Vector3 centerPosition = new Vector3(0, 0, 0); // Center position of the screen
    public Vector3 giftScale = new Vector3(3f, 3f, 1f); // Gift scale (3, 3)
    public Vector3 gift1OriginalPosition = new Vector3(-1f, -3.2f, 0f);
    public Vector3 gift1OriginalScale= new Vector3(0.5f, 0.5f, 0.5f);

    public float treeDownPositionY = -10f; // Final tree Y position
    public float treeInitialY = -2f; // Initial tree Y position
    public float animationDuration = 1f; // Duration for each animation
    public float giftMoveDuration = 1f; // Duration for moving the gift

    public PulsateEffect Selector;

    private int openedGifts;
    public GameObject gift1Open;
    public GameObject gift2Open;
    public GameObject gift3Open;

    public TreeDecorator treeDecorator;
    public ListSaver lists;
    public GameObject treeColorMenu;
    public GameObject treeDecorMenu;
    public GameObject treeStarMenu;
    public GameObject treeSubmitButton;

    private bool showingTreeMenu = false;

    private bool showingGift = false;

    public SceneTransition sceneTransitioner;

    public GameObject buyGameInfo;
    public Button buyGameButton;
    public Button restorePurchaseButton;

    private bool gameBought = false;

    public AudioSource SFX_audiosource;
    public AudioClip buttonClick;

    public Image audioIcon;
    public AudioSource musicAudio;
    public Sprite muteSprite;
    public Sprite volumeSprite;

    private void Awake()
    {
        // Loading saved data
        openedGifts = PlayerPrefs.GetInt("GiftsOpened", 0); // Default to 0 if no data is found
        treeDecorator.selectedColors = lists.LoadList("treeColors");
        treeDecorator.selectedDecors = lists.LoadList("treeDecors");
        treeDecorator.selectedStar = PlayerPrefs.GetString("treeStar");
        gameBought = bool.Parse(PlayerPrefs.GetString("boughtGame", "false"));

        musicAudio = GameObject.Find("MusicManager").GetComponent<AudioSource>();

        switch(openedGifts)
        {
            case 0: 
                Selector.targetObject = gift1;
                break;
            case 1:
                Selector.targetObject = gift2;
                if (treeDecorator.selectedColors == null || treeDecorator.selectedColors.Count == 0)
                {
                    showingTreeMenu = true;
                    ShowTreeDecorMenu(1);
                }

                break;
            case 2:
                Selector.targetObject = gift3;
                if (treeDecorator.selectedDecors == null || treeDecorator.selectedDecors.Count == 0)
                {
                    showingTreeMenu = true;
                    ShowTreeDecorMenu(2);
                }

                break;
            case 3:
                Selector.enabled = false;
                if (treeDecorator.selectedStar == "" || treeDecorator.selectedStar == null)
                {
                    showingTreeMenu = true;
                    ShowTreeDecorMenu(3);
                }
                break;
            default:
                Selector.enabled = false;
                break;
        }
    }
    private void Start()
    {
        audioIcon.sprite = musicAudio.mute ? muteSprite : volumeSprite;

        if (!showingTreeMenu)
        {
            treeDecorator.LoadTree();

            // show opened gifts
            for (int i = 0; i <= openedGifts; i++)
            {
                switch(i)
                {
                    case 1:
                        gift1.SetActive(false);
                        gift1Open.SetActive(true);
                        break;
                    case 2:
                        gift2.SetActive(false);
                        gift2Open.SetActive(true);
                        break;
                    case 3:
                        gift3.SetActive(false);
                        gift3Open.SetActive(true);
                        break;
                    default:
                        break;
                }
            }
        }

        // Add listener to the back button
        backButton.onClick.AddListener(ReverseSequence);


    }

    private void Update()
    {
        if (showingGift)
        {
            if (Input.GetMouseButtonDown(0) && Input.mousePosition.y <= Screen.height - 250) // Detect user taps
            {
                switch(header.text)
                {
                    case "Gift 1":
                        sceneTransitioner.nextSceneName = "Chapter1";
                        break;
                    case "Gift 2":
                        sceneTransitioner.nextSceneName = "Chapter2";
                        break;
                    case "Gift 3":
                        sceneTransitioner.nextSceneName = "Chapter3";
                        break;
                }

                sceneTransitioner.EndTransition();
            }
        }
    }

    public void NextSequence(string clickedItem)
    {
        SFX_audiosource.PlayOneShot(buttonClick);

        Debug.Log("Next sequence called for " + clickedItem);
        GameObject selectedGift = Selector.targetObject;
        gift1OriginalPosition = selectedGift.transform.position;
        gift1OriginalScale = selectedGift.transform.localScale;

        // Disable the other gifts
        if (Selector.targetObject == gift1)
        {
            gift2.SetActive(false);
            gift3.SetActive(false);

            header.text = "Gift 1";
        } else if (Selector.targetObject == gift2)
        {
            gift1Open.SetActive(false);
            gift3.SetActive(false);

            header.text = "Gift 2";

            if (!gameBought)
            {
                ShowBuyGameScreen();
                selectedGift = null;
            }
        } else if (Selector.targetObject == gift3)
        {
            gift1Open.SetActive(false);
            gift2Open.SetActive(false);

            // Update header text for the chapter
            header.text = "Gift 3";
        }

        // Animate tree down to make space
        tree.transform.DOMoveY(treeDownPositionY, animationDuration).SetEase(Ease.OutQuad);

        if (selectedGift != null)
        {
            // Animate Gift to center position and scale
            selectedGift.transform.DOMove(centerPosition, giftMoveDuration).SetEase(Ease.OutQuad)
            .OnComplete(() => 
            {
                showingGift = true;
            });
            
            selectedGift.transform.DOScale(giftScale, animationDuration).SetEase(Ease.OutBack);
        }

        audioButton.gameObject.SetActive(false);
        
        backButton.gameObject.SetActive(true);
    }

    private void ShowBuyGameScreen()
    {
        header.text = "Buy Game";
        buyGameInfo.SetActive(true);
        buyGameButton.gameObject.SetActive(true);
        restorePurchaseButton.gameObject.SetActive(true);
    }

    private void CloseBuyGameScreen()
    {
        header.text = "The Official Christmas Game";
        buyGameInfo.SetActive(false);
        buyGameButton.gameObject.SetActive(false);
        restorePurchaseButton.gameObject.SetActive(false);
    }

    public void ReverseSequence()
    {
        GameObject selectedGift = Selector.targetObject;
        showingGift = false;

        // Revert tree position to initial Y
        tree.transform.DOMoveY(treeInitialY, animationDuration).SetEase(Ease.InQuad)
        .OnComplete(() => 
        {
            // Reactivate the other gifts
            if (Selector.targetObject == gift1)
            {
                gift2.SetActive(true);
                gift3.SetActive(true);
            } else if (Selector.targetObject == gift2)
            {
                gift1Open.SetActive(true);
                gift3.SetActive(true);
            } else if (Selector.targetObject == gift3)
            {
                gift1Open.SetActive(true);
                gift2Open.SetActive(true);
            }
        });

        CloseBuyGameScreen();

        // Revert Gift1 to its original position and scale
        selectedGift.transform.DOMove(gift1OriginalPosition, giftMoveDuration).SetEase(Ease.InQuad)
        .OnComplete(()=> 
        {
            Selector.SetOutline();
            Selector.isPulsating = true;
        });

        selectedGift.transform.DOScale(gift1OriginalScale, animationDuration).SetEase(Ease.InBack);


        // Reset the header text
        header.text = "The Official Christmas Game";

        backButton.gameObject.SetActive(false);

        audioButton.gameObject.SetActive(true);

    }

    public void ShowTreeDecorMenu(int stage)
    {
        gift1.SetActive(false);
        gift2.SetActive(false);
        gift3.SetActive(false);
        gift1Open.SetActive(false);
        gift2Open.SetActive(false);
        gift3Open.SetActive(false);

        // Animate tree down to make space
        tree.transform.DOMoveY(treeDownPositionY, animationDuration).SetEase(Ease.OutQuad);

        switch (stage)
        {
            case 1:
                header.text = "Choose 2";
                treeColorMenu.SetActive(true);
                break;
            case 2:
                header.text = "Choose 2";
                treeDecorMenu.SetActive(true);
                break;
            case 3:
                header.text = "Choose a star";
                treeStarMenu.SetActive(true);

                dayToggleButton.gameObject.GetComponent<DayNightToggle>().SwitchToNightImmediate();
                break;
        }

        treeSubmitButton.SetActive(true);
    }

    public void CloseTreeDecorMenu()
    {
        if (Selector.enabled)
        {
            // Reset the header text
            header.text = "The Official Christmas Game";
        }
        else
        {
            header.text = "Thanks for playing!";
            animationDuration = animationDuration + 1f;
        }

        SFX_audiosource.PlayOneShot(buttonClick);

        // Revert tree position to initial Y
        tree.transform.DOMoveY(treeInitialY, animationDuration).SetEase(Ease.InQuad)
        .OnComplete(() => 
        {
            if (Selector.enabled)
            {
                // Reactivate the other gifts
                if (Selector.targetObject == gift1)
                {
                    gift1.SetActive(true);
                    gift2.SetActive(true);
                    gift3.SetActive(true);
                } else if (Selector.targetObject == gift2)
                {
                    gift1Open.SetActive(true);
                    gift2.SetActive(true);
                    gift3.SetActive(true);
                } else if (Selector.targetObject == gift3)
                {
                    gift1Open.SetActive(true);
                    gift2Open.SetActive(true);
                    gift3.SetActive(true);
                }
            }
            else
            {
                gift1Open.SetActive(true);
                gift2Open.SetActive(true);
                gift3Open.SetActive(true);

                //EndOfGameAdjustments();
            }
        });


        treeSubmitButton.SetActive(false);
        treeColorMenu.SetActive(false);
        treeDecorMenu.SetActive(false);
        treeStarMenu.SetActive(false);

        treeDecorator.LoadTree();

    }

    public void EndOfGameAdjustments()
    {
        if (!IsOldDeviceResolution())
            header.rectTransform.DOAnchorPosY(700f, 0.5f);

        header.text = "Thanks for playing!";

        dayToggleButton.gameObject.SetActive(true);
        dayToggleButton.gameObject.GetComponent<DayNightToggle>().SwitchToNightImmediate();
    }

    bool IsOldDeviceResolution()
    {
        // Get the current screen resolution
        int width = Screen.currentResolution.width;
        int height = Screen.currentResolution.height;

        // Define resolutions for older devices
        var oldDeviceResolutions = new HashSet<(int, int)>
        {
            (750, 1334),  // iPhone SE (2020, 2022), iPhone 7, iPhone 8
            (2048, 2732), // iPad Pro 12.9
            (1668, 2388), // iPad Pro 11
            (1488, 2266),  // iPad Mini (6th gen)
            (1536, 2048), // iPad Mini 4
            (1668, 2224), // iPad Pro 10.15
            (1640, 2360), // iPad Air 5th gen
            (810, 1080), // iPad 10.2
            (2064, 2752), // More iPads..
            (1620, 2160),
            (744, 1133),
            (834, 1194),
            (1668, 2420)
        };

        // Check if current resolution matches any in the set
        return oldDeviceResolutions.Contains((width, height));
    }

    public void MuteToggle()
    {
        musicAudio.gameObject.GetComponent<MusicManager>().MuteToggle();
    }
}
