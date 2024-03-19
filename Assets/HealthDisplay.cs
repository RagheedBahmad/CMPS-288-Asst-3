using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    private void Start() // initializing health display
    {
        // gets player's health to update
        var currentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<GameData>().playerHealth; 
        UpdateHealth(currentHealth);
    }

    public void UpdateHealth(int currentHealth)
    {
        var healthPercent = (float)currentHealth / 100f; // gets health percentage
        var lerpedColor = Color.Lerp(Color.red, Color.green, healthPercent); // shifts hue based on percentage
        healthText.color = lerpedColor; // assigns new color
        healthText.text = currentHealth + " HP"; // updates text
    }
}