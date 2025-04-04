using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float shootDelay;
    private float shootTime;
    public bool isStaffEquipped;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckShoot())
        {
            Shoot();
        }
    }

    public void UpdateShooter(PlayerStats playerStats)
    {
        isStaffEquipped = playerStats.isStaffEquipped > 0;
        bulletSpeed = playerStats.staffBulletSpeed;
        shootDelay = playerStats.staffAttackDelay;
    }

    private void Shoot()
    {
        shootTime = Time.time;
        Vector3 createPosition = transform.position;
        GameObject bullet = Instantiate(bulletPrefab, createPosition, transform.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = transform.right * bulletSpeed;
    }

    private bool CheckShoot()
    {
        return (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.M))
                && Time.timeScale != 0
                && Time.time > shootDelay + shootTime
                && isStaffEquipped;
    }
}
