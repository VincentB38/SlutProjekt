using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantPlacementController : MonoBehaviour
{
    [Header("Plants")]
    public Plants[] availablePlants;
    public GameObject PlantHolder;
    public int selectedPlantIndex = 0;
    private GameObject currentPreview;

    [Header("Placement")]
    [SerializeField] LayerMask tileLayer;
    Key[] numberKeys;

    public TextMeshProUGUI SunText;

    private void Start()
    {
        numberKeys = new Key[] {Key.Digit1, Key.Digit2,Key.Digit3,Key.Digit4,Key.Digit5,Key.Digit6,Key.Digit7,Key.Digit8,Key.Digit9}; // keybinds for plants
    }

    void Update()
    {
        SunText.text = $"Paper: {transform.GetComponent<PlayerStats>().GetSun()}"; // amount of sun you have

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Change selected plant with number keys
        for (int i = 0; i < availablePlants.Length && i < numberKeys.Length; i++)
        {
            if (Keyboard.current[numberKeys[i]].wasPressedThisFrame)
            {
                if (currentPreview != null)
                {
                    Destroy(currentPreview); // Destroy the preview if it was being used
                }

                if (i == selectedPlantIndex)
                {
                    selectedPlantIndex = 10; // deselect
                    Debug.Log($"Deselected plant index: {i}");
                }
                else
                {
                    selectedPlantIndex = i;
                    Debug.Log($"Selected plant index: {i}");

                    // Create preview
                    Plants plantPrefab = availablePlants[selectedPlantIndex];
                    currentPreview = Instantiate(plantPrefab.gameObject);

                    SetTransparency(currentPreview, 0.5f);

                    foreach (var script in currentPreview.GetComponentsInChildren<MonoBehaviour>()) // Disable scripts to prevent the plants attacking etc
                    {
                        script.enabled = false;
                    }

                    // Disable colliders so it doesn't block placement
                    foreach (var col in currentPreview.GetComponentsInChildren<Collider2D>())
                    {
                        col.enabled = false;
                    }
                }
            }
        }

        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // Convert mouse position
        mousePos3D.z = 0f;
        Vector2 mousePos2D = mousePos3D;
        Collider2D tileCol = Physics2D.OverlapPoint(mousePos2D, tileLayer);// see if the mouse is on the tiler
        // Place plant
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (tileCol == null) // if no tile is under the mouse
            {
                Debug.Log("No tile detected under mouse!");
                return;
            }

            PlantTile tile = tileCol.GetComponent<PlantTile>(); // get the plant tile
            if (tile == null || tile.isOccupied) // if no tile or the tile is already being used
            {
                Debug.Log("Tile invalid or occupied!");
                return;
            }

            if (selectedPlantIndex < availablePlants.Length) // make sure the plant exists
            {
                Plants plantPrefab = availablePlants[selectedPlantIndex]; // find the plant prefab

                //if (!plantPrefab.CheckPrice())
                // {
                //    Debug.Log("Not enough sun to place plant!");
                //  return;
                // }

                // plantPrefab.Buy();

                tile.isOccupied = true; // tile is being used

                Instantiate(plantPrefab, tile.plantAnchor.position + new Vector3(0, 0, -1), Quaternion.identity, PlantHolder.transform); // spawn in the plant

                Debug.Log($"Placed plant: {plantPrefab.name}");
            }

        }

        if (currentPreview != null) // if the preview is being used
        {
            if (tileCol) // if the tile is under the mouse
            {
                PlantTile tile = tileCol.GetComponent<PlantTile>();

                if (tile != null && !tile.isOccupied)
                {
                    currentPreview.SetActive(true); // make it show
                    currentPreview.transform.position = tile.plantAnchor.position + new Vector3(0, 0, -1);

                    SetTransparency(currentPreview, 0.5f); // send the preview and make it half transparent
                }
                else
                {
                    currentPreview.SetActive(false); // make preview hide
                }
            }
            else
            {
                currentPreview.SetActive(false); // if no tile under mouse then hide
            }
        }
    }

    void SetTransparency(GameObject obj, float alpha) // make it semi trasnparent
    {
        foreach (var renderer in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = renderer.color;
            c.a = alpha;
            renderer.color = c;
        }
    }
}
