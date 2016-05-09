using UnityEngine;
using System.Collections;

public class PlayerRigBehaviour : MonoBehaviour
{
    public const float DEFAULT_SPEED = 10f;

    public PlayerPathNodeBehaviour initialNode = null;

    private PlayerPathNodeBehaviour nextNode = null;
    private float currentNodeSpeed = 0;


    //Events
    
    void Awake()
    {
        //Start on the initial node
        ChangeNextNode(initialNode);
    }

    void Update()
    {
        //Stop if there is no next node.
        if (nextNode == null)
        {
            return;
        }

        //Move towards the next node
        transform.position = Vector3.MoveTowards(transform.position, nextNode.transform.position, currentNodeSpeed * Time.deltaTime);

        //Change to the next node if we've reached it.
        if (transform.position == nextNode.transform.position)
        {
            ChangeNextNode(nextNode.nextNode);
        }
    }


    //Misc methods

    private void ChangeNextNode(PlayerPathNodeBehaviour node)
    {
        //Change the next node
        nextNode = node;

        //Change the speed
        if (nextNode == null)
        {
            currentNodeSpeed = 0;
        }
        else if (nextNode.useDefaultSpeed)
        {
            currentNodeSpeed = DEFAULT_SPEED;
        }
        else
        {
            currentNodeSpeed = nextNode.speed;
        }
    }
}
