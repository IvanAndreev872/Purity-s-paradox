using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InaccurateShooter : MonoBehaviour
{
    public Transform player;

    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float shooterDistanceMax;
    public float shooterDistanceMin;
    public float fireDelay;
    public float inaccuracy;

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
                Vector3 createPosition = transform.position;
                float angle = Random.Range(-inaccuracy, inaccuracy);
                Quaternion bulletRotation = transform.rotation * Quaternion.Euler(0, 0, angle);
                GameObject bullet = Instantiate(bulletPrefab, createPosition, bulletRotation);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                bulletRb.velocity = bullet.transform.right * bulletSpeed;
            }
        }
    }
}
