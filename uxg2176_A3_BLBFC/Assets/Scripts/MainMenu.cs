using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "Level1";

    // Called by the Play button
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Called by the Quit button
    public void QuitGame()
    {
        Debug.Log("Quit Game pressed");

        // This will quit the application in a built game
        Application.Quit();

    }
}
