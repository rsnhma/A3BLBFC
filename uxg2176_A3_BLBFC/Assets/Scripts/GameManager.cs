using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // If you're using TextMeshPro. If not, use UnityEngine.UI and Text.

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Simple singleton

    [Header("Progress / Interactions")]
    [Tooltip("How many interactions required before exit appears")]
    public int totalInteractionsRequired = 3;

    private int interactionsRemaining;

    [Header("HUD")]
    public TextMeshProUGUI interactionsText;   // Assign in Inspector

    [Header("Exit / Level Complete")]
    [Tooltip("Exit object that should appear when interactions are done")]
    public DoorController exitDoor;             // Door, portal, etc.
    public GameObject levelCompletePanel;     // UI panel for level complete

    [Header("Game Over")]
    public GameObject gameOverPanel;          // UI panel for game over

    [Header("Scenes")]
    [Tooltip("Name of your Start Menu scene")]
    public string startMenuSceneName = "MainMenu";

    private bool isGameOver = false;

    void Awake()
    {
        // Simple singleton pattern so other scripts can call GameManager.Instance
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Uncomment if GameManager persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        interactionsRemaining = totalInteractionsRequired;

        

        UpdateHUD();

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // Call this when the player completes an interaction
    public void RegisterInteraction()
    {
        if (isGameOver) return;

        interactionsRemaining = Mathf.Max(0, interactionsRemaining - 1);
        UpdateHUD();

        if (interactionsRemaining <= 0)
        {
            OnAllInteractionsComplete();
        }
    }

    void UpdateHUD()
    {
        if (interactionsText != null)
        {
            interactionsText.text = "Interactions Remaining: " + interactionsRemaining;
        }
    }

    void OnAllInteractionsComplete()
    {
        // Unlock the door
        if (exitDoor != null)
        {
            exitDoor.UnlockDoor();
            Debug.Log("Door unlocked via assigned reference!");
        }
        else
        {
            Debug.LogError("Exit door not assigned in GameManager!");
        }
    }

    // Call this when player reaches exit / wins
    public void LevelComplete()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f; // Freeze game

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);
    }

    // Call this when player dies / fails
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // UI BUTTON FUNCTIONS  ===================

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void ReturnToStartMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(startMenuSceneName);
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }

    // Add this method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
