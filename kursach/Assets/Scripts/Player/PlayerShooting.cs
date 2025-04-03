using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet_prefab;
    public float bullet_speed;
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
        bullet_speed = playerStats.staffBulletSpeed;
        shootDelay = playerStats.staffAttackDelay;
    }

    private void Shoot()
    {
        shootTime = Time.time;
        Vector3 create_position = transform.position;
        GameObject bullet = Instantiate(bullet_prefab, create_position, transform.rotation);
        Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
        bullet_rb.velocity = transform.right * bullet_speed;
    }

    private bool CheckShoot()
    {
        return (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.M))
                && Time.timeScale != 0
                && Time.time > shootDelay + shootTime
                && isStaffEquipped;
    }
}
