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
    public bool IsFlipped { get; private set; } = false;

    public void Init(Sprite frontSprite, int cardId)
    {
        front.sprite = frontSprite;
        id = cardId;
        FlipBack(); // Ensure card starts face-down
    }

    public void Flip()
    {
        if (isMatched || IsFlipped) return;

        frontObj.SetActive(true);
        backObj.SetActive(false);
        IsFlipped = true;
    }

    public void FlipBack()
    {
        if (isMatched) return;

        frontObj.SetActive(false);
        backObj.SetActive(true);
        IsFlipped = false;
    }

    public void FlipInstant()
    {
        frontObj.SetActive(true);
        backObj.SetActive(false);
        IsFlipped = true;
    }

    public void OnClick()
    {
        if (!isMatched && !IsFlipped)
        {
            GameManager.Instance.OnCardClicked(this);
        }
    }
}
