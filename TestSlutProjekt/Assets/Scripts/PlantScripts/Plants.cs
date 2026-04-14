using System.Collections;
using UnityEngine;

public class Plants : MonoBehaviour
{
    public string name; // Remove name var. from other subclasses
    public int price;
    public float health;

    protected Transform playerTransform;
    PlayerStats playerStats;

    private void Start()
    {
        try
        {
            playerStats = PlayerStats.Instance;
        }
    }


    // Function that basically replaces a constructor since we are using prefabs with subclasses integrated
    public virtual void SetValues(string name, int price, float health, Transform playerTransform)
    {
        this.pName = name;
        this.price = price;
        this.health = health;
        this.playerTransform = playerTransform;
        playerStats = playerTransform.GetComponent<PlayerStats>();
    }

    // Function to change health
    public void ChangeHealth(float amount)
    {
        health += amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool CheckPrice()
    {
        if (playerStats.GetSun() >= price) // Checks if player has enough money
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
        return pName;
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