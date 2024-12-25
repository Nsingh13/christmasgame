using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour
{
    private float origY;
    private RectTransform buttonTransform;

    public AudioSource SFX_audiosource;
    public AudioClip buttonClickSound;

    // Start is called before the first frame update
    void Start()
    {
       buttonTransform = this.GetComponent<RectTransform>();
        origY = buttonTransform.anchoredPosition.y;
    }

    public void PushButtonDown()
    {
        SFX_audiosource.PlayOneShot(buttonClickSound);        
        buttonTransform.DOAnchorPos(new Vector2(buttonTransform.anchoredPosition.x, origY - 25f), 0.2f);
    }

    public void ReleaseButton()
    {
        buttonTransform.DOAnchorPos(new Vector2(buttonTransform.anchoredPosition.x, origY), 0.1f);
    }
}
