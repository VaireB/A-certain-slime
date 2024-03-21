using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsCanvas; // Reference to the options menu canvas GameObject
    public GameObject mainMenuCanvas; // Reference to the main menu canvas GameObject

    void Start()
    {
        // Ensure the main menu canvas is turned on when the game starts
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Main menu canvas reference is missing.");
        }
    }

    public void StartGame()
    {
        // Load the game scene
        SceneManager.LoadScene("GameScene"); // Replace "GameScene" with the name of your game scene
    }

    public void BackButton()
    {
        // Toggle visibility of main menu and options canvas
        ToggleCanvasVisibility(mainMenuCanvas, true);
        ToggleCanvasVisibility(optionsCanvas, false);
    }

    public void OpenOptions()
    {
        // Deactivate any other canvas that might be active
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas.gameObject != optionsCanvas)
            {
                canvas.gameObject.SetActive(false);
            }
        }

        // Activate the options menu canvas
        if (optionsCanvas != null)
        {
            optionsCanvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Options canvas reference is missing.");
        }
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }

    private void ToggleCanvasVisibility(GameObject canvas, bool visible)
    {
        if (canvas != null)
        {
            canvas.SetActive(visible);
        }
        else
        {
            Debug.LogWarning("Canvas reference is missing.");
        }
    }
}
