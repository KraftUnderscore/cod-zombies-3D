using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int totalScore = 0;
    private UIManager uIManager;
    private EnemySpawner spawner;

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
        currentMaxEnemies = spawner.CurrentMaxEnemies;
        spawner.SpawnRound();
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

    private IEnumerator StartNextRound()
    {
        currentRound++;
        IncreaseScore(currentRound * scorePerRound);
        yield return new WaitForSeconds(timeBetweenRounds);
        currentMaxEnemies = spawner.CurrentMaxEnemies;
        spawner.SpawnRound();
    }
}
