using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI roundText;

    private void Start()
    {
        itemText.gameObject.SetActive(false);
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateHealthText(float health)
    {
        healthText.text = health.ToString("000");
    }
    public void UpdateRoundText(int round)
    {
        roundText.text = round.ToString();
    }

    public void HighlightItem(ShopItem shopItem)
    {
        itemText.text = "Press E to buy " + shopItem.name + " for " + shopItem.price + " points.";
        itemText.gameObject.SetActive(true);
    }

    public void HideItem()
    {
        itemText.gameObject.SetActive(false);
    }
}
