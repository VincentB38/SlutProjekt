using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Variables
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject itemDescPrefab;
    public List<Item> plantsItem = new List<Item>();

    void Start()
    {
        itemPrefab.GetComponent<Image>().sprite = plantsItem[0].sprite;

        
    }

    void AssignPanel()
    {
        // Spawn in panel and assign corresponding values
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

// Makes it possible to edit inside Unity Editor
[System.Serializable]
public class Item // More efficient way of creating buttons
{
    public Sprite sprite;
    public int id; // For assigning plant id to vincent's planthandler script
    public Plants plant;
}
