using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BufferEnemy : EnemyHandler
{
    GameObject PlantHolder;
    public float Radius;
    public float BuffCooldown;
    public float HealAmount;
    bool isAttacking = false;
    protected override void Awake()
    {
        this.GetComponent<CircleCollider2D>().radius = Radius;
        base.Awake(); // call Enemy Awake

        PlantHolder = GameObject.Find("PlantHolder");

        StartCoroutine(CheckPlantDistance());
        StartCoroutine(HealEnemies());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        EnemyBody.linearVelocity = isAttacking ? Vector2.zero : new Vector2(-Speed, 0); // if isattacking is false then no movement else the base movement
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
            Vector2 position = transform.position;
            LayerMask mask = LayerMask.GetMask("EnemyLayer"); // the enemy later

            Collider2D[] hits = Physics2D.OverlapCircleAll(position, Radius, mask); // checks in a cirular to see if an enemy is near it

            foreach (Collider2D hit in hits) // goes through the hits
            {
                EnemyHandler enemy = hit.GetComponent<EnemyHandler>(); // gets the enemy script
                if (enemy != null && enemy.gameObject != gameObject) // makes sure enemy isn't null and the enemy isn't itself (prevent self healing)
                {
                    enemy.ChangeHealth(-HealAmount); // negative as negative and negative is positive
                }
            }

            yield return new WaitForSeconds(BuffCooldown); // wait the cooldown
        }
    }
}
