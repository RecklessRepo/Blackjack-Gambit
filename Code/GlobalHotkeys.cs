using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalHotkeys : MonoBehaviour
{
    [Tooltip("Scene name or build‑index of the Main Menu")]
    public string mainMenuScene = "Homepage";   // set this in the Inspector

    void Awake()
    {
        DontDestroyOnLoad(gameObject);          // keep this object alive
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
