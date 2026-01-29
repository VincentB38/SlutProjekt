using UnityEngine;

public class MeleeEnemy : EnemyHandler
{
    GameObject PlantHolder;

    protected override void Awake()
    {
        base.Awake(); // call Enemy Awake

        PlantHolder = GameObject.Find("PlantHolder");
    }

    protected override void Update()
    {
        bool isAttacking = false;

        foreach (Transform plantTransform in PlantHolder.transform)
        {
            GameObject plant = plantTransform.gameObject;

            float distance = Mathf.Abs(plant.transform.position.x - transform.position.x);

            if (distance <= AttackDistance)
            {
                Debug.Log("Attacking");
                isAttacking = true;
                break;
            }
        }

        EnemyBody.linearVelocity = isAttacking ? Vector2.zero : new Vector2(-Speed, 0); // if isattacking is false then no movement else the base movement
    }
}
