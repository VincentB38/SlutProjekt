using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Currency : MonoBehaviour
{
    private int money;
    [SerializeField] private int gainMoney;

    private int level;

    private float timer;

    // Make money increase slowly through time, possible to add multiplier to the time it takes to generate money
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MoneyFlow()); // Gives money through specific intervals
    }

    IEnumerator MoneyFlow()
    {
        money += gainMoney;
        yield return new WaitForSeconds(timer);
    }

    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.Save();
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt("PlayerLevel");
    }

    public int GetMoney()
    {
        return money;
    }

    public void ChangeMoney(int amount)
    {
        money += amount;
    }
}
