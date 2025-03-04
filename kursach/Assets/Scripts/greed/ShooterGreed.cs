using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterGreed : MonoBehaviour
{
    public Transform player;
    public GameObject bullet_prefab;
    public float bullet_speed;
    public float shooter_distance_max;
    public float shooter_distance_min;
    public float fire_delay;

    private float shoot_time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        Vector2 watch_direction = (player.position - transform.position).normalized;
        transform.right = watch_direction;

        if (Vector2.Distance(transform.position, player.position) < shooter_distance_max && 
            Vector2.Distance(transform.position, player.position) > shooter_distance_min)
        {
            if (Time.time > fire_delay + shoot_time)
            {
                shoot_time = Time.time;
                Vector3 create_position = transform.position + transform.right * 2;
                GameObject bullet = Instantiate(bullet_prefab, create_position, transform.rotation);
                Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
                bullet_rb.velocity = transform.right * bullet_speed;
            }
        }
    }
}
