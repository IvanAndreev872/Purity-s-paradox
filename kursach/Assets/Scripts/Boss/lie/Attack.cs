using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Transform player;
    public GameObject bullet_prefab;
    public float bullet_speed;
    public float fire_delay;
    public float back_attack_chance;

    private float shoot_time;
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
        if (Time.time > fire_delay + shoot_time)
        {
            RegularShoot();
        }

        if (Random.value < back_attack_chance) 
        { 
            KnifeInTheBack();
        }
    }

    void RegularShoot()
    {
        shoot_time = Time.time;
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
        GameObject bullet = Instantiate(bullet_prefab, create_position, transform.rotation);
        Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
        bullet_rb.velocity = transform.right * bullet_speed;
    }
}
