// DifficultyMenu.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DifficultyMenu : MonoBehaviour
{
    [Header("Assign these three buttons in the Inspector")]
    public Button easyButton;
    public Button regularButton;
    public Button hardButton;

    [Header("Scene to load when a difficulty is picked")]
    public string gameSceneName = "BlackjackTable";  // change to your play‑scene name

    void Awake()
    {
        // Wire listeners once, programmatically
        easyButton   .onClick.AddListener(() => StartGame(GameSettings.Difficulty.Easy));
        regularButton.onClick.AddListener(() => StartGame(GameSettings.Difficulty.Regular));
        hardButton   .onClick.AddListener(() => StartGame(GameSettings.Difficulty.Hard));
    }

    private void StartGame(GameSettings.Difficulty mode)
    {
    GameSettings.DifficultyChosen = mode;

    // **reset** card‑counting mode back to NoHelp
    GameSettings.CountingChosen  = GameSettings.CountingMode.NoHelp;
    GameSettings.IsCountingSession  = false;

    SceneManager.LoadScene(gameSceneName);
    }
}
