using UnityEngine;
using System.Collections;

public class PlayerPathNodeBehaviour : MonoBehaviour
{
    public PlayerPathNodeBehaviour nextNode = null;
    public bool useDefaultSpeed = true;
    public float speed;


    //Events

    void OnDrawGizmos()
    {
        //Draw a line to the next node
        Gizmos.color = Color.white;
        if (nextNode != null)
        {
            Gizmos.DrawLine(transform.position, nextNode.transform.position);
        }

        //Draw position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
