using UnityEngine;
using TMPro;

public class UISlimeStats : MonoBehaviour
{
    public SlimeAI slime; // Reference to the slime object
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI attackText; // Text for displaying attack damage
    public TextMeshProUGUI attackIntervalText; // Text for displaying attack interval
    public TextMeshProUGUI nameText; // Text for displaying slime's name
    public TextMeshProUGUI rebirthCountText; // Text for displaying rebirth count

    private void Update()
    {
        // Update UI with slime stats
        if (slime != null)
        {
            hpText.text = "HP: " + slime.currentHP.ToString() + " / " + slime.maxHP.ToString();
            levelText.text = "Lvl: " + slime.level.ToString();
            expText.text = "Exp: " + slime.experience.ToString() + "/" + slime.experienceToNextLevel.ToString();
            attackText.text = "Atk: " + slime.attackDamage.ToString(); // Display attack damage
            attackIntervalText.text = "Atkspd: " + slime.attackInterval.ToString("F2") + "/s"; // Display attack interval with 2 decimal places
            nameText.text = slime.slimeName; // Display slime's name

            // Display rebirth count
            rebirthCountText.text = "Rebirths: " + slime.rebirthCount.ToString();
        }
    }
}
