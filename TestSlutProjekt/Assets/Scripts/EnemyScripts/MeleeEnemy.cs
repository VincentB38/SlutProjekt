using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : EnemyHandler
{
    GameObject PlantHolder;
    EnemyHandler enemyHandler;
    public float Damage = 2f;
    public float ActionCooldown = 1f;

    protected override void Awake()
    {
        base.Awake(); // call Enemy Awake

        PlantHolder = GameObject.Find("PlantHolder");

        enemyHandler = this.GetComponent<EnemyHandler>();
        StartCoroutine(CheckDistance());
    }

    protected override void Update()
    {
        base.Update();

        EnemyBody.linearVelocity = enemyHandler.IsPlantNear() ? Vector2.zero : new Vector2(-Speed, 0); // if IsPlantNear is false then no movement else the base movement
    }

    IEnumerator CheckDistance()
    {
        while (true)
        {
            if (enemyHandler.IsPlantNear()) // if a Plant is near
            {
                enemyHandler.plant.ChangeHealth(-Damage); // get the plant that is near and then deal damage
            }
            
            yield return new WaitForSeconds(ActionCooldown); // wait action coolldown as its doing the action
        }
    }
}
