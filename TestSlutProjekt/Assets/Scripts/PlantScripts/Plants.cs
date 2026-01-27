using UnityEngine;

public class Plants : MonoBehaviour
{
    string name;
    int price;
    float health;

    Transform playerTransform;
    PlayerStats playerStats;

    // Function that basically replaces a constructor since we are using prefabs with subclasses integrated
    public void SetValues(string name, int price, float health, Transform playerTransform)
    {
        this.name = name;
        this.price = price;
        this.health = health;
        this.playerTransform = playerTransform;
        playerStats = playerTransform.GetComponent<PlayerStats>();
    }

    // Function to change health
    public void ChangeHealth(float amount)
    {
        health += amount;
    }

    public bool CheckPrice()
    {
        if (playerStats.GetMoney() >= price) // Checks if player has enough money
        {
            return true; // Sends a true value to the placement script to allow placement
        }
        else
        {
            return false; // Sends a false value to the placement script to deny placement
        }
    }

    public void Buy()
    {
        playerStats.ChangeMoney(-price);
    }

    // The Get functions
    #region GetFunctions
    public string GetName()
    {
        return name;
    }

    public int GetPrice()
    {
        return price;
    }

    public float GetHealth()
    {
        return health;
    }
    #endregion
}