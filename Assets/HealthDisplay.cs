using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    // Reference to the Text or TextMeshProUGUI component
    public TextMeshProUGUI healthText;

    private void Start()
    {
        var lerpedColor = Color.Lerp(Color.red, Color.green, 0.1f);
        healthText.color = lerpedColor;
        healthText.text = GameObject.FindGameObjectWithTag("Player").GetComponent<GameData>().playerHealth + " HP";
    }

    public void UpdateHealth(int currentHealth)
    {
        var healthPercent = (float)currentHealth / 100f;
        var lerpedColor = Color.Lerp(Color.red, Color.green, healthPercent);
        healthText.color = lerpedColor;
        healthText.text = currentHealth + " HP";
    }
    
}