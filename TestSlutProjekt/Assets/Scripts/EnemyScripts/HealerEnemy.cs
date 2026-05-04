using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class HealerEnemy : EnemyHandler
{
    GameObject PlantHolder;
    public float Radius;
    public float BuffCooldown;
    public float HealAmount;

    EnemyHandler enemyHandler;

    protected override void Awake()
    {
        this.GetComponent<CircleCollider2D>().radius = Radius;
        base.Awake(); // call Enemy Awake

        PlantHolder = GameObject.Find("PlantHolder");

        enemyHandler = this.GetComponent<EnemyHandler>();
        StartCoroutine(HealEnemies());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        EnemyBody.linearVelocity = enemyHandler.IsPlantNear() ? Vector2.zero : new Vector2(-Speed, 0); // if IsPlantNear is false then no movement else the base movement
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
                if (enemy != null && enemy.gameObject != gameObject && enemy.GetComponent<HealerEnemy>() == null) 
                    // makes sure enemy isn't null and the enemy isn't itself (prevent self healing) and also checks so the enemy isn't a buffer too (prevents buffers buffing eachother)
                {
                    enemy.ChangeHealth(-HealAmount); // negative as negative and negative is positive
                }
            }

            yield return new WaitForSeconds(BuffCooldown); // wait the cooldown
        }
    }
}
