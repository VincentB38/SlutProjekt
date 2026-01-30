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
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MoneyFlow()); // Gives sun(money) between intervals
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator MoneyFlow() // Sunlight basically
    {
        while (true)
        {
            sun += gainMoney;
            yield return new WaitForSeconds(timer);
        }
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
