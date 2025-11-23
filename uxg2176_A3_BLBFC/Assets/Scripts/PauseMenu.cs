using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;  // Assign in Inspector
    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Don't allow pause if game is over
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver())
        {
            return; // Exit early - no pausing during game over
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freeze time
        if (pausePanel != null)
            pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume time
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToStartMenu()
    {
        // Use GameManager's function so logic is consistent
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReturnToStartMenu();
        }
    }

    // NEW: Quit game function for pause menu button
    public void QuitGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.QuitGame();
        }
    }
}