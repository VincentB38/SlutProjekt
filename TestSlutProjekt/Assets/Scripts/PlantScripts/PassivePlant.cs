using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PassivePlant : Plants
{
    [Header("General")]
    public string plantName;
    public int plantPrice;
    public float plantHealth;

    // Variables for visuals

    [Header("SunflowerSettings")]
    public float minimumInterval;
    public float maximumInterval;
    public GameObject sunPrefab; 
    public GameObject sunFolder; 

    [SerializeField] private bool generatesSun;

    // Add animation / animator logic

    private void Start()
    {
        SetValues(plantName, this.plantPrice, plantHealth, transform);

        if (generatesSun)
        {
            StartCoroutine(GenerateSun());
        }
    }

    IEnumerator GenerateSun() // Sunlight generates, spawns clickable prefab
    {
        while (true)
        {
            Instantiate(sunPrefab, transform.position, Quaternion.identity, sunFolder.transform);
            yield return new WaitForSeconds(UnityEngine.Random.Range(minimumInterval, maximumInterval));
        }
    }
}
