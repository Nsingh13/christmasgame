using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource musicAudio;

    public Image audioIcon;
    public Sprite muteSprite;
    public Sprite volumeSprite;

    void Start()
    {
        musicAudio = this.gameObject.GetComponent<AudioSource>();
        // Check if there is already an instance of this GameObject
        if (instance == null)
        {
            // Set this as the instance and make it persist
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy duplicate GameObject
            Destroy(gameObject);
        }
    }

    public void MuteToggle()
    {
        musicAudio.mute = !(musicAudio.mute);

        audioIcon = GameObject.Find("AudioIcon").GetComponent<Image>();

        audioIcon.sprite = musicAudio.mute ? muteSprite : volumeSprite;
    }
}
