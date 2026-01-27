using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


[System.Serializable]
public class EnemyLevel
{
    public string levelName;
    public List<EnemyWave> waves = new List<EnemyWave>();
}

[System.Serializable]
public class EnemyGroup // Creates the information for each group
{
    public GameObject enemyPrefab;
    public int quantity = 1;
    public float spawnInterval = 0.5f;
    public int maxAliveEnemies = 3;
}

[System.Serializable]
public class EnemyWave
{
    public string waveName;
    public List<EnemyGroup> groups = new List<EnemyGroup>();
}

public class EnemyLevelSHandler : MonoBehaviour
{
    public List<EnemyLevel> levels = new List<EnemyLevel>();
    private int levelIndex = 0;

    private EnemyLevel currentLevel;

    List<Transform> spawnPoints = new List<Transform>();

    public GameObject SpawnPointsArea;

    public TMP_Text waveText;

    public Transform enemyFolder;

    private int currentWaveIndex = 0;
    private int aliveEnemies = 0;

    public float wavePopDuration = 0.5f;   // How long it grows/shrinks
    public float waveDisplayDuration = 1f; // How long it stays on full size

    // private GameObject Player;

    private void Start()
    {
        foreach (Transform spawns in SpawnPointsArea.transform)
        {
            spawnPoints.Add(spawns);
        }

        waveText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        waveText.rectTransform.localScale = Vector3.zero;

        levelIndex = PlayerPrefs.GetInt("PlayerLevel");

        if (levels.Count == 0)
        {
            Debug.LogWarning("No levels configured!");
            return;
        }

        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError("Invalid level index!");
            return;
        }

        currentLevel = levels[levelIndex];
        currentWaveIndex = 0;

        StartCoroutine(RunWave(currentLevel.waves[currentWaveIndex]));
    }


    private IEnumerator ShowWaveText(string text)
    {
        if (waveText == null)
            yield break;

        waveText.gameObject.SetActive(true);
        waveText.text = text;

        Vector3 startScale = Vector3.zero;
        Vector3 fullScale = Vector3.one * 1.5f;
        waveText.transform.localScale = startScale;

        // Grow
        float t = 0f;
        while (t < wavePopDuration)
        {
            t += Time.deltaTime;
            float scale = Mathf.SmoothStep(0f, 1f, t / wavePopDuration);
            waveText.transform.localScale = fullScale * scale;
            yield return null;
        }
        waveText.transform.localScale = fullScale;
        // Stay big for a bit
        yield return new WaitForSeconds(waveDisplayDuration);

        // Shrink
        t = 0f;
        while (t < wavePopDuration)
        {
            t += Time.deltaTime;
            float scale = Mathf.SmoothStep(1f, 0f, t / wavePopDuration);
            waveText.transform.localScale = fullScale * scale;
            yield return null;
        }

        waveText.gameObject.SetActive(false);
    }


    private IEnumerator RunWave(EnemyWave wave)
    {
        yield return StartCoroutine(ShowWaveText($"{wave.waveName}"));

        // 1s delay before enemies spawn
        yield return new WaitForSeconds(1f);

        Debug.Log($"Starting Wave: {wave.waveName}");

        foreach (var group in wave.groups) // Loop through the groups in the waves
        {
            int spawnedInGroup = 0;
            while (spawnedInGroup < group.quantity)
            {
                if (aliveEnemies < group.maxAliveEnemies) // if enemies are less than max then spawn
                {
                    SpawnEnemy(group);
                    spawnedInGroup++;
                }
                yield return new WaitForSeconds(group.spawnInterval);
            }
        }

        // Wait until all enemies die
        while (aliveEnemies > 0)
            yield return null;

        currentWaveIndex++;

        if (currentWaveIndex < currentLevel.waves.Count)
        {
            yield return StartCoroutine(RunWave(currentLevel.waves[currentWaveIndex]));
        }
        else
        {
            // Level Finished
            yield return StartCoroutine(ShowWaveText("Level Complete!"));
            Debug.Log("Level completed!");

            // Win logic here
            // Player.GetComponent<PlayerHandler>().EndGame(true);
        }
    }

    private void SpawnEnemy(EnemyGroup group) // Spawn enemies
    {
        if (spawnPoints.Count == 0) // if there is no spawnpoitns assigned
        {
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)]; // Randomly spawn between the assigned spawnpoints
        GameObject enemy = Instantiate(group.enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        aliveEnemies++;

        //EnemyTracker tracker = enemy.AddComponent<EnemyTracker>();
      //  tracker.spawner = this;
    }

    public void OnEnemyDestroyed() // When enemy dies
    {
        aliveEnemies--;
    }
}
