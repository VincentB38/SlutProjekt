using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PaperFlower : PassivePlant
{
    // Variables for visuals
    [Header("SunflowerSettings")]

    public GameObject paperPrefab;
    public GameObject paperFolder; 

    private void Start()
    {
        try // Tries to find a folder to place its paper instaniated prefabs
        {
            if (paperFolder == null)
            {
                paperFolder = GameObject.Find("PaperFolder");
            }
            
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
       
        StartCoroutine(Generate());
    }

    protected override IEnumerator Generate()
    {
        // Waits at the start for the maximum time
        yield return new WaitForSeconds(timeRangeInterval.y);

        while (true) // Loops for eternity
        {
            try // In case paperFolder is not found
            {
                Instantiate(paperPrefab, transform.position, Quaternion.identity, paperFolder.transform);
            }
            catch(System.Exception e)
            {
                Debug.LogException(e); // Logs exception and breaks the loop
                break;
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(timeRangeInterval.x, timeRangeInterval.y));
            // Randomizes the interval between summons
        }
    }
}
