using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public Rigidbody2D EnemyBody { get; private set; }
    private GameObject GameHandler;
    public float Health = 10f; // Stats for the enemies
    public float Speed = 2f;
    public float Damage = 2f;
    public float AttackDistance = 10f;
    public int Line; // get what lane the enemy is in 
    protected virtual void Awake()
    {
        EnemyBody = GetComponent<Rigidbody2D>();
        GameHandler = GameObject.Find("GameHandler");
    }

    protected virtual void Update()
    {
        // Baslogik kan ligga här, t.ex. allmän rörelse eller animation
        EnemyBody.linearVelocity = new Vector2(-Speed, 0);
    }

    public int GetEnemyLane() // Use to find which lane the enemy is on
    {
        return Line;
    }
    public virtual void TakeDamage(float amount) // Deal Damage
    {
        Health -= amount;
        if (Health <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
        GameHandler.GetComponent<EnemyLevelSHandler>().OnEnemyDestroyed();
    }
}
