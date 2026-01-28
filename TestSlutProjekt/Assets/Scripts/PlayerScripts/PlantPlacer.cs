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
    Key[] numberKeys;

    public TextMeshProUGUI SunText;

    private void Start()
    {
        numberKeys = new Key[] {Key.Digit1, Key.Digit2,Key.Digit3,Key.Digit4,Key.Digit5,Key.Digit6,Key.Digit7,Key.Digit8,Key.Digit9};
    }

    void Update()
    {
        SunText.text = $"Sun: {transform.GetComponent<PlayerStats>().GetSun()}"; // amount of sun you have

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Change selected plant (number keys)
        for (int i = 0; i < availablePlants.Length && i < numberKeys.Length; i++)
        {
            if (Keyboard.current[numberKeys[i]].wasPressedThisFrame)
            {
                selectedPlantIndex = i;
                Debug.Log($"Selected plant index: {i}");
            }
        }

        // Place plant
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {

            // Convert mouse position
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
            //if (!plantPrefab.CheckPrice())
           // {
            //    Debug.Log("Not enough sun to place plant!");
              //  return;
           // }

           // plantPrefab.Buy();
            tile.isOccupied = true;

            Instantiate(plantPrefab, tile.plantAnchor.position, Quaternion.identity);

            Debug.Log($"Placed plant: {plantPrefab.name}");
        }
    }
}
