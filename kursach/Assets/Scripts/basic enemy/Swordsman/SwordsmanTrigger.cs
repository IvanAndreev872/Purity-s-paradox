using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanTrigger : MonoBehaviour
{
    public bool InTrigger = false;
    public bool inTriggerPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            InTrigger = true;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            inTriggerPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            InTrigger = false;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            inTriggerPlayer = false;
        }
    }
}
