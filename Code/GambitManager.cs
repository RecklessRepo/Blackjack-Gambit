using UnityEngine;

public static class GambitManager
{
    public static void TriggerRandomGambit()
    {
        int roll = Random.Range(0, 2);   // sample pool
        switch (roll)
        {
            case 0: DoublePot(); break;
            case 1: FreeHit();   break;
        }
    }

    /* ---- Sample Gambit effects ----------------------------------- */

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




}
