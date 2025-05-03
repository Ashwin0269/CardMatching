using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform grid;
    public GameObject cardPrefab;
    public Sprite[] cardFaces;
    public Text scoreText;

    private List<Card> cards = new List<Card>();
    private Card firstCard, secondCard;
    private int score = 0;

    void Awake() => Instance = this;

    void Start()
    {
        LoadGame();
        CreateBoard(4); // for example: 2x2
    }

    public void CreateBoard(int pairCount)
    {
        List<int> ids = new List<int>();
        for (int i = 0; i < pairCount; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        Shuffle(ids);

        foreach (int id in ids)
        {
            GameObject cardObj = Instantiate(cardPrefab, grid);
            Card card = cardObj.GetComponent<Card>();
            card.Init(cardFaces[id], id);
            cards.Add(card);
        }
    }

    public void OnCardClicked(Card clicked)
    {
        if (firstCard != null && secondCard != null) return;

        clicked.Flip();

        if (firstCard == null)
        {
            firstCard = clicked;
        }
        else if (secondCard == null && clicked != firstCard)
        {
            secondCard = clicked;
            StartCoroutine(CompareCards());
        }
    }

    IEnumerator CompareCards()
    {
        yield return new WaitForSeconds(0.5f);

        if (firstCard.id == secondCard.id)
        {
            firstCard.isMatched = true;
            secondCard.isMatched = true;
            score += 100;
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();
            score -= 20;
        }

        firstCard = secondCard = null;
        scoreText.text = $"Score: {score}";
        SaveGame();
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            int tmp = list[i];
            list[i] = list[rand];
            list[rand] = tmp;
        }
    }

    void SaveGame() => SaveManager.Save(score);
    void LoadGame() => score = SaveManager.Load();
}
