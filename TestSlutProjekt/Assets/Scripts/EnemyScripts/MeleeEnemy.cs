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
            Vector2 direction = Vector2.right; // Direction

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, AttackDistance);

            if (hit.collider != null && hit.collider.CompareTag("Plant")) // Checks if it is null and an enemy
            {
                //Action();
                isAttacking = true;
                yield return new WaitForSeconds(ActionCooldown); // wait action coolldown as its doing the action
            }
            else
            {
                isAttacking = false;
            }

            yield return new WaitForSeconds(0.1f); // Wait shorter amount of times as it's checking distance
        }
    }
}
