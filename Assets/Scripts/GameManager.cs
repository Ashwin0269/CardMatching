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

    public AudioSource[] Sounds;

    private List<Card> cards = new List<Card>();
    private Card firstCard, secondCard;
    private int score = 0;

    void Awake() => Instance = this;

    void Start()
    {
        LoadGame();
        CreateBoard(2,2); // for example: 2x2
    }

    public void CreateBoard(int rows, int cols)
    {
        int totalCards = rows * cols;

        if (totalCards % 2 != 0)
        {
            Debug.LogError("Total number of cards must be even.");
            return;
        }

        // Clear previous cards
        foreach (Transform child in grid)
            Destroy(child.gameObject);
        cards.Clear();

        // Setup GridLayoutGroup cell size dynamically
        GridLayoutGroup layout = grid.GetComponent<GridLayoutGroup>();
        RectTransform rt = grid.GetComponent<RectTransform>();

        float cellWidth = rt.rect.width / cols;
        float cellHeight = rt.rect.height / rows;
        layout.cellSize = new Vector2(cellWidth, cellHeight);

        // Create card ID pairs
        List<int> ids = new List<int>();
        for (int i = 0; i < totalCards / 2; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        Shuffle(ids);

        // Instantiate cards with shuffled IDs
        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, grid);
            Card card = cardObj.GetComponent<Card>();
            card.Init(cardFaces[ids[i]], ids[i]);
            cards.Add(card);
        }
    }

    public void OnCardClicked(Card clicked)
    {
        Sounds[0].Play();

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
            Sounds[1].Play();
            score += 100;
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();
            Sounds[2].Play();
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
