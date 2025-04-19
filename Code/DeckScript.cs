using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    [Header("52 standard faces (index 0 is card back)")]
    public List<Sprite> cardSprites = new();
    [Header("Optional Gambit faces (only used in Hard mode)")]
    public List<Sprite> gambitSprites = new();

    private List<int> cardValues = new();
    private int currentIndex = 0;

    void Start() => BuildBaseDeck();

    /* ---------------------------------------------------------------- */

    void BuildBaseDeck()
    {
        cardValues.Clear();

        // skip index 0 (card back)
        for (int i = 1; i < cardSprites.Count; i++)
        {
            int val = (i - 1) % 13 + 1;             // 1‑13
            val = (val > 10) ? 10 : val;            // face = 10
            cardValues.Add(val);
        }

        // Optional Gambits
        if (GameSettings.UseGambits && gambitSprites.Count > 0)
        {
            foreach (Sprite g in gambitSprites)
            {
                cardSprites.Add(g);
                cardValues.Add(-99);                // sentinel = Gambit
            }
        }

        Shuffle();
    }

    public void Shuffle()
    {
        for (int i = cardSprites.Count - 1; i > 1; i--)
        {
            int j = Random.Range(1, i + 1);
            (cardSprites[i], cardSprites[j]) = (cardSprites[j], cardSprites[i]);
            (cardValues[i - 1], cardValues[j - 1]) = (cardValues[j - 1], cardValues[i - 1]);
        }
        currentIndex = 1;
    }

    public int DealCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[currentIndex]);
        int val = cardValues[currentIndex - 1];
        cardScript.SetValue(val);
        currentIndex++;

        if (val == -99)
        {
            Debug.Log("Gambit card dealt!");
            GambitManager.TriggerRandomGambit();
        }

        return val;
    }

    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }

}
