using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPointsFirst;
    [SerializeField] private Transform[] spawnPointsSecond;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float afterSpawnDelayMin, afterSpawnDelayMax;

    private List<Transform> currentSpawnPoints;
    private bool isPhaseTwo = false;
    public bool IsPhaseTwo
    {
        set
        {
            isPhaseTwo = value;
        }
    }

    private List<GameObject> enemies;

    [SerializeField] private float startingSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedIncrease;
    [SerializeField] private float startingHealth;
    [SerializeField] private float healthIncrease;
    [SerializeField] private float spawnPhaseDuration;
    [SerializeField] private float enemiesIncrease;
    [SerializeField] private int maxEnemies;

    public int MaxEnemies
    {
        get
        {
            return maxEnemies;
        }
    }

    private void Start()
    {
        currentSpawnPoints = new List<Transform>();
        enemies = new List<GameObject>();
    }

    public void SpawnRound()
    {
        float spawnInterval = spawnPhaseDuration / maxEnemies;
        for(int i = 0; i < maxEnemies; i++)
            StartCoroutine(SpawnEnemy(spawnInterval * (i + 1)));
    }

    private IEnumerator SpawnEnemy(float delay)
    {
        yield return new WaitForSeconds(delay);
        spawned++;
        CreateEnemy(GetSpawnPoint());
        IncreaseStats();
    }

    private int spawned = 0;

    private void IncreaseStats()
    {
        if (spawned != maxEnemies) return;
        maxEnemies = (int)((1 + enemiesIncrease) * maxEnemies);
        startingHealth *= (1 + healthIncrease);
        startingSpeed *= (1 + speedIncrease);
        startingSpeed = Mathf.Min(startingSpeed, maxSpeed);
        spawned = 0;
    }

    private void CreateEnemy(Transform spawnPoint)
    {
        GameObject enemyObj = GetEnemyObject();
        enemyObj.transform.position = spawnPoint.position;
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.health = startingHealth * 2f;
        enemy.Speed = startingSpeed;
        enemyObj.SetActive(true);
        enemy.SwitchAgent(true);
        StartCoroutine(AfterSpawnDelay(enemy));
    }

    private IEnumerator AfterSpawnDelay(Enemy enemy)
    {
        yield return new WaitForSeconds(Random.Range(afterSpawnDelayMin, afterSpawnDelayMax));
        if(enemy.gameObject.activeSelf) enemy.SwitchAgent(false);
    }

    private GameObject GetEnemyObject()
    {
        foreach (var enemy in enemies) if (!enemy.activeSelf) return enemy;
        return CreateEnemyObject();
    }

    private GameObject CreateEnemyObject()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform);
        enemies.Add(newEnemy);
        return newEnemy;
    }

    private Transform GetSpawnPoint()
    {
        if(currentSpawnPoints.Count == 0) PopulateSpawnPointList();

        Transform nextPoint = currentSpawnPoints[Random.Range(0, currentSpawnPoints.Count)];
        currentSpawnPoints.Remove(nextPoint);

        return nextPoint;
    }   
    
    private void PopulateSpawnPointList()
    {
        currentSpawnPoints.AddRange(spawnPointsFirst);
        if (isPhaseTwo) currentSpawnPoints.AddRange(spawnPointsSecond);
    }    
}
