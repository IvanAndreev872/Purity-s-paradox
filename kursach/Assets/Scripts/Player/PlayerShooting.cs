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
    public bool shootOnCursor = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckShoot())
        {
            if (!shootOnCursor)
            {
                ShootOnDirection();
            }
            else
            {
                ShootOnCursor();
            }
        }
    }

    public void UpdateShooter(PlayerStats playerStats)
    {
        isStaffEquipped = playerStats.isStaffEquipped > 0;
        bulletSpeed = playerStats.staffBulletSpeed;
        shootDelay = playerStats.staffAttackDelay;
    }
    private void ShootOnCursor()
    {
        shootTime = Time.time;
        Vector2 createPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToCursor = (mousePosition - createPosition).normalized;

        float angle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject bullet = Instantiate(bulletPrefab, createPosition, rotation);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = directionToCursor * bulletSpeed;
    }

    private void ShootOnDirection()
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
