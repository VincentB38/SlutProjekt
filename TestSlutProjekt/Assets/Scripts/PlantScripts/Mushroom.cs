using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : SpecialOffensePlant
{
    [SerializeField] private float poisonCooldown;
    [SerializeField] private float aliveTimer;
    [SerializeField] private float rayLength = 1.5f;

    private bool isDead;

    private EnemyHandler enemy;

    public override void ChangeHealth(float amount) // Overriding the change health system from base class
    {
        base.health += amount;

        if (health <= 0 && !isDead) // If health goes below or equal to zero, it dies.
        {
            isDead = true;
            base.tile.isOccupied = false; // Makes the tile unoccupied for Vincents code
            LayerMask mask = LayerMask.GetMask("EnemyLayer"); // Converts layer name to mask number
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, rayLength, mask); // Sends out raycast

            if (hit.collider.CompareTag("Enemy")) enemy = hit.collider.GetComponent<EnemyHandler>(); // Makes enemy var. to corresponding enemy
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(DamageEnemy());

            StartCoroutine(deathTimer());
        }
    }

    IEnumerator deathTimer() // Activates a timer to remove itself after a while, which stops the poison
    {
        yield return new WaitForSeconds(aliveTimer);
        Destroy(gameObject);
    }

    IEnumerator DamageEnemy() // Damages enemy with a set time cooldown, POISON
    {
        while (true)
        {
            Debug.Log("Enemy is: " + enemy);
            enemy.ChangeHealth(-damage);
            yield return new WaitForSeconds(poisonCooldown);
        }
    }
}
