using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantPlacementController : MonoBehaviour
{
    [Header("Plants")]
   // public Plant[] availablePlants;   // assign prefabs in inspector
    public int selectedPlantIndex = 0;

    [Header("Placement")]
    [SerializeField] LayerMask tileLayer;

    [Header("Resources")]
    public TextMeshPro SunText;
    public int Sun = 150;

    void Update()
    {
        SunText.text = $"Sun: {Sun}";

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        // Change selected plant (number keys)
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedPlantIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedPlantIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedPlantIndex = 2;

        // Place plant
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D tileCol = Physics2D.OverlapPoint(mousePos, tileLayer);
            if (tileCol == null) return;

            PlantTile tile = tileCol.GetComponent<PlantTile>();
            if (tile == null || tile.isOccupied) return;

           // Plant plantPrefab = availablePlants[selectedPlantIndex];
            //if (Sun < plantPrefab.sunCost) return;

          //  Sun -= plantPrefab.sunCost;
            tile.isOccupied = true;

           // Instantiate(
           //     plantPrefab,
           //     tile.plantAnchor.position,
           //     Quaternion.identity
           // );
        }
    }
}
