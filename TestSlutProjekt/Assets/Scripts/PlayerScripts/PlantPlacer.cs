using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantPlacementController : MonoBehaviour
{
    [Header("Plants")]
    public Plants[] availablePlants;   // assign prefabs in inspector
    public int selectedPlantIndex = 0;

    [Header("Placement")]
    [SerializeField] LayerMask tileLayer;

    public TextMeshProUGUI SunText;

    void Update()
    {
        SunText.text = $"Sun: {transform.GetComponent<PlayerStats>().GetMoney()}";

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        // Change selected plant (number keys)
        if (Keyboard.current.digit1Key.wasPressedThisFrame) selectedPlantIndex = 0;
        if (Keyboard.current.digit2Key.wasPressedThisFrame) selectedPlantIndex = 1;
        if (Keyboard.current.digit3Key.wasPressedThisFrame) selectedPlantIndex = 2;

        // Place plant
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Collider2D tileCol = Physics2D.OverlapPoint(mousePos, tileLayer);
            if (tileCol == null) return;

            PlantTile tile = tileCol.GetComponent<PlantTile>();
            if (tile == null || tile.isOccupied) return;

           Plants plantPrefab = availablePlants[selectedPlantIndex];
            //if (plantPrefab.CheckPrice() == false) return;

            plantPrefab.Buy();
            tile.isOccupied = true;

           Instantiate(
               plantPrefab,
                tile.plantAnchor.position,
                Quaternion.identity
            );

            Debug.Log($"Placed plant: {plantPrefab.name}");
        }
    }
}
