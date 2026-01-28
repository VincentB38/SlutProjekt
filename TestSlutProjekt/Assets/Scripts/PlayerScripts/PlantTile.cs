using UnityEngine;

public class PlantTile : MonoBehaviour
{
    public bool isOccupied;
    public Transform plantAnchor;
    private void Awake()
    {
        if (plantAnchor == null)
            plantAnchor = transform;
    }
}