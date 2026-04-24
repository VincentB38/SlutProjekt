using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
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
            collision.GetComponent<EnemyHandler>().TakeDamage(damage); // Take damage function from Enemy SuperClass

            Die(); // Destroy itself after it hits
        }
    }
}
