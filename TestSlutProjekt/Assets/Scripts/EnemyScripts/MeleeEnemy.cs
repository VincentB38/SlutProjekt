using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : EnemyHandler
{
    GameObject PlantHolder;
    bool isAttacking = false;

    protected override void Awake()
    {
        base.Awake(); // call Enemy Awake

        PlantHolder = GameObject.Find("PlantHolder");

        StartCoroutine(CheckDistance());
    }

    protected override void Update()
    {
        base.Update();

        EnemyBody.linearVelocity = isAttacking ? Vector2.zero : new Vector2(-Speed, 0); // if isattacking is false then no movement else the base movement
    }

    IEnumerator CheckDistance()
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
                    print("Hit");
                    plant.ChangeHealth(-Damage);
                    isAttacking = true;
                    yield return new WaitForSeconds(ActionCooldown); // wait action coolldown as its doing the action
                }
            }
            else
            {
                isAttacking = false;
                print(hit.collider);
            }

            yield return new WaitForSeconds(0.1f); // Wait shorter amount of times as it's checking distance
        }
    }
}
