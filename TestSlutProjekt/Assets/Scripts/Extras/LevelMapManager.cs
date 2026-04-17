using UnityEngine;

public class LevelMapManager : MonoBehaviour
{
    public LevelPart[] levelParts;

    void Start()
    {
        int levelIndex = PlayerPrefs.GetInt("PlayerLevel", 0);

        for (int i = 0; i < levelParts.Length; i++)
        {
            if (i <= levelIndex)
                levelParts[i].Unlock();
        }
    }
}