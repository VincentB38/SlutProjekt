using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangedOffensePlant : Plants
{
    public Transform muzzle;

    // Variables for visuals

    [Header("Range")]
    public float damage;
    public float bulletSpeed;
    public float attackRange;
    public float fireRate;
    public GameObject bulletPrefab;
    GameObject bulletFolder;
    public Sprite bulletImage;

    public bool isMelee = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletFolder = GameObject.Find("BulletFolder");
        if (fireRate <= 0) fireRate = 1f; // Sets fire rate to above 0 just in case

        StartCoroutine(CheckDistance());
    }

    IEnumerator CheckDistance()
    {
        while (true)
        {
            Vector2 origin = transform.position; // Gets origin
            Vector2 direction = Vector2.right; // Direction

            LayerMask mask = LayerMask.GetMask("EnemyLayer");
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, attackRange, mask);
            Debug.DrawRay(origin, direction, Color.green);

            if (hit.collider != null && hit.collider.CompareTag("Enemy")) // Checks if it's not null and an enemy
            {
                Attack();
            }

            yield return new WaitForSeconds(fireRate); // Wait for a set time
        }
    }

    public void Attack() // Attack Function
    {
        // Creates bullet, origin and parent
        GameObject tempBullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity, bulletFolder.transform);
        tempBullet.GetComponent<Rigidbody2D>().linearVelocityX = bulletSpeed; // Sets speed of bullet
        tempBullet.GetComponent<Bullet>().SetDamage(damage);
        tempBullet.GetComponent<SpriteRenderer>().sprite = bulletImage;

        tempBullet.GetComponent<SpriteRenderer>().enabled = isMelee; // Hides bullet in case it is melee.
    }

    public override float GetDamage()
    {
        return damage;
    }
}
