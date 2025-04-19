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

    private void StartGame(GameSettings.Difficulty diff)
    {
        GameSettings.DifficultyChosen = diff;      // store choice for the next scene
        SceneManager.LoadScene(gameSceneName);     // load play scene
    }
}
