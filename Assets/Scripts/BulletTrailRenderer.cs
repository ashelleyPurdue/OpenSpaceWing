using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class BulletTrailRenderer : MonoBehaviour
{
    private LineRenderer lineRend;
    private Vector3 lastPos;

    void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        lastPos = transform.position;

        //Configure the line renderer
        lineRend.SetPosition(0, transform.position);
        lineRend.SetPosition(1, transform.position);

        lineRend.SetWidth(transform.localScale.x, 0);
    }

    void Update()
    {
        //Update the trail
        lineRend.SetPosition(0, transform.position);
        lineRend.SetPosition(1, lastPos);

        //Save the last position
        lastPos = transform.position;
    }
}
