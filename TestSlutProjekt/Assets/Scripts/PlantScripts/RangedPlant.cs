using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangedPlant : AggresivePlant
{
    [Header("General")]
    public string plantName;
    public int plantPrice;
    public float plantHealth;

    public Transform muzzle;

    // Variables for visuals

    [Header("Range")]
    public float damage;
    public float bulletSpeed;
    public float attackRange;
    public bool isInRange;
    public float fireRate;
    public GameObject bulletPrefab;
    public GameObject bulletFolder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetValues(plantName, plantPrice, plantHealth, transform);

        if (fireRate <= 0) fireRate = 1f; // Just in case, using while loops can cause crashable errors
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            // Start attacking // CHANGE THE TIMER FROM A COUROUTINE TO A MANUAL IF TIMER CUZ I NEED TO CHECK
        }
    }

    IEnumerator FireRate()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(fireRate);
        }
    }

    public override void Attack()
    {
        GameObject tempBullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity, bulletFolder.transform);
        tempBullet.GetComponent<Rigidbody2D>().linearVelocityX = bulletSpeed;
    }
}
