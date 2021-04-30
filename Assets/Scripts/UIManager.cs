using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateHealthText(int health)
    {
        healthText.text = health.ToString();
    }
}
