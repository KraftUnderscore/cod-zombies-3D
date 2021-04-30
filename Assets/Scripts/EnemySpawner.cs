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

    private List<GameObject> enemies;

    [SerializeField] private float spawnPhaseDuration;
    [SerializeField] private float enemiesIncrease;
    private int currentMaxEnemies = 10;
    public int CurrentMaxEnemies
    {
        get
        {
            return currentMaxEnemies;
        }
    }

    private void Start()
    {
        currentSpawnPoints = new List<Transform>();
        enemies = new List<GameObject>();
    }

    public void SpawnRound()
    {
        float spawnInterval = spawnPhaseDuration / currentMaxEnemies;
        for(int i = 0; i < currentMaxEnemies; i++)
            StartCoroutine(SpawnEnemy(spawnInterval * (i + 1)));
        currentMaxEnemies = (int)((1 + enemiesIncrease) * currentMaxEnemies);
    }

    private IEnumerator SpawnEnemy(float delay)
    {
        yield return new WaitForSeconds(delay);

        CreateEnemy(GetSpawnPoint());
    }

    private void CreateEnemy(Transform spawnPoint)
    {
        GameObject enemyObj = GetEnemyObject();
        enemyObj.transform.position = spawnPoint.position;
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.health = 100f;
        enemy.isPursuing = false;
        enemyObj.SetActive(true);
        StartCoroutine(AfterSpawnDelay(enemy));
    }

    private IEnumerator AfterSpawnDelay(Enemy enemy)
    {
        yield return new WaitForSeconds(Random.Range(afterSpawnDelayMin, afterSpawnDelayMax));
        enemy.isPursuing = true;
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
