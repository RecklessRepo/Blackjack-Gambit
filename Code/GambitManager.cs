using UnityEngine;

public static class GambitManager
{

    static GameManager GM() => Object.FindObjectOfType<GameManager>();

    public static void TriggerRandomGambit()
    {
        int roll = Random.Range(0, 4);   // 0‑4  (five Gambits total)
        switch (roll)
        {
            case 0: DoublePot();       break;
            case 1: FreeHit();         break;
            case 4: InsuranceFraud();  break;
        }
    }

    /* Gambit effects */

    static void DoublePot()
    {
        GameManager gm = Object.FindObjectOfType<GameManager>();
        gm.pot *= 2;
        gm.betText.text = "Bets: $" + gm.pot;
        gm.ShowMessage("GAMBIT! Pot Doubled!");
    }

    static void FreeHit()
    {
        GameManager gm = Object.FindObjectOfType<GameManager>();
        gm.playerScript.GetCard();
        gm.UpdateScoreTexts();
        gm.ShowMessage("GAMBIT! Free Card!");
    }


    static void InsuranceFraud()
    {
        var gm = GM();
        int refund = gm.pot / 2;
        gm.playerScript.AdjustMoney(refund);
        gm.pot -= refund;
        gm.cashText.text = "$" + gm.playerScript.GetMoney();
        gm.betText.text = "Bets: $" + gm.pot;
        gm.ShowMessage($"GAMBIT! Insurance Fraud ‑ you stole ${refund} back!");
    }
    
}
