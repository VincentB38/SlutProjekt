using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BufferEnemy : EnemyHandler
{
    GameObject PlantHolder;
    public float Radius;
    public float BuffCooldown;
    bool isAttacking = false;

    private List<GameObject> EnemiesInRange = new List<GameObject>();
    protected override void Awake()
    {
        this.GetComponent<CircleCollider2D>().radius = Radius;
        base.Awake(); // call Enemy Awake

        PlantHolder = GameObject.Find("PlantHolder");

        StartCoroutine(CheckPlantDistance());
        StartCoroutine(HealEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckPlantDistance()
    {
        while (true)
        {
            Vector2 origin = transform.position; // Gets origin
            Vector2 direction = Vector2.left; // Direction

            LayerMask mask = LayerMask.GetMask("PlantLayer"); // so it ignores everything that isn't in the plant layer
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, AttackDistance, mask);

            if (hit.collider != null && hit.collider.CompareTag("Plant")) // Checks if it is null and an enemy
            {
                Plants plant = hit.collider.GetComponent<Plants>();

                print(plant.GetLane());

                if (plant.GetLane() == this.GetEnemyLane()) // just extra to make sure the plant is on the same lane as the enemy
                {
                    plant.ChangeHealth(-Damage);
                    isAttacking = true;
                    yield return new WaitForSeconds(ActionCooldown); // wait action coolldown as its doing the action
                }
            }
            else
            {
                isAttacking = false;
            }

            yield return new WaitForSeconds(0.1f); // Wait shorter amount of times as it's checking distance
        }
    }

    IEnumerator HealEnemies()
    {
        while (true)
        {
            foreach (GameObject Enemy in EnemiesInRange)
            {
                //Enemy.GetComponent<EnemyHandler>().
            }
            yield return new WaitForSeconds(0.1f); // Wait shorter amount of times as it's checking distance
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject Object = collision.gameObject;

        if (Object.layer == LayerMask.GetMask("EnemyLayer"))
        {
            if (!EnemiesInRange.Contains(Object))
            {
                EnemiesInRange.Add(Object);
            }
        }
    }
}
