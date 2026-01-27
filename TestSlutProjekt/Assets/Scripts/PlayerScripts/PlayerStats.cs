using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int sun;
    [SerializeField] private int gainMoney;

    private int level;
    private int maxLevel;

    [SerializeField] private float timer;

    // Make money increase slowly through time, possible to add multiplier to the time it takes to generate money
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MoneyFlow()); // Gives sun(money) through specific intervals
    }

    IEnumerator MoneyFlow() // Sunlight basically
    {
        sun += gainMoney;
        yield return new WaitForSeconds(timer);
    }

    public void SetLevel(int level)
    {
        if (level > maxLevel) // Checks if level is bigger than maxlevel, adjusts the maxlevel
        {
            PlayerPrefs.SetInt("PlayerMaxLevel", level);
        }

        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.Save();
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt("PlayerLevel");
    }

    public int GetSun()
    {
        return sun;
    }

    public void ChangeMoney(int amount)
    {
        sun += amount;
    }
}
