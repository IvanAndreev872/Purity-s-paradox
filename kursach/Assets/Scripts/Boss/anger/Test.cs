using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AngerStatusController;

public class AngerAttack : MonoBehaviour
{
    private AngerStatusController anger_status_controller;

    public Transform player;

    public GameObject bullet_prefab;
    public float bullet_speed;
    public float shooter_distance_max;
    public float shooter_distance_min;
    public float fire_delay;

    private float shoot_time;
    // Start is called before the first frame update
    void Awake()
    {
        anger_status_controller = GetComponentInParent<AngerStatusController>();
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

    private void OnAngerChanged(AngerStatusController.AngerLevel new_level)
    {
        // Debug.Log("1");
    }

    void OnEnable()
    {
        if (anger_status_controller)
        {
            anger_status_controller.EnragementChanged += OnAngerChanged;
        }   
    }

    void OnDisable()
    {
        if (anger_status_controller)
        {
            anger_status_controller.EnragementChanged -= OnAngerChanged;
        }
    }
}
