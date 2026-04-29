using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;


[System.Serializable]
public class EnemyLevel
{
    public string levelName;
    public float startDelay = 2f; // time before the wave starts
    public List<EnemyWave> waves = new List<EnemyWave>();
}

[System.Serializable]
public class EnemyTypeEntry
{
    public GameObject enemyPrefab; // the enemy prefab
    public int totalAmount; // total amount of that enemy
}

[System.Serializable]
public class EnemyGroup // Creates the information for each group
{
    public List<EnemyTypeEntry> enemyTypes = new List<EnemyTypeEntry>();
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
        PlayerPrefs.SetInt("PlayerLevel", 0); // Reset level for testing, REMOVE WHEN EVERYTHING IS DONE
        foreach (Transform spawns in SpawnPointsArea.transform)
        {
            spawnPoints.Add(spawns); // add in the spawn points
        }
        

        waveText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        waveText.rectTransform.localScale = Vector3.zero;

        levelIndex = PlayerPrefs.GetInt("PlayerLevel"); // get the level chosen

        if (levels.Count == 0)
        {
            Debug.LogWarning("No levels configured!");
            return;
        }

        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            print(levelIndex);
            Debug.LogError("Invalid level index!");
            return;
        }

        currentLevel = levels[levelIndex]; // get the current level
        currentWaveIndex = 0;

        StartCoroutine(RunLevel()); // start the level
    }

    private IEnumerator RunLevel()
    {
        yield return new WaitForSeconds(currentLevel.startDelay); // timer before enemies spawn so player has time to prepare

        StartCoroutine(RunWave(currentLevel.waves[currentWaveIndex]));// start running the wave
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

        waveText.gameObject.SetActive(false); // hide the text
    }


    private IEnumerator RunWave(EnemyWave wave)
    {
        yield return StartCoroutine(ShowWaveText($"{wave.waveName}")); // show the wave name

        // 1s delay before enemies spawn
        yield return new WaitForSeconds(1f);

        Debug.Log($"Starting Wave: {wave.waveName}");

        foreach (var group in wave.groups) // Loop through the groups in the waves
        {
            Dictionary<GameObject, int> remaining = new Dictionary<GameObject, int>();
            int totalToSpawn = 0;

            foreach (var type in group.enemyTypes) // go through the enemy types inside of the group
            {
                remaining[type.enemyPrefab] = type.totalAmount; // keep track of the amount of that unit has been spawned
                totalToSpawn += type.totalAmount;
            }

            int spawned = 0;

            while (spawned < totalToSpawn)
            {
                if (aliveEnemies < group.maxAliveEnemies) // if enemies are less than max then spawn
                {
                    GameObject chosen = GetRandomEnemy(remaining);

                    if (chosen != null)
                    {
                        SpawnEnemy(chosen); // spawn the enemy
                        remaining[chosen]--;
                        spawned++;
                    }
                }
                yield return new WaitForSeconds(group.spawnInterval); // wait for the assigned wait time
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

            int PlrLevel = PlayerPrefs.GetInt("PlayerLevel", 0); // get their current level
            int MaxLevelUnlocked = PlayerPrefs.GetInt("MaxLevel", 0); // Get their max Levvel

            if (PlrLevel == MaxLevelUnlocked) // incase you replay a stage, this just makes sure you can't unlock new stages by playing previous ones
            {
                PlayerPrefs.SetInt("MaxLevel", MaxLevelUnlocked + 1); // increase max level by 1
            }
            yield return new WaitForSeconds(1f);
            SceneHandler.Instance.BackToLevelMenu(); // go back to level menu (CHANGE LATER IF WE WANT A PLAY SCREEN)
        }
    }

    GameObject GetRandomEnemy(Dictionary<GameObject, int> remaining) // using dictionary to store the enemies, for example RegularZombie = 3, FastZombie = 2;
        // instead of using a list that just returns index 0 is RegularZombie
    {
        List<GameObject> available = new List<GameObject>(); // create a list to store enemy types that still have enemies left to spawn

        foreach (var kv in remaining) // go through each enemy  (k stands for Key and v for Value)
        {
            if (kv.Value > 0) // only include the enemy type if there's some left
                available.Add(kv.Key); // add it in
        }

        if (available.Count == 0) // if everything has spawned in for that enemy
            return null;

        return available[Random.Range(0, available.Count)]; // pick a random enemy from the available one
    }

    private void SpawnEnemy(GameObject prefab) // Spawn enemies
    {
        if (spawnPoints.Count == 0) // if there is no spawnpoitns assigned
        {
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)]; // Randomly spawn between the assigned spawnpoints
        GameObject enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        aliveEnemies++;

        enemy.GetComponent<EnemyHandler>().SetEnemyLane(int.Parse(spawnPoint.name)); // Set the enemies lane based of the spawnpoint name

    }

    public void OnEnemyDestroyed() // When enemy dies
    {
        aliveEnemies--;
    }
}