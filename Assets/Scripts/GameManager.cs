using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform grid;
    public GameObject cardPrefab;
    public Sprite[] cardFaces;
    public Text scoreText;
    public Text Turns;
    public Text Matches;
    public GameObject gamePanel;
    public GameObject wellPlayedPanel; // Assign in Inspector
    public string mainMenuSceneName = "MainMenu"; // Replace with your actual menu scene name

    public AudioSource[] Sounds;

    private List<Card> cards = new List<Card>();
    private Card firstCard, secondCard;
    private int score = 0;

    void Awake() => Instance = this;

    void Start()
    {
        LoadGame();
        CreateBoard(2, 2); // Example: 2x2
    }

    public void CreateBoard(int rows, int cols)
    {
        LoadGame();
        scoreText.text = $"Score: {score}";
        Matches.text = "0";
        Turns.text = "0";
        int totalCards = rows * cols;

        if (totalCards % 2 != 0)
        {
            Debug.LogError("Total number of cards must be even.");
            return;
        }

        foreach (Transform child in grid)
            Destroy(child.gameObject);
        cards.Clear();

        GridLayoutGroup layout = grid.GetComponent<GridLayoutGroup>();
        RectTransform rt = grid.GetComponent<RectTransform>();
        layout.cellSize = new Vector2(rt.rect.width / cols, rt.rect.height / rows);

        List<int> ids = new List<int>();
        for (int i = 0; i < totalCards / 2; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }
        Shuffle(ids);

        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, grid);
            Card card = cardObj.GetComponent<Card>();
            card.Init(cardFaces[ids[i]], ids[i]);
            cards.Add(card);
        }

        StartCoroutine(RevealCardsTemporarily());
    }

    IEnumerator RevealCardsTemporarily()
    {
        // Show all cards
        foreach (var card in cards)
            card.FlipInstant(); // You should add this method to flip without delay

        yield return new WaitForSeconds(2.5f);

        // Hide all cards
        foreach (var card in cards)
            card.FlipBack();
    }

    public void OnCardClicked(Card clicked)
    {
        if (firstCard != null && secondCard != null || clicked.isMatched || clicked.IsFlipped) return;

        Sounds[0].Play();
        clicked.Flip();

        if (firstCard == null)
        {
            firstCard = clicked;
        }
        else if (secondCard == null && clicked != firstCard)
        {
            secondCard = clicked;
            Turns.text = (int.Parse(Turns.text)+1).ToString();
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
            Matches.text = (int.Parse(Matches.text) + 1).ToString();
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

        // Check win condition
        if (AllCardsMatched())
            StartCoroutine(HandleGameComplete());
    }

    bool AllCardsMatched()
    {
        foreach (var card in cards)
        {
            if (!card.isMatched)
                return false;
        }
        return true;
    }

    IEnumerator HandleGameComplete()
    {
        yield return new WaitForSeconds(1f);
        wellPlayedPanel.SetActive(true);
        gamePanel.SetActive(false);
        
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
