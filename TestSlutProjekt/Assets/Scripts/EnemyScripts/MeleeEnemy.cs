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
    }

    protected override void Update()
    {
        base.Update();

        foreach (Transform plantTransform in PlantHolder.transform)
        {
            GameObject plant = plantTransform.gameObject;

            float distance = Mathf.Abs(plant.transform.position.x - transform.position.x);

            if (distance <= AttackDistance && distance > 0)
            {
                Debug.Log("Attacking");
                isAttacking = true;
                break;
            }
        }

        EnemyBody.linearVelocity = isAttacking ? Vector2.zero : new Vector2(-Speed, 0); // if isattacking is false then no movement else the base movement
    }

    IEnumerator CheckDistance()
    {
        while (true)
        {
            Vector2 origin = transform.position; // Gets origin
            Vector2 direction = Vector2.right; // Direction

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, AttackDistance);

            if (hit.collider != null && hit.collider.CompareTag("Plant")) // Checks if it is null and an enemy
            {
               // Attack();
            }

            yield return new WaitForSeconds(ActionCooldown); // Wait for a set time
        }
    }
}
