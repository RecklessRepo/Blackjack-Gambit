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
        /*
        cardValues.Clear();

        for (int i = 1; i < cardSprites.Count; i++)
        {
            int val = (i - 1) % 13 + 1;             
            val = (val > 10) ? 10 : val;            
            cardValues.Add(val);
        }

        if (GameSettings.UseGambits && gambitSprites.Count > 0)
        {
            foreach (Sprite g in gambitSprites)
            {
                cardSprites.Add(g);
                cardValues.Add(-99);                
            }
        }

        Shuffle();
        */
        cardValues.Clear();
        var baseSprites   = new List<Sprite>(cardSprites);   // copy original 52
        var baseValues    = new List<int>(cardValues);      // if you prefill values, else rebuild values each deck

        cardSprites.Clear();
        cardValues .Clear();

        for (int deck = 0; deck < GameSettings.NumberOfDecks; deck++)
        {
            // add one fresh deck’s worth of sprites+values
            for (int i = 1; i < baseSprites.Count; i++)
            {
                // sprite
                cardSprites.Add(baseSprites[i]);
                // value logic (1–10, face cards → 10)
                int val = (i - 1) % 13 + 1;
                val = (val > 10) ? 10 : val;
                cardValues.Add(val);
            }
            // if using gambits, append those sentinel cards per deck too
            if (GameSettings.UseGambits)
                foreach (var g in gambitSprites)
                {
                    cardSprites.Add(g);
                    cardValues.Add(-99);
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
        CheckShuffle();

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
    
    private void CheckShuffle()
    {
        int deckSize    = cardValues.Count;             // total cards in deck
        int cardsDealt  = currentIndex - 1;             // how many have been dealt
        float fracUsed  = (float)cardsDealt / deckSize; // fraction used

        // 50% for normal, 75% for hard
        float threshold = GameSettings.UseGambits ? 0.75f : 0.50f;
        Debug.Log($"[DeckScript] CheckShuffle(): dealt {cardsDealt}/{deckSize} "
              + $"({fracUsed:P0}) vs threshold {threshold:P0}");

        if (fracUsed >= threshold)
        {
            Debug.Log($"[DeckScript] {cardsDealt}/{deckSize} cards dealt ({fracUsed:P0}), reshuffling.");
            Shuffle();
        }
    }

}
