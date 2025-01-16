using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet_prefab;
    public float bullet_speed;
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

    private void Shoot()
    {
        Vector3 create_position = transform.position + transform.right * 1.2f;
        GameObject bullet = Instantiate(bullet_prefab, create_position, transform.rotation);
        Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
        bullet_rb.velocity = transform.right * bullet_speed;
    }

    private bool CheckShoot()
    {
        return Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0);
    }
}
