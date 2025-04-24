// DifficultyMenu.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DifficultyMenu : MonoBehaviour
{
    [Header("Assign these three buttons in the Inspector")]
    public Button easyButton;
    public Button regularButton;
    public Button hardButton;

    [Header("Deck Selection Dropdown")]
    public TMP_Dropdown deckCountDropdown;

    [Header("Scene to load when a difficulty is picked")]
    public string gameSceneName = "BlackjackTable";  // change to your play‑scene name

    void Awake()
    {
        // ——— Wire up dropdown first ———
        // Initialize to current setting
        string current = GameSettings.NumberOfDecks.ToString();
        int idx = deckCountDropdown.options.FindIndex(o => o.text == current);
        deckCountDropdown.value = (idx >= 0) ? idx : 0;

        // Listen for changes
        deckCountDropdown.onValueChanged.AddListener(OnDeckCountChanged);

        // Wire listeners once, programmatically
        easyButton   .onClick.AddListener(() => StartGame(GameSettings.Difficulty.Easy));
        regularButton.onClick.AddListener(() => StartGame(GameSettings.Difficulty.Regular));
        hardButton   .onClick.AddListener(() => StartGame(GameSettings.Difficulty.Hard));
    }

    private void OnDeckCountChanged(int optionIndex)
    {
        if (int.TryParse(deckCountDropdown.options[optionIndex].text, out int chosen))
        {
            GameSettings.NumberOfDecks = chosen;
            Debug.Log($"[DifficultyMenu] NumberOfDecks set to {chosen}");
        }
        else
        {
            Debug.LogWarning($"[DifficultyMenu] Couldn't parse deck count '{deckCountDropdown.options[optionIndex].text}'");
        }
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
