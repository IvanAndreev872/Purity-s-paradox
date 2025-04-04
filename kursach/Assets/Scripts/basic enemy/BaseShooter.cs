    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShooter : MonoBehaviour
{
    public Transform player;

    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float shooterDistanceMax;
    public float shooterDistanceMin;
    public float fireDelay;

    private float shootTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Character").transform;
    }

    // Update is called once per frame
    void Update()
    {   
        Vector2 watchDirection = (player.position - transform.position).normalized;
        transform.right = watchDirection;

        if (Vector2.Distance(transform.position, player.position) < shooterDistanceMax && 
            Vector2.Distance(transform.position, player.position) > shooterDistanceMin)
        {
            if (Time.time > fireDelay + shootTime)
            {
                shootTime = Time.time;
                Vector3 createPosition = transform.position + transform.right * 2;
                GameObject bullet = Instantiate(bulletPrefab, createPosition, transform.rotation);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                bulletRb.velocity = transform.right * bulletSpeed;
            }
        }
    }
}
