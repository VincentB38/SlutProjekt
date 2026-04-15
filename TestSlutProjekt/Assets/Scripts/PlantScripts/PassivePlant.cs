using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PassivePlant : Plants
{
    // Interval for the coroutine which will be randomized
    [SerializeField] protected Vector2 timeRangeInterval = new Vector2(1f, 2f);

    protected virtual IEnumerator Generate()
    {
        return null;
    }
}
