using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeMenu : MonoBehaviour
{
    [Header("Assign buttons in the Inspector")]
    public Button blackjackButton;
    public Button cardCountingButton;

    [Header("Scene names to load")]
    public string regularMenuScene = "Regular Game Menu";
    public string cardCountingScene = "Card Counting Menu";

    void Awake()
    {
        blackjackButton.onClick.AddListener(() => SceneManager.LoadScene(regularMenuScene));
        cardCountingButton.onClick.AddListener(() => SceneManager.LoadScene(cardCountingScene));
    }
}
