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

    public TMP_Text runningCountText;
    private int runningCount = 0;

    private int playerBet = 0;
    private int standClicks = 0;

    private int handsPlayed = 0;

    // --- new prompt panel references (assign these in the Inspector!) ---
    [Header("No‑Help Count Prompt")]
    public GameObject     countPromptPanel; 
    public TMP_Text       promptInstructions; 
    public TMP_InputField promptInput;         
    public Button         promptCheckButton;   


    public PlayerScript playerScript;
    public PlayerScript dealerScript;
    void Start()
    {
        // On click listeners
        DealB.onClick.AddListener(() => DealClicked());
        HitB.onClick.AddListener(() => HitClicked());
        StandB.onClick.AddListener(() => StandClicked());
        BetB.onClick.AddListener(() => BetClicked());

        promptCheckButton.onClick.AddListener(OnCheckCount);
        
        runningCount = 0;
        bool show = GameSettings.ShowRunningCount;
        runningCountText.gameObject.SetActive(show);
        if (show)
        runningCountText.text = "Count: 0";

        countPromptPanel.SetActive(false);
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
        Debug.Log("I GOT TO THIS PART");
        // Adjust buttons visibility
        DealB.gameObject.SetActive(false);
        HitB.gameObject.SetActive(true);
        StandB.gameObject.SetActive(true);
        BetB.gameObject.SetActive(false);
        standBText.text = "Stand";

        // show or hide the counter depending on mode
        runningCountText.gameObject.SetActive(GameSettings.ShowRunningCount);
        if (GameSettings.ShowRunningCount) runningCountText.text = "Count: " + runningCount;

        //for no help mode
        if (!GameSettings.ShowRunningCount)
            handsPlayed++;
        
        cashText.text = "$" + playerScript.GetMoney();
    }

   private void HitClicked()
    {
        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text = "Hand: " + playerScript.handValue;

            if (playerScript.handValue > 20)
            {
                RoundOver();
            }
        }
    }
    private void StandClicked()
    {
    HitB.gameObject.SetActive(false);
    standClicks++;
    hideCard.GetComponent<Renderer>().enabled = false;
    HitDealer();   // let dealer play out their turn
    RoundOver();   // then settle the round
    standBText.text = "Call";
    }


    private void HitDealer()
{
    switch (GameSettings.DifficultyChosen)
    {
        /* ---------------- Easy ---------------- */
        case GameSettings.Difficulty.Easy:
            // Dealer stops at ANY 15 or more
            while (dealerScript.handValue <= 16 && dealerScript.cardIndex < 10)
                DealToDealer();
            break;

        /* ------------- Regular ---------------- */
        case GameSettings.Difficulty.Regular:
            // Dealer stops at hard/soft 17 or more
            while (dealerScript.handValue <= 17 && dealerScript.cardIndex < 10)
                DealToDealer();
            break;

        /* -------------- Hard ------------------ */
        case GameSettings.Difficulty.Hard:
            // Dealer must hit on 17 (we added earlier)
            while (dealerScript.handValue < 17 && dealerScript.cardIndex < 10)
                DealToDealer();
            break;
    }

    if (dealerScript.handValue > 21) RoundOver();   // auto‑settle on bust
    }

/* helper so we don’t duplicate two lines */
    private void DealToDealer()
    {
        dealerScript.GetCard();
        UpdateRunningCount(dealerScript.hand[dealerScript.cardIndex-1].GetComponent<CardScript>().GetValueOfCard());
        dealerText.text = "Hand: " + dealerScript.handValue;
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
            int payout = pot;
            // Easy: 2 × pot  |  Regular: 3/2 pot  |  Hard: normal
            if (GameSettings.DifficultyChosen == GameSettings.Difficulty.Easy)     payout = pot * 2;
            else if (GameSettings.DifficultyChosen == GameSettings.Difficulty.Regular) payout = (int)(pot * 1.5f);

            playerScript.AdjustMoney(payout);
            mainText.text = "You win!";
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
            betText.text = "Pot: $0";
            BetB.interactable = true;
            BetB.gameObject.SetActive(true);

             if (GameSettings.IsCountingSession && GameSettings.CountingChosen == GameSettings.CountingMode.NoHelp && handsPlayed >= 5)
            {
                ShowCountPrompt();
                return;
            }
        }
    }

    private void ShowCountPrompt()
    {
        // disable Deal so they can’t skip it
        DealB.interactable = false;

        // show & clear input
        countPromptPanel.SetActive(true);
        promptInput.text = "";
        promptInstructions.text = "Enter the running count:";
        mainText.gameObject.SetActive(false);
    }

    private void OnCheckCount()
    {
        // parse the player’s entry
        int answer = 0;
        int.TryParse(promptInput.text, out answer);

        // award / penalty
        if (answer == runningCount)
        {
            playerScript.AdjustMoney(100);
            ShowMessage("Correct! +$100");
        }
        else
        {
            playerScript.AdjustMoney(-100);
            ShowMessage($"Wrong (was {runningCount}). -$100");
        }

        cashText.text = "$" + playerScript.GetMoney();

        // hide prompt & re‑enable Deal
        countPromptPanel.SetActive(false);
        DealB.interactable = true;

        // reset the 5‑hand counter
        handsPlayed = 0;
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

    }

    public void DoublePot()
    {
        pot *= 2;
        betText.text = "Pot: $" + pot;
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

    private void ShowCoverCard()
    {
        var cs = hideCard.GetComponent<CardScript>();
        cs.ResetCard();  // force the back sprite, hopefully fix the bug
        hideCard.GetComponent<Renderer>().enabled = true;
    }

    private void HideCoverCard()
    {
        hideCard.GetComponent<Renderer>().enabled = false;
    }

    public void UpdateRunningCount(int cardValue)
    {
    // Hi‑Low: 2‑6 => +1, 7‑9 => 0, 10/A => –1
    if (cardValue >= 2 && cardValue <= 6)      runningCount++;
    else if (cardValue == 1 || cardValue == 10) runningCount--;

    if (GameSettings.ShowRunningCount)
        runningCountText.text = "Count: " + runningCount;
    }

    
}
