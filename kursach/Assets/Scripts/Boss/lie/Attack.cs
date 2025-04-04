using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float fireDelay;
    public float backAttackChance;
    public float backAttackDelay;

    private float shootTime;
    private float backAttackTime;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Character").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 watch_direction = (player.position - transform.position).normalized;
        transform.right = watch_direction;
        if (Time.time > fireDelay + shootTime)
        {
            RegularShoot();
        }

        if ((Random.value < backAttackChance) && (Time.time > backAttackTime + backAttackDelay)) 
        { 
            backAttackTime = Time.time;
            KnifeInTheBack();
        }
    }

    void RegularShoot()
    {
        shootTime = Time.time;
        Vector3 create_position = transform.position + transform.right * 2; 
        ShootFromPosition(create_position);
    }

    void KnifeInTheBack()
    {
        Vector3 create_position = player.position + transform.right * 5;
        transform.right *= -1;
        ShootFromPosition(create_position);
    }

    void ShootFromPosition(Vector3 create_position)
    {
        GameObject bullet = Instantiate(bulletPrefab, create_position, transform.rotation);
        Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
        bullet_rb.velocity = transform.right * bulletSpeed;
    }
}
