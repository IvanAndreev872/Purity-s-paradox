using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node camefrom;
    public List<Node> connections;

    public float gScore;
    public float hScore;

    public float fScore()
    {
        return gScore + hScore;
    }
/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (connections.Count > 0)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                Gizmos.DrawLine(transform.position, connections[i].transform.position);
            }
        }
    }*/
}
