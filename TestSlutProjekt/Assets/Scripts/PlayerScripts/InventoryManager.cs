using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Variables for Panel
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject itemDescPrefab;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI descText;

    [SerializeField] private PlantPlacer plantPlacer;

    [SerializeField] private float panelTimer; // How long the panel lasts when pressed

    // Variable for the indexed plants
    public List<Item> plantsItem = new List<Item>();

    void Start()
    {
        try
        {
            plantPlacer = GameObject.Find("GameHandler").GetComponent<PlantPlacer>();
        }catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void Update() // Change from update to when pressed according to PlantPlacer script
    {
        AssignPanel(plantsItem[plantPlacer.selectedPlantIndex]);
    }

    void AssignPanel(Item selectedPlant)
    {
        itemPrefab.GetComponent<Image>().sprite = plantsItem[0].sprite;
        // Assign panel's corresponding values
        nameText.text = "Name: " + selectedPlant.plant.GetName();
        priceText.text = "Price: " + selectedPlant.plant.GetPrice().ToString();
        statsText.text = "Hp/Dmg: " + selectedPlant.plant.GetHealth() + " / " + selectedPlant.plant.GetDamage();
        descText.text = "Desc: " + selectedPlant.description;
    }

    IEnumerator ShowPanel() // Shows panel for a brief time period
    {
        itemDescPrefab.SetActive(true);
        itemPrefab.SetActive(true);
        yield return new WaitForSeconds(panelTimer);
        itemDescPrefab.SetActive(false);
        itemPrefab.SetActive(false);
    }
}

// Makes it possible to edit inside Unity Editor
[System.Serializable]
public class Item // More efficient way of creating buttons
{
    public Sprite sprite;
    public int id; // For assigning plant id to vincent's planthandler script
    public Plants plant;
    public string description;
}
