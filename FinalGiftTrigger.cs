using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinalGiftTrigger : MonoBehaviour
{
    public OpenGiftUI giftUI;

    // Start is called before the first frame update
    void Start()
    {
        giftUI = GameObject.Find("Canvas_Main").transform.GetChild(3).GetComponent<OpenGiftUI>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Gift.GetStackCount() >=5)
        {
            RejectGift();
            return;
        }
        
        // Check if Santa interacted with the gift
        if (collision.CompareTag("Player") && collision.gameObject.activeInHierarchy)
        {
            collision.gameObject.SetActive(false);
            giftUI.ShowGiftPanel();
        }
    }


    void RejectGift()
    {
        // Scale down slightly
        transform.DOScale(transform.localScale * 0.8f, 0.15f).SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                // Shake the rotation gently
                transform.DORotate(new Vector3(0, 0, 8f), 0.15f).SetLoops(3, LoopType.Yoyo).SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        // Scale back to the original size
                        transform.DOScale(transform.localScale / 0.8f, 0.15f).SetEase(Ease.OutExpo);
                        transform.rotation = Quaternion.identity; // Reset rotation to default
                    });
            });
    }
}
