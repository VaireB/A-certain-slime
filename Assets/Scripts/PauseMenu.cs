using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    void Start()
    {
        // Initially, hide the pause menu UI
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        // Toggle the visibility of the pause menu UI
        bool isPaused = !pauseMenuUI.activeSelf;
        pauseMenuUI.SetActive(isPaused);

        // Pause or resume the game based on the pause menu's visibility
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Quit()
    {
        // Quit the application
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}
