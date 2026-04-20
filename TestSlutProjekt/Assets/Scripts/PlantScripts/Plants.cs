using System.Collections;
using UnityEngine;

public class Plants : MonoBehaviour
{
    /*
     * Add sprite and animation logic inside here
     */

    [Header("General")] // General Variables
    [SerializeField] private string plantName;
    [SerializeField] private int price;
    [SerializeField] private float health;

    protected Transform playerTransform;
    PlayerStats playerStats; // PlayerStats instance

    PlantTile tile;

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

        if (health <= 0) // If health goes below or equal to zero, it dies.
        {
            tile.isOccupied = false; // Makes the tile unoccupied for Vincents code
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

    public void Buy() // Efficient way to reduce price without needing other variables
    {
        playerStats.ChangeMoney(-price);
    }

    public void SetTile(PlantTile tile) // Allows tile setting from Vincents code
    {
        this.tile = tile;
    }

    // The Get functions
    #region GetFunctions
    public string GetName()
    {
        return plantName;
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