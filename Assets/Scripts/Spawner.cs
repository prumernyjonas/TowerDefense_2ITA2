using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyPrefabWithSpawnOnce
    {
        public GameObject enemyPrefab;
        public bool spawnOnce = false;
        [HideInInspector]
        public bool hasSpawned = false;
    }

    [SerializeField]
    private List<EnemyPrefabWithSpawnOnce> enemyPrefabs;

    [SerializeField]
    private float spawnInterval;

    [SerializeField]
    private Text waveText;

    [SerializeField]
    private string attackAnimationName = "Attack";

    private int currentWaveIndex = 1;
    private int enemiesSpawnedInCurrentWave = 0; // Proměnná pro sledování počtu spawnutých nepřátel v aktuální vlně

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogError("No enemy prefabs assigned!");
            return;
        }

        // Kontrola, zda byl dosažen limit pro první vlnu
        if (currentWaveIndex == 0 && enemiesSpawnedInCurrentWave >= 30)
        {
            Debug.Log("First wave limit reached.");
            return;
        }

        EnemyPrefabWithSpawnOnce randomPrefab = GetRandomPrefab();

        if (randomPrefab != null)
        {
            if (randomPrefab.spawnOnce && randomPrefab.hasSpawned)
            {
                Debug.Log("This enemy has already been spawned!");
                return;
            }

            GameObject newEnemy = Instantiate(randomPrefab.enemyPrefab, transform.position, Quaternion.identity);

            Animator enemyAnimator = newEnemy.GetComponent<Animator>();
            if (enemyAnimator != null)
            {
                enemyAnimator.Play(attackAnimationName);
            }
            else
            {
                Debug.LogWarning("Animator component not found on enemy prefab.");
            }

            if (randomPrefab.spawnOnce)
            {
                randomPrefab.hasSpawned = true;
            }

            // Zvýšení počtu spawnutých nepřátel v aktuální vlně
            enemiesSpawnedInCurrentWave++;

            // Aktualizace textu aktuální vlny
            UpdateWaveText();
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

    private void UpdateWaveText()
    {
        waveText.text = "Wave " + (currentWaveIndex + 1);
    }
}
