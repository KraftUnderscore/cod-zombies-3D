using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI itemText;

    private void Start()
    {
        itemText.gameObject.SetActive(false);
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateHealthText(int health)
    {
        healthText.text = health.ToString();
    }

    public void HighlightItem(ShopItem shopItem)
    {
        itemText.text = "Buy " + shopItem.name + " for " + shopItem.price + "?";
        itemText.gameObject.SetActive(true);
    }

    public void HideItem()
    {
        itemText.gameObject.SetActive(false);
    }
}
