using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float camera_speed;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
    }
        
    // Update is called once per frame
    void Update()
    {
        Vector3 new_position = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, new_position, camera_speed * Time.deltaTime);
    }
}
