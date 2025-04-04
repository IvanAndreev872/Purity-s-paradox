using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTrigger : MonoBehaviour
{
    public bool InTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            InTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            InTrigger = false;
        }
    }
}
