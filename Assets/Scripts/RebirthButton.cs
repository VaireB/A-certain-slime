using UnityEngine;
using UnityEngine.UI;

public class RebirthButton : MonoBehaviour
{
    public Button rebirthButton;
    public SlimeAI slimeAI;

    private void Start()
    {
        rebirthButton.onClick.AddListener(Rebirth);
    }

    private void Rebirth()
    {
        if (slimeAI != null)
        {
            slimeAI.Rebirth();
        }
    }
}
