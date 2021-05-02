using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int totalScore = 0;

    private UIManager uIManager;
    private EnemySpawner spawner;
    private PlayerInventory playerInventory;

    private int currentEnemiesKilled = 0;
    private int currentMaxEnemies = 0;

    private int currentRound = 1;

    [SerializeField] private int scorePerRound = 100;
    [SerializeField] private float timeBetweenRounds = 60f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else DestroyImmediate(gameObject);
    }

    private void Start()
    {
        uIManager = GetComponent<UIManager>();
        spawner = FindObjectOfType<EnemySpawner>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        currentMaxEnemies = spawner.MaxEnemies;
        uIManager.UpdateRoundText(currentRound);
        spawner.SpawnRound();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
        }

        if (item == null) return;
        if(Input.GetKeyDown(KeyCode.E) && item.price <= totalScore)
        {
            if (item.isBarrier)
            {
                item.item.SetActive(false);
                spawner.IsPhaseTwo = true;
            }
            else playerInventory.AddItem(item.item);
            item.gameObject.SetActive(false);
            IncreaseScore(-item.price);
            HideShopItem();
        }
    }

    public void IncreaseScore(int amount)
    {
        totalScore += amount;
        uIManager.UpdateScoreText(totalScore);
    }

    public void EnemyKilled()
    {
        currentEnemiesKilled++;
        if (currentEnemiesKilled == currentMaxEnemies)
            StartCoroutine(StartNextRound());
    }

    public void UpdateHealth(float health)
    {
        if (health <= 0f) SceneManager.LoadScene(0);
        else uIManager.UpdateHealthText(health);
    }

    private ShopItem item;

    public void ShowShopItem(ShopItem shopItem)
    {
        item = shopItem;
        uIManager.HighlightItem(shopItem);
    }

    public void HideShopItem()
    {
        item = null;
        uIManager.HideItem();
    }

    private IEnumerator StartNextRound()
    {
        currentEnemiesKilled = 0;
        IncreaseScore(currentRound * scorePerRound);
        currentRound++;
        uIManager.UpdateRoundText(currentRound);
        yield return new WaitForSeconds(timeBetweenRounds);
        currentMaxEnemies = spawner.MaxEnemies;
        spawner.SpawnRound();
    }
}
