using UnityEngine;
using TMPro;

public class FullscreenDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    private void Start()
    {
        // Initialize dropdown value based on current screen mode
        dropdown.value = Screen.fullScreen ? 0 : 1; // Assuming 0 is fullscreen and 1 is windowed
    }

    // Method to handle changing fullscreen/windowed mode based on dropdown selection
    public void SetFullscreenMode(int index)
    {
        // Check if the index is valid (ensure that the dropdown options are configured properly)
        if (index == 0) // Assuming index 0 is for fullscreen
        {
            Screen.fullScreen = true;
        }
        else if (index == 1) // Assuming index 1 is for windowed mode
        {
            Screen.fullScreen = false;
        }
    }
}
