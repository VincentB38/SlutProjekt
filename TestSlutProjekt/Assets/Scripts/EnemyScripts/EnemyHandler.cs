using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyHandler : MonoBehaviour
{
    public Rigidbody2D EnemyBody { get; private set; }
    private GameObject GameHandler;
    public float Health = 10f; // Stats for the enemies
    public float Speed = 2f;
    public float Damage = 2f;
    public float AttackDistance = 25f;
    public float ActionCooldown = 1f;
    private int Line; // get what lane the enemy is in 
    protected virtual void Awake()
    {
        EnemyBody = GetComponent<Rigidbody2D>();
        GameHandler = GameObject.Find("GameHandler");
    }

    protected virtual void Update()
    {

        print(EnemyBody.position.x);
        if (EnemyBody.position.x <= -10) // if they manage to get past
        {
            Die();
            // add lose heart logic or wtv here
        } 


    }

    public int GetEnemyLane() // Use to find which lane the enemy is on
    {
        return Line;
    }

    public void SetEnemyLane(int Number)
    {
        Line = Number;
    }

    public virtual void TakeDamage(float amount) // Deal Damage
    {
        Health -= amount;
        if (Health <= 0)
            Die();
    }

    protected virtual void Die() // handles an enemy dying
    {
        Destroy(gameObject); // Destroys them
        GameHandler.GetComponent<EnemyLevelSHandler>().OnEnemyDestroyed(); // removes them from the spawner script to keep track of enemies alived
    }
}