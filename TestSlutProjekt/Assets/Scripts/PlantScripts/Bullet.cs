using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage = 0;
    public float timeUntilDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DeathTimer());
    }

    IEnumerator DeathTimer() // Initilize countdown until destruction
    {
        yield return new WaitForSeconds(timeUntilDeath);
        Die();
    }

    void Die() // Destroy
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // Checks if collision is with Enemy
        {
            collision.GetComponent<EnemyHandler>().ChangeHealth(damage); // Take damage function from Enemy SuperClass

            Die(); // Destroy itself after it hits
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
