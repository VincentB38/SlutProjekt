using System.Collections.Generic;
using System.Collections;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class Vines : SpecialOffensePlant
{
    public float slownessMultiplier;
    public float radius;
    public float damageCooldown = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHandler>().Speed *= slownessMultiplier; // Changes the enemy's speed by multiplying with the slownessMulti.
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyHandler>().Speed /= slownessMultiplier; // Restores slowed speed to original speed after leaving trigger
        }
    }

    IEnumerator DamageEnemies()
    {
        while (true)
        {
            Vector2 position = transform.position;
            LayerMask mask = LayerMask.GetMask("EnemyLayer");
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, mask);

            foreach (Collider2D hit in hits) // Goes through hits array
            {
                EnemyHandler enemy = hit.GetComponent<EnemyHandler>(); // gets the enemy script
                if (enemy != null)
                // makes sure enemy isn't null
                {
                    enemy.ChangeHealth(-1); // negative as negative and negative is positive
                }
            }

            yield return new WaitForSeconds(damageCooldown);
        }
    }
}
