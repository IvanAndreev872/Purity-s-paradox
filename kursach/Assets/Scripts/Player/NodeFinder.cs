using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeFinder : MonoBehaviour
{
    [SerializeField] public Node currentNode;
    private void Awake()
    {
       GetStartNode();
    }

    private void Update()
    {
        GetCurrentNode();
    }
    private void GetStartNode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.71f);
        float minDist = 1.0f;
        Node NearestNode = null;
        foreach (Collider2D collider in colliders)
        {
            Node node = collider.GetComponent<Node>();
            if (node != null)
            {
                float curDist = Vector2.Distance(transform.position, node.transform.position);
                if (minDist > curDist)
                {
                    minDist = curDist;
                    NearestNode = node;
                }
            }
        }
        currentNode = NearestNode;
    }

    private void GetCurrentNode()
    {
        if (currentNode != null)
        {
            float minDist = Vector2.Distance(transform.position, currentNode.transform.position);
            Node curNode = null;
            foreach (Node node in currentNode.connections)
            {
                float distBetweenNeig = Vector2.Distance(transform.position, node.transform.position);
                if (distBetweenNeig < minDist)
                {
                    minDist = distBetweenNeig;
                    curNode = node;
                }
            }
            currentNode = curNode;
        }
        GetStartNode();
    }
}
