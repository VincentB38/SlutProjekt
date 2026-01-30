using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PassivePlant : Plants
{
    [SerializeField] private string name;
    [SerializeField] private int price;
    [SerializeField] private float health;
    [SerializeField] private float timer; // Change name

    [SerializeField] private bool generatesSun;

    public void GenerateSun()
    {

    }

    IEnumerator MoneyFlow() // Sunlight basically
    {
        //sun += gainMoney;
        yield return new WaitForSeconds(timer);
    }
}
