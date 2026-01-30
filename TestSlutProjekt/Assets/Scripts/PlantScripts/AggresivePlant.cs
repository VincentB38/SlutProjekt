using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class AggresivePlant : Plants
{
    private float damage;
    private float attackRange;
    private float fireRate;
    private bool isInRange;

    public virtual void Attack()
    {

    }
}
