using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 10f;
    public int maxBulletCount = 50;
    public float timeBetweenShots = 0.5f;
    public AudioSource shootingSound;
    public Animator camAnim; 

    private int currentBulletCount;
    private float lastShotTime;
    private bool isRecharging;
    private float rechargingSpeed;

    public EnemyHealth takeDamage;

    private void Start()
    {
        currentBulletCount = maxBulletCount;
        isRecharging = false;
        rechargingSpeed = 1.0f;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time - lastShotTime >= timeBetweenShots)
        {
            Shoot();
            if (shootingSound != null)
            {
                Debug.Log("Playing shooting sound");
                shootingSound.Play();
            }
            if (camAnim != null)
            {
                camAnim.SetTrigger("shake");
            }
        }
    }

    private void Shoot()
    {
        if (currentBulletCount > 0)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 shootingDirection = (mousePosition - firePoint.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = shootingDirection * bulletForce;
                currentBulletCount--;
                lastShotTime = Time.time;
                StartCoroutine(RechargeBullets());
                Destroy(bullet, 2f);
            }
            else
            {
                Debug.LogError("Rigidbody2D component not found on the bullet.");
            }
        }
        else
        {
            isRecharging = true;
        }
    }

    private IEnumerator RechargeBullets()
    {
        while (isRecharging)
        {
            yield return new WaitForSeconds(3.0f);

            if (currentBulletCount < maxBulletCount)
            {
                currentBulletCount += (int)rechargingSpeed;
                currentBulletCount = Mathf.Min(currentBulletCount, maxBulletCount);
            }
            else
            {
                isRecharging = false;
                rechargingSpeed = 0;
            }
        }
    }

    private int GetBulletDamage()
    {
        return 34;
    }
}
