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
        try // Tries to get instance of playerstats
        {
            playerStats = PlayerStats.Instance;
        }
        catch (System.NullReferenceException) // Catches if it doesnt get assigned
        {
            Debug.Log("Playerstats variable in Plants failed to get Instance");
        }
        catch (System.Exception e) // In case other unexpected exceptions occur
        {
            Debug.LogException(e);
        }
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