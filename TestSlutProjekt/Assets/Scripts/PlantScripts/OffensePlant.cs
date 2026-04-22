using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OffensePlant : Plants
{
    public Transform muzzle;

    // Variables for visuals

    [Header("Range")]
    public float damage;
    public float bulletSpeed;
    public float attackRange;
    public float fireRate;
    public GameObject bulletPrefab;
    public GameObject bulletFolder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (fireRate <= 0) fireRate = 1f; // Just in case, using while loops can cause crashable errors

        StartCoroutine(CheckDistance());
    }

    IEnumerator CheckDistance()
    {
        Vector2 origin = transform.position;
        Vector2 direction = Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, attackRange);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Attack();
        }

        yield return new WaitForSeconds(fireRate);
    }

    public void Attack()
    {
        GameObject tempBullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity, bulletFolder.transform);
        tempBullet.GetComponent<Rigidbody2D>().linearVelocityX = bulletSpeed;
    }
}
