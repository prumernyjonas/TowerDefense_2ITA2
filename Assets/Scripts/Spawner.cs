using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public int bossCount;
        public float spawnInterval;
    }

    [System.Serializable]
    public class EnemyPrefabWithSpawnOnce
    {
        public GameObject enemyPrefab;
        public bool spawnOnce = false;
        [HideInInspector]
        public bool hasSpawned = false;
    }

    [SerializeField]
    private List<Wave> waves;
    [SerializeField]
    private List<EnemyPrefabWithSpawnOnce> enemyPrefabs;

    [SerializeField]
    private Text waveText;

 

    private int currentWaveIndex = 0;
    private int enemiesSpawnedInCurrentWave = 0;
    private int bossesSpawnedInCurrentWave = 0;
    private bool isPaused = false;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            if (isPaused)
            {
                yield return new WaitForSeconds(5);
                isPaused = false;
                currentWaveIndex++;
                enemiesSpawnedInCurrentWave = 0;
                bossesSpawnedInCurrentWave = 0;
                ResetHasSpawned(); // Resetování hodnoty hasSpawned pro všechny prefaby
                UpdateWaveText();
            }

            if (currentWaveIndex < waves.Count)
            {
                Wave currentWave = waves[currentWaveIndex];
                if (enemiesSpawnedInCurrentWave < currentWave.enemyCount || bossesSpawnedInCurrentWave < currentWave.bossCount)
                {
                    yield return new WaitForSeconds(currentWave.spawnInterval);
                    SpawnEntity();
                }
                else
                {
                    isPaused = true;
                }
            }
            else
            {
                yield break; // Ukončení cyklu, pokud nejsou k dispozici žádné další wavy
            }
        }
    }
    
    private void SpawnEntity()
    {
        if (enemiesSpawnedInCurrentWave < waves[currentWaveIndex].enemyCount)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        EnemyPrefabWithSpawnOnce randomPrefab = GetRandomPrefab();
        if (randomPrefab != null)
        {
            GameObject newEnemy = Instantiate(randomPrefab.enemyPrefab, transform.position, Quaternion.identity);
            Animator enemyAnimator = newEnemy.GetComponent<Animator>();
            
            if (randomPrefab.spawnOnce)
            {
                randomPrefab.hasSpawned = true;
            }
            enemiesSpawnedInCurrentWave++;
        }
        else
        {
            Debug.LogWarning("No available enemy prefabs.");
        }
    }

    private EnemyPrefabWithSpawnOnce GetRandomPrefab()
    {
        List<EnemyPrefabWithSpawnOnce> availablePrefabs = new List<EnemyPrefabWithSpawnOnce>();
        foreach (var prefab in enemyPrefabs)
        {
            if (!prefab.hasSpawned || !prefab.spawnOnce)
            {
                availablePrefabs.Add(prefab);
            }
        }
        if (availablePrefabs.Count > 0)
        {
            return availablePrefabs[Random.Range(0, availablePrefabs.Count)];
        }
        else
        {
            return null;
        }
    }

    private void ResetHasSpawned()
    {
        foreach (var prefab in enemyPrefabs)
        {
            prefab.hasSpawned = false;
        }
    }

    private void UpdateWaveText()
    {
        if (currentWaveIndex < waves.Count)
        {
            waveText.text = "Wave " + (currentWaveIndex + 1);
        }
        else
        {
            waveText.text = "All waves completed.";
        }
    }
}
