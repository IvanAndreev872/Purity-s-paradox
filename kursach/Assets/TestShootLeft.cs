using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShootLeft : MonoBehaviour
{
    public float speed = -5f;

    private Rigidbody2D rb;
    private bool is_moving = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (is_moving) 
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        } 
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        is_moving = false;
    }
}
