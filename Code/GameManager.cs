using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Buttons
    public Button DealB;
    public Button HitB;
    public Button StandB;
    public Button BetB;

    public TMP_Text scoreText;
    public TMP_Text dealerText;
    public TMP_Text betText;
    public TMP_Text cashText;
    public TMP_Text standBText;
    public TMP_Text mainText;

    public GameObject hideCard;
    public int pot=0;

    private int playerBet = 0;
    private int standClicks = 0;

    public PlayerScript playerScript;
    public PlayerScript dealerScript;
    void Start()
    {
        // On click listeners
        DealB.onClick.AddListener(() => DealClicked());
        HitB.onClick.AddListener(() => HitClicked());
        StandB.onClick.AddListener(() => StandClicked());
        BetB.onClick.AddListener(() => BetClicked());
    }

    private void DealClicked()
    {

        if (pot == 0)
        {
            ShowMessage("Place a bet first!");
            return;
        }

        playerScript.ResetHand();
        dealerScript.ResetHand();

        dealerText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        // Update the scores displayed
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerText.text = "Hand: " + dealerScript.handValue.ToString();
        // Place card back on dealer card, hide card
        hideCard.GetComponent<Renderer>().enabled = true;
        // Adjust buttons visibility
        DealB.gameObject.SetActive(false);
        HitB.gameObject.SetActive(true);
        StandB.gameObject.SetActive(true);
        BetB.gameObject.SetActive(false);
        standBText.text = "Stand";
        
        
        cashText.text = "$" + playerScript.GetMoney();
    }
    private void HitClicked()
    {
        hideCard.GetComponent<Renderer>().enabled = true;
         // Check that there is still room on the table
        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text = "Hand: " + playerScript.handValue.ToString();
            if (playerScript.handValue > 20) RoundOver();
        }
    }
    private void StandClicked()
    {
        HitB.gameObject.SetActive(false);
        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        standBText.text = "Call";
    }


    private void HitDealer()
    {

        while (dealerScript.handValue < 17 && dealerScript.cardIndex < 10)
        {
            hideCard.GetComponent<Renderer>().enabled = true;
            dealerScript.GetCard();
            dealerText.text = "Hand: " + dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20) RoundOver();
        }
    
    }

    void RoundOver()
    {
        // Booleans (true/false) for bust and blackjack/21
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;
        // If stand has been clicked less than twice, no 21s or busts, quit function
        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        // All bust, bets returned
        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        // if player busts, dealer didnt, or if dealer has more points, dealer wins
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer wins!";
        }
        // if dealer busts, player didnt, or player has more points, player wins
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
            playerScript.AdjustMoney(pot);
        }
        //Check for tie, return bets
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Push: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }
        // Set ui up for next move / hand / turn
        if (roundOver)
        {
            hideCard.GetComponent<Renderer>().enabled = false;
            HitB.gameObject.SetActive(false);
            StandB.gameObject.SetActive(false);
            DealB.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            dealerText.gameObject.SetActive(true);
            cashText.text = "$" + playerScript.GetMoney().ToString();
            standClicks = 0;

            pot = 0;
            playerBet = 0;
            betText.text = "Bets: $0";
            BetB.interactable = true;
            BetB.gameObject.SetActive(true);
        }
        return;
    }

    // Add money to pot if bet clicked
    private void BetClicked()
    {
        TMP_Text newBet = BetB.GetComponentInChildren<TMP_Text>();
        int intBet = int.Parse(newBet.text);   // “25” from the button text

        // take the chips from the player
        playerScript.AdjustMoney(-intBet);
        cashText.text = "$" + playerScript.GetMoney();

        // grow the pot each time the button is pressed
        pot += intBet * 2;                     // add both player + dealer share
        betText.text = "Pot: $" + pot;        // refresh label

        hideCard.GetComponent<Renderer>().enabled = true;

    }

    public void DoublePot()
    {
        pot *= 2;
        betText.text = "Bets: $" + pot;
    }

    public void UpdateScoreTexts()
    {
        scoreText.text  = "Hand: " + playerScript.handValue;
        dealerText.text = "Hand: " + dealerScript.handValue;
    }

    public void ShowMessage(string msg)
    {
        mainText.text = msg;
        mainText.gameObject.SetActive(true);
    }
}
