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

    private HashSet<EnemyHandler> slowedEnemies = new HashSet<EnemyHandler>();

    private void Start()
    {
        StartCoroutine(DamageEnemies()); // Starts checking for enemies
    }

    IEnumerator DamageEnemies()
    {
        while (true)
        {
            Vector2 position = transform.position;
            LayerMask mask = LayerMask.GetMask("EnemyLayer");
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, mask);

            HashSet<EnemyHandler> currentEnemies = new HashSet<EnemyHandler>();

            foreach (Collider2D hit in hits)
            {
                EnemyHandler enemy = hit.GetComponent<EnemyHandler>();
                if (enemy != null)
                {
                    currentEnemies.Add(enemy);

                    // DAMAGE (always applies)
                    enemy.ChangeHealth(-damage);

                    // SLOW (only apply once)
                    if (!slowedEnemies.Contains(enemy))
                    {
                        enemy.Speed *= slownessMultiplier;
                        slowedEnemies.Add(enemy);
                    }
                }
            }

            // REMOVE SLOW from enemies that left
            foreach (EnemyHandler enemy in slowedEnemies)
            {
                if (!currentEnemies.Contains(enemy))
                {
                    enemy.Speed /= slownessMultiplier;
                }
            }

            // Update the tracked enemies
            slowedEnemies = currentEnemies;

            yield return new WaitForSeconds(damageCooldown);
        }
    }
}
