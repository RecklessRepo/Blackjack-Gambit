using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // --- This script is for BOTH player and dealer

    // Get other scripts
    public CardScript cardScript;
    public DeckScript deckScript;

    // Total value of player/dealer's hand
    public int handValue = 0;

    // Betting money
    private int money = 1000;

    // Array of card objects on table
    public GameObject[] hand;

    // Index of next card to be turned over
    public int cardIndex = 0;

    List<CardScript> aceList = new List<CardScript>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartHand()
    {
        GetCard();
        GetCard();
    }

   // Add a hand to the player/dealer's hand
    public int GetCard()
    {
        // Get a card, use DealCard to assign sprite and value to card on table
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<CardScript>());

        // Show card on game screen
        hand[cardIndex].GetComponent<Renderer>().enabled = true;

        // Add only normal card values to running total of the hand
        if (cardValue > 0) handValue += cardValue;

        // If value is 1, it is an ace
        if (cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
        }

        // Check if we should use an 11 instead of a 1

        cardIndex++;
        return handValue;
    }

    public void AceCheck()
    {
        // for each ace in the lsit check
        foreach (CardScript ace in aceList)
        {
            if(handValue + 10 < 22 && ace.GetValueOfCard() == 1)
            {
                // if converting, adjust card object value and hand
                ace.SetValue(11);
                handValue += 10;
            } else if (handValue > 21 && ace.GetValueOfCard() == 11)
            {
                // if converting, adjust gameobject value and hand value
                ace.SetValue(1);
                handValue -= 10;
            }
        }
    }

    public void AdjustMoney(int amount)
    {
        money += amount;
    }

    // Output players current money amount
    public int GetMoney()
    {
        return money;
    }

    // Hides all cards, resets the needed variables
    public void ResetHand()
    {
        for(int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<CardScript>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }
        cardIndex = 0;
        handValue = 0;
        aceList = new List<CardScript>();
    }

    public void RecalculateHandValue()
    {
        handValue = 0;
        aceList.Clear();

        for (int i = 0; i < cardIndex; i++)
        {
            int v = hand[i].GetComponent<CardScript>().GetValueOfCard();

            if (v == 1) aceList.Add(hand[i].GetComponent<CardScript>());
            if (v > 0)  handValue += v;          // ignore –99 Gambit sentinels
        }

        AceCheck();                              // handle soft / hard aces
    }
}
