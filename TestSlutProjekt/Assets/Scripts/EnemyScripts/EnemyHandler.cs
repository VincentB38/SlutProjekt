using System.Collections;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyHandler : MonoBehaviour
{
    public Rigidbody2D EnemyBody { get; private set; }
    private GameObject GameHandler;
    [SerializeField] private float Health = 10f; // Stats for the enemies
    [SerializeField] private float AttackDistance = 25f;
    public float Speed = 2f;
    private bool IsNear = false; // just a bool
    public Plants plant; // to store the nearby plant
    private int Line; // get what lane the enemy is in 
    protected virtual void Awake()
    {
        EnemyBody = GetComponent<Rigidbody2D>();
        GameHandler = GameObject.Find("GameHandler");
        StartCoroutine(PlantChecker());
    }

    protected virtual void Update()
    {

        if (EnemyBody.position.x <= -10) // if they manage to get past
        {
            Die(); // kill the enemy
            PlayerStats.Instance.ChangeHealth(-1); // take one health 
        } 
    }
    IEnumerator PlantChecker() // function to check if plant is near
    {
        while (true)
        {
            Vector2 origin = transform.position; // Gets origin
            Vector2 direction = Vector2.left; // Direction

            LayerMask mask = LayerMask.GetMask("PlantLayer"); // so it ignores everything that isn't in the plant layer
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, AttackDistance, mask);

            if (hit.collider != null && hit.collider.CompareTag("Plant")) // Checks if it's not null and an enemy
            {
                plant = hit.collider.GetComponent<Plants>();

                if (plant.GetLane() == this.GetEnemyLane() && plant.GetPassThrough() == false) // just extra to make sure the plant is on the same lane as the enemy
                    // passthrough to make sure if the enemy should attack it or ignore it (true = ignore it)
                {
                    IsNear = true;
                }
            }
            else
            {
                IsNear = false;
                plant = null;
            }

            yield return new WaitForSeconds(0.1f); // Wait shorter amount of times as it's checking distance
        }
    }

    public bool IsPlantNear() // used to check if the plant is near
    {
        return IsNear;
    }
    public int GetEnemyLane() // Use to find which lane the enemy is on
    {
        return Line;
    }

    public void SetEnemyLane(int Number)
    {
        Line = Number;
    }

    public virtual void ChangeHealth(float amount) // Change Health
    {
        Health += amount;
        if (Health <= 0)
            Die();
    }
    protected virtual void Die() // handles an enemy dying
    {
        if (gameObject != null)
        {
            Destroy(gameObject); // Destroys them
            GameHandler.GetComponent<EnemyLevelSHandler>().OnEnemyDestroyed(); // removes them from the spawner script to keep track of enemies alived
        }
        else
        {
            Debug.Log("Enemy is already destroyed or wasn't found");
        }
    }
}