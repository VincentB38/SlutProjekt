using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    static public PlayerStats Instance;

    private int sun;
    [SerializeField] private int gainMoney;
    [SerializeField] private int health;

    private int level;
    private int maxLevel;

    [SerializeField] private float timer = 0.5f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;

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

    public void ChangeHealth(int amount)
    {
        health += amount;

        // Add so something happens when health is under 0
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
