using UnityEngine;

public class LevelMapManager : MonoBehaviour
{
    public LevelPart[] levelParts;
    public PathGenerator pathGenerator;

    void Start()
    {

        int levelIndex = PlayerPrefs.GetInt("MaxLevel", 0); // get the max level and if it does not exist then 0
        levelIndex = 0;
        // unlock levels
        for (int i = 0; i < levelParts.Length; i++) // Loop through
        {
            if (levelIndex > i) // if level index is more than i then that means you unlocked it
                levelParts[i].Unlock(); // disable the lock
        }

        if (pathGenerator != null)
        {
            pathGenerator.maxLevelIndex = levelIndex; // Let the path generator know the length it should do
            pathGenerator.Play(); // Start it
        }
    }
}