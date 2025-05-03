using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int id;
    public Image front;
    public GameObject frontObj, backObj;

    public bool isMatched = false;

    public void Init(Sprite frontSprite, int cardId)
    {
        front.sprite = frontSprite;
        id = cardId;
    }

    public void Flip()
    {
        if (isMatched) return;
        frontObj.SetActive(true);
        backObj.SetActive(false);
    }

    public void FlipBack()
    {
        if (isMatched) return;
        frontObj.SetActive(false);
        backObj.SetActive(true);
    }

    public void OnClick()
    {
        GameManager.Instance.OnCardClicked(this);
    }
}
