using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardiansColumn : MonoBehaviour
{
    public bool InTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            InTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            InTrigger = false;
        }
    }
}
