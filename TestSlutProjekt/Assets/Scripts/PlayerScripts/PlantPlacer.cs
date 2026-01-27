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
        SunText.text = $"Sun: {transform.GetComponent<PlayerStats>().GetSun()}";

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
            print("Hi");

            // Convert mouse position properly for 2D
            Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos3D.z = 0f;
            Vector2 mousePos2D = mousePos3D;

            Collider2D tileCol = Physics2D.OverlapPoint(mousePos2D, tileLayer);
            if (tileCol == null)
            {
                Debug.Log("No tile detected under mouse!");
                return;
            }

            PlantTile tile = tileCol.GetComponent<PlantTile>();
            if (tile == null || tile.isOccupied)
            {
                Debug.Log("Tile invalid or occupied!");
                return;
            }

            Plants plantPrefab = availablePlants[selectedPlantIndex];
            if (!plantPrefab.CheckPrice())
            {
                Debug.Log("Not enough sun to place plant!");
                return;
            }

            plantPrefab.Buy();
            tile.isOccupied = true;

            Instantiate(plantPrefab, tile.plantAnchor.position, Quaternion.identity);

            Debug.Log($"Placed plant: {plantPrefab.name}");
        }
    }
}
