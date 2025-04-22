using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountingMenu : MonoBehaviour
{
    public Button helpButton;
    public Button noHelpButton;
    public string blackjackScene = "BlackjackTable";

    void Awake()
    {
        helpButton   .onClick.AddListener(() => StartGame(GameSettings.CountingMode.Help));
        noHelpButton .onClick.AddListener(() => StartGame(GameSettings.CountingMode.NoHelp));
    }
    private void StartGame(GameSettings.CountingMode mode)
    {
        GameSettings.CountingChosen = mode;
        GameSettings.IsCountingSession  = true;
        SceneManager.LoadScene(blackjackScene);
    }
}
